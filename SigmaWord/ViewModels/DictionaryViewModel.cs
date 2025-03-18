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
        private readonly DbService _dbService;
        private readonly SettingsService _settingsService;
        [ObservableProperty]
        public ObservableCollection<Category> categories;
        public DictionaryViewModel(VocabularyService vocabularyService, DbService dbService)
        {
            _dbService = dbService;
            var categoruessDb = _dbService.GetAllCategoriesAsync().Result;
            categories =  new ObservableCollection<Category>(categoruessDb);
            _settingsService = new SettingsService();
        }
        [RelayCommand]
        public async Task GoToWordsPage(string categoryName)
        {
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new WordsViewModel(_dbService);
            var wordsPage = new WordsPage(viewModel, categoryName);
            await Shell.Current.Navigation.PushAsync(wordsPage);
        }
    }
}
