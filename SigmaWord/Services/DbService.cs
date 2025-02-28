using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using SigmaWord.Data.Entities;
using SigmaWord.Models;
using System.Reflection;

namespace SigmaWord.Services
{
    public class DbService
    {
        private readonly SigmaWordDbContext _context;
        public DbService(SigmaWordDbContext context)
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
            await _context.Category.AddAsync(category);
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
            return await _context.Category.ToListAsync();
        }

        // Удаление категории
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Category.Remove(category);
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
        public async Task InitializeStatisticsAsync()
        {
            // Получаем сегодняшнюю дату и дату две недели назад
            var today = DateTime.Today;
            var twoWeeksAgo = today.AddDays(-14);

            // Получаем список дат от двух недель назад до сегодня
            var dates = Enumerable.Range(0, 15)
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

    }
}

        