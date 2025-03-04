using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using SigmaWord.Data;
using SigmaWord.Data.Entities;
using SigmaWord.Models;
using System.Reflection;

namespace SigmaWord.Services
{
    public class DbService
    {
        private readonly AppDbContext _context;
        public DbService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddCardAsync(FlashCard card)
        {
            await _context.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCardAsync(FlashCard card)
        {
            _context.FlashCards.Remove(card);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FlashCard>> GetAllCardsAsync()
        {
            var cards = await _context.FlashCards
                .Include(fc => fc.Categories)
                .Include(fc => fc.ExampleSentences)
                .ToListAsync();
            return cards;
        }

        // Полное удаление базы данных
        public async Task CLEARDatabaseAsync()
        {
            var dbPath = PathDb.GetPath();
            if (File.Exists(dbPath))
            {
                try
                {
                    // Удаляем файл
                    File.Delete(dbPath);
                    await Task.CompletedTask; // Для соблюдения асинхронности
                }
                catch (IOException ex)
                {
                    // Обработка исключений
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
        }
        //Инициализация базы данных. Если базы данных нету или она пустая, заполняем её словами из файлов. 
        public async Task InitializeDatabaseAsync()
        {
            await CLEARDatabaseAsync(); //Очистить всё!! Для тестирования*

            // Проверяем, существует ли база данных
            bool canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
            {
                // Если нет, копируем базу данных из ресурсов
                await CopyDataBase();
            }
            else
            {
                // Проверяем, есть ли карточки в базе данных
                bool hasFlashCards = await _context.FlashCards.AnyAsync();

                if (!hasFlashCards)
                {
                    // Если нет, копируем базу данных из ресурсов
                    await CopyDataBase();
                }
            }
        }


        // Добавление примера слова к карточке
        public async Task AddExampleToCardAsync(FlashCard card, ExampleSentence example)
        {
            card.ExampleSentences.Add(example);
            _context.FlashCards.Update(card);
            await _context.SaveChangesAsync();
        }

        // Добавление новой категории
        public async Task<Category> AddCategoryAsync(string categoryName)
        {
            var category = new Category { Name = categoryName };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Добавление категории к карточке
        public async Task AddCategoryToCardAsync(FlashCard card, Category category)
        {
            card.Categories.Add(category);
            _context.FlashCards.Update(card);
            await _context.SaveChangesAsync();
        }

        // Получение всех категорий
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // Удаление категории
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        // Получение карточки по ID
        public async Task<FlashCard> GetCardByIdAsync(int id)
        {
            return await _context.FlashCards
                .Include(fc => fc.Categories)
                .Include(fc => fc.ExampleSentences)
                .FirstOrDefaultAsync(fc => fc.Id == id);
        }
        public async Task<List<FlashCard>> GetFlashCardsByStatusAsync(WordStatus status)
        {
            if(status == WordStatus.Learning){
                return await _context.FlashCards
                    .Where(fc => fc.Status == status && fc.NextRepeatDate <= DateTime.Now)
                    .Include(fc => fc.ExampleSentences)
                    .ToListAsync();
            }
            if(status == WordStatus.ToLearn)
            {
                return await _context.FlashCards
                    .Where(fc => fc.Status == status)
                    .ToListAsync();
            }
            return await _context.FlashCards
                .Where(fc => fc.Status == status)
                .ToListAsync();
        }
        public async Task<List<FlashCard>> GetFlashCardsByStatusAsync(WordStatus status, int numberOfTake)
        {
            return await _context.FlashCards
                .Where(fc => fc.Status == status)
                .Include(fc => fc.ExampleSentences)
                .Take(numberOfTake)
                .ToListAsync();
        }
        //Метод для добавления в базу данных слов из файла.
        public async Task CopyDataBase()
        {
            try
            {
                // Получаем путь к базе данных на устройстве
                var dbPath = PathDb.GetPath();

                // Получаем поток к файлу из ресурсов
                var assembly = typeof(DbService).Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("SigmaWord.Resources.Words.SigmaWordDb.db"))
                {
                    if (stream == null)
                    {
                        throw new FileNotFoundException("Database file not found in resources.");
                    }

                    using (FileStream fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА ПРИ КОПИРОВАНИИ БАЗЫ ДАННЫХ ИЗ РЕСУРСОВ: {ex.Message}");
            }
        }
        public async Task UpdateFlashCard(FlashCard flashCard)
        {
            _context.FlashCards.Update(flashCard);
            await _context.SaveChangesAsync();
        }
        public async Task<List<FlashCard>> GetWordsByCategoryNameAsync(string categoryName)
        {
            return await _context.FlashCards
             .Where(fc => fc.Categories.Any(c => c.Name == categoryName))
             .ToListAsync();
        }
        public async Task<List<FlashCardDto>> GetWordsDtoByCategoryNameAsync(string categoryName)
        {
            // Пример запроса к базе данных, который возвращает только необходимые поля
            return await _context.FlashCards
                .Where(fc => fc.Categories.Any(c => c.Name == categoryName))
                .Select(fc => new FlashCardDto
                {
                    Word = fc.Word,
                    Translation = fc.Translation
                })
                .ToListAsync();
        }
        public async Task<int> GetNeedToRepeatDataAsync()
        {
            var result = await _context.FlashCards
                    .CountAsync(fc => fc.Status == WordStatus.Learning && fc.NextRepeatDate <= DateTime.Now);
            return result;
        }
        public async Task<List<DailyStatistics>> GetDailyStatisticsAsync()
        {
            return await _context.DailyStatistics.ToListAsync();
        }
        public async Task AddStatisticsAsync(DailyStatistics statistics) 
        {
            await _context.DailyStatistics.AddAsync(statistics);
            await _context.SaveChangesAsync();
        }
        public async Task<List<DailyStatistics>> GetDailyStatisticsAsync(int days)
        {
            // Получаем дату, начиная с которой будем делать выборку
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            // Возвращаем последние 'days' записей, начиная с сегодняшнего дня
            var result = await _context.DailyStatistics
                .Where(s => s.Date >= cutoffDate)
                .OrderByDescending(s => s.Date) // Сортируем по убыванию даты
                .Take(days) // Берем только последние 'days' записей
                .ToListAsync();

            return result;
        }
        public async Task<DailyStatistics> GetTodayStatisticsAsync()
        {
            return await _context.DailyStatistics.FirstOrDefaultAsync(ds => ds.Date == DateTime.Today);
        }
        public async Task InitializeStatisticsAsync(int days)
        {
            // Получаем сегодняшнюю дату и дату две недели назад
            var today = DateTime.Today;
            var twoWeeksAgo = today.AddDays(-days);

            // Получаем список дат от двух недель назад до сегодня
            var dates = Enumerable.Range(0, days)
                .Select(i => today.AddDays(-i))
                .ToList();

            // Получаем существующие статистики из базы данных
            var existingStatistics = await _context.DailyStatistics
                .Where(stat => dates.Contains(stat.Date))
                .ToListAsync();

            // Создаем список новых статистик для добавления
            var newStatistics = new List<DailyStatistics>();

            foreach (var date in dates)
            {
                // Проверяем, существует ли уже статистика для данной даты
                if (!existingStatistics.Any(stat => stat.Date.Date == date))
                {
                    // Если нет, создаем новую запись с нулевыми значениями
                    newStatistics.Add(new DailyStatistics
                    {
                        Date = date,
                        TotalRepeats = 0,
                        TotalWordsStudied = 0
                    });
                }
            }

            // Добавляем новые записи в контекст
            if (newStatistics.Any())
            {
                await _context.DailyStatistics.AddRangeAsync(newStatistics);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddCategoryToLearning(Category category)
        {
            // Обновляем статус карточек, относящихся к данной категории и имеющих статус Unknown
            await _context.FlashCards
                .Where(fc => fc.Categories.Any(c => c.Id == category.Id) && fc.Status == WordStatus.Unknown)
                .ForEachAsync(fc => fc.Status = WordStatus.ToLearn);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryFromLearning(Category category)
        {
            // Обновляем статус карточек, относящихся к данной категории и имеющих статус ToLearn
            await _context.FlashCards
                .Where(fc => fc.Categories.Any(c => c.Id == category.Id) && fc.Status == WordStatus.ToLearn)
                .ForEachAsync(fc => fc.Status = WordStatus.Unknown);

            await _context.SaveChangesAsync();
        }
        public async Task AddStatistics(string statisticName)
        {
            // Получаем сегодняшнюю дату без времени
            var today = DateTime.Today;

            // Ищем статистику за сегодняшний день
            var dailyStats = await _context.DailyStatistics
                .FirstOrDefaultAsync(stats => stats.Date == today);

            // Если запись найдена, увеличиваем TotalRepeats
            if (dailyStats != null)
            {
                switch (statisticName)
                {
                    case "TotalRepeats":
                        dailyStats.TotalRepeats += 1; break;
                    case "TotalWordsStudied":
                        dailyStats.TotalWordsStudied += 1; break;
                    case "TotalWordsStarted":
                        dailyStats.TotalWordsStarted += 1; break;
                    case "TotalKnownWords":
                        dailyStats.TotalKnownWords += 1; break;
                }
            }
            else
            {
                // Если записи нет, создаем новую
                dailyStats = new DailyStatistics
                {
                    Date = today,
                };
                switch (statisticName)
                {
                    case "TotalRepeats":
                        dailyStats.TotalRepeats += 1; break;
                    case "TotalWordsStudied":
                        dailyStats.TotalWordsStudied += 1; break;
                    case "TotalWordsStarted":
                        dailyStats.TotalWordsStarted += 1; break;
                    case "TotalKnownWords":
                        dailyStats.TotalKnownWords += 1; break;
                }

                // Добавляем новую запись в контекст
                await _context.DailyStatistics.AddAsync(dailyStats);
            }

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();
        }
        
        public async Task<UserSettings> GetSettings()
        {
            var settings = await _context.UserSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                var newSettings = new UserSettings { DailyWordGoal = 5, SelectedTheme = Themes.DarkPurple.ToString() };
                _context.UserSettings.Add(newSettings);
                await _context.SaveChangesAsync();
                return newSettings;
            }
            return settings;
        }
        public async Task UpdateSettings(UserSettings newSettings)
        {
            var settings = await _context.UserSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                _context.UserSettings.Add(newSettings);
            }
            else
            {
                if (newSettings.SelectedTheme != settings.SelectedTheme) 
                    settings.SelectedTheme = newSettings.SelectedTheme;
                if (newSettings.DailyWordGoal != settings.DailyWordGoal)
                    settings.DailyWordGoal = newSettings.DailyWordGoal;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetStatisticsCountForPeriod(TypeStatisticses type, int days)
        {
            // Получаем текущую дату (сегодня)
            var endDate = DateTime.Now.Date; // Сегодняшняя дата без времени
                                             // Вычисляем дату, которая была `days` дней назад
            var startDate = endDate.AddDays(-days); // Дата `days` дней назад

            // Фильтруем записи за период от `startDate` до `endDate`
            var query = _context.DailyStatistics
                                .Where(s => s.Date >= startDate && s.Date <= endDate);

            // Выбираем нужное поле в зависимости от типа статистики
            switch (type)
            {
                case TypeStatisticses.TotalWordsStarted:
                    return await query.SumAsync(s => s.TotalWordsStarted);

                case TypeStatisticses.TotalRepeats:
                    return await query.SumAsync(s => s.TotalRepeats);

                case TypeStatisticses.TotalWordsStudied:
                    return await query.SumAsync(s => s.TotalWordsStudied);

                case TypeStatisticses.TotalKnownWords:
                    return await query.SumAsync(s => s.TotalKnownWords);
            }
            return 0;
        }
        public async Task<int> GetStatisticsCountForAllTime(TypeStatisticses type)
        {
            // Выбираем нужное поле в зависимости от типа статистики
            switch (type)
            {
                case TypeStatisticses.TotalWordsStarted:
                    return await _context.DailyStatistics.SumAsync(s => s.TotalWordsStarted);

                case TypeStatisticses.TotalRepeats:
                    return await _context.DailyStatistics.SumAsync(s => s.TotalRepeats);

                case TypeStatisticses.TotalWordsStudied:
                    return await _context.DailyStatistics.SumAsync(s => s.TotalWordsStudied);

                case TypeStatisticses.TotalKnownWords:
                    return await _context.DailyStatistics.SumAsync(s => s.TotalKnownWords);

                default:
                    throw new ArgumentException("Invalid statistics type", nameof(type));
            }
        }
    }
}

        