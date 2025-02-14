using SigmaWord.Models;
using SQLite;

namespace SigmaWord.Services
{
    public class LocalDBService
    {
        private const string DBName = "SigmaWordDB.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DBName));
            _connection.CreateTablesAsync<FlashCard, Translation, ExampleSentence>().Wait();
        }
        //Получаем все флэш-карточки
        public async Task<List<FlashCard>> GetFlashCards()
            => await _connection.Table<FlashCard>().ToListAsync();

        //Получаем флэш-карточку по Id
        public async Task<FlashCard> GetById(Guid id)
            => await _connection.Table<FlashCard>().FirstOrDefaultAsync(x => x.Id == id);

        //Создаем флэш=карточку
        public async Task Create(FlashCard flashCard)
            => await _connection.InsertAsync(flashCard);

        //Обновляем флэш-карточку
        public async Task Update(FlashCard flashCard)
            => await _connection.UpdateAsync(flashCard);

        //Удаляем флэш-карточку
        public async Task Delete(Guid id)
            => await _connection.DeleteAsync(id);

        // Получаем все примеры предложений для флэш-карточки
        public async Task<List<ExampleSentence>> GetExampleSentences(Guid flashCardId)
            => await _connection.Table<ExampleSentence>().Where(x => x.FlashCardId == flashCardId).ToListAsync();

        // Создаем пример предложения
        public async Task CreateExampleSentence(ExampleSentence exampleSentence)
            => await _connection.InsertAsync(exampleSentence);

        // Удаляем пример предложения
        public async Task DeleteExampleSentence(Guid id)
           => await _connection.DeleteAsync<ExampleSentence>(id);

        // Получаем все переводы для флэш-карточки
        public async Task<List<Translation>> GetTranslations(Guid flashCardId)
            => await _connection.Table<Translation>().Where(x => x.FlashCardId == flashCardId).ToListAsync();

        // Создаем перевод
        public async Task CreateTranslation(Translation translation)
            => await _connection.InsertAsync(translation);

        // Удаляем перевод
        public async Task DeleteTranslation(int id)
            => await _connection.DeleteAsync<Translation>(id);
    }
}
