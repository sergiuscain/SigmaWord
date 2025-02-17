using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
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
            var allCard = await _dbService.GetAllCardsAsync();
            var card = await _dbService.GetWordsByCategoryNameAsync(categoryName);
        }

    }
}
