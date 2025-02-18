using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;

namespace SigmaWord.ViewModels
{
    public partial class WordsViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        [ObservableProperty]
        public string categoryName;
        [ObservableProperty]
        public List<FlashCard> words;
        public WordsViewModel(DbService dbService)
        {
            _dbService = dbService;
        }
        public async Task LoadWords()
        {
            var words = await _dbService.GetWordsByCategoryNameAsync(categoryName);
            Words = words;
        }
        [RelayCommand]
        public async Task DoSome()
        {
            Console.WriteLine("DO SOME!! DO SOME DO SOME!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}
