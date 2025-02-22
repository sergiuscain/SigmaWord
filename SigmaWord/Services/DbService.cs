using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using SigmaWord.Data.Entities;
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
    }
}

        