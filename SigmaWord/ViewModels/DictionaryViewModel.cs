using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using SigmaWord.Views;
using System.Reflection;

namespace SigmaWord.ViewModels
{
    public partial class DictionaryViewModel : ObservableObject
    {
        private readonly VocabularyService _vocabularyService;
        private readonly DbService _dbService;
        public DictionaryViewModel(VocabularyService vocabularyService, DbService dbService)
        {
            _vocabularyService = vocabularyService;
            _dbService = dbService;
        }
        [RelayCommand]
        public async Task GoToWordsPage(string categoryName)
        {
            await _dbService.InitializeDatabaseAsync();
            var allCard = await _dbService.GetAllCardsAsync();
            var card = await _dbService.GetWordsByCategoryNameAsync(categoryName);

            if (card != null)
            {
                // Создаем новую страницу и устанавливаем свойство
                var viewModel = new WordsViewModel(_dbService);
                var wordsPage = new WordsPage(viewModel, categoryName);
                await Shell.Current.Navigation.PushAsync(wordsPage);
            }
            else
            {
                // Обработка случая, когда карточек нет
                Console.WriteLine($"Нет карточек для категории: {categoryName}");
            }
        }

    }
}
