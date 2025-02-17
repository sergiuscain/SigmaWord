using Microsoft.EntityFrameworkCore;
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
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        //Инициализация базы данных. Если базы данных нету или она пустая, заполняем её словами из файлов. 
        public async Task InitializeDatabaseAsync()
        {
            // Проверяем, существует ли база данных
            if (!await _context.Database.CanConnectAsync())
            {
                // Если нет, создаем базу данных
                await _context.Database.EnsureCreatedAsync();
                //await AddFromFile("Oxfard3000_b1Ready");
                //await AddFromFile("Oxfard3000_b2Ready");
                //await AddFromFile("Oxford3000_a1Ready");
                //await AddFromFile("Oxford3000_a2Ready");
            }
            else
            {
                // Проверяем, есть ли карточки в базе данных
                if (!await _context.FlashCards.AnyAsync())
                {
                    // Если карточек нет, добавляем данные из файлов ресурсов
                    await AddFromFile("Oxfard3000_b1Ready");
                    //await AddFromFile("Oxfard3000_b2Ready");
                    //await AddFromFile("Oxford3000_a1Ready");
                    //await AddFromFile("Oxford3000_a2Ready");
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
        public async Task AddFromFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"SigmaWord.Resources.Words.{fileName}.txt";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            //Если категории с именем файла нету, создаем её
            if (_context.Category.Any(c => c.Name == resourceName));
            {
                await AddCategoryAsync(fileName);
            }
            while (!reader.EndOfStream)
            {
                //absolutely # абсолютно # I absolutely agree. # Я абсолютно согласен.
                var data = (await reader.ReadLineAsync()).Split("#");
                FlashCard card = new FlashCard()
                {
                    Word = data[0],
                    Translation = data[1],
                    ExampleSentences = new List<ExampleSentence>()
                    {
                        new ExampleSentence
                        {
                            Sentence = data[2],
                            Translation = data[3],
                        }
                    },
                    Status = WordStatus.Unknown,
                    RequiredRepetitions = 10,
                    CurrentRepetitions = 0,
                };
                await AddCardAsync(card);
                var category = await _context.Category.FirstAsync(card => card.Name == fileName);
                await AddCategoryToCardAsync(card, category);
            }
        }

        public async Task<List<FlashCard>> GetWordsByCategoryIdAsync(int categoryId)
        {
            return await _context.FlashCards
             .Where(fc => fc.Categories.Any(c => c.Id == categoryId))
             .ToListAsync();
        }
    }
}

        