using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;

namespace SigmaWord.ViewModels
{
    public partial class DictionaryViewModel : ObservableObject
    {
        private readonly VocabularyService _vocabularyService;
        private readonly DbService _dbService;
        [ObservableProperty]
        string flashCards;
        public DictionaryViewModel(VocabularyService vocabularyService, DbService dbService)
        {
            _vocabularyService = vocabularyService;
            _dbService = dbService;
        }


    }
}
