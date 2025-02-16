using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;

namespace SigmaWord.ViewModels
{
    public partial class DictionaryViewModel : ObservableObject
    {
        private readonly VocabularyService _vocabularyService;
        [ObservableProperty]
        List<FlashCard> flashCards;
        public DictionaryViewModel(VocabularyService vocabularyService)
        {
            _vocabularyService = vocabularyService;
        }

        [RelayCommand]
        private void ShowMenu(FlashCard flashCard)
        {
            // Логика для показа меню с действиями для карточки
            // Возможно, вам понадобится использовать Popup или другой метод для отображения меню
        }

        [RelayCommand]
        private void Test()
        {
        }
    }
}
