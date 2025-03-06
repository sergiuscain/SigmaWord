using CommunityToolkit.Mvvm.ComponentModel;
using SigmaWord.Data.Entities;

using SigmaWord.Services;
using System.Collections.ObjectModel;

namespace SigmaWord.ViewModels
{
    public partial class WordsViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        [ObservableProperty]
        public string categoryName;
        [ObservableProperty]
        public ObservableCollection<FlashCard> words;
        public WordsViewModel(DbService dbService)
        {
            _dbService = dbService;
        }
        public async Task LoadWords()
        {
            var words = await _dbService.GetWordsByCategoryNameAsync(CategoryName);
            Words = new ObservableCollection<FlashCard>(words);
        }
    }
}
