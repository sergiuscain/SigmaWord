using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using SigmaWord.Views;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SigmaWord.ViewModels
{
    public partial class DictionaryViewModel : ObservableObject
    {
        private readonly VocabularyService _vocabularyService;
        private readonly DbService _dbService;
        [ObservableProperty]
        public ObservableCollection<Category> categories;
        public DictionaryViewModel(VocabularyService vocabularyService, DbService dbService)
        {
            _vocabularyService = vocabularyService;
            _dbService = dbService;
            categories = new ObservableCollection<Category>(_dbService.GetAllCategoriesAsync().Result);
        }
        [RelayCommand]
        public async Task GoToWordsPage(string categoryName)
        {
            await _dbService.InitializeDatabaseAsync();
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new WordsViewModel(_dbService);
            var wordsPage = new WordsPage(viewModel, categoryName);
            await Shell.Current.Navigation.PushAsync(wordsPage);
        }

    }
}
