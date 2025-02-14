using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Models;
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
        private void ClearAll()
        {
            // Логика для очистки всех данных
            FlashCards = null;
            // Дополнительная логика, если необходимо
        }
        [RelayCommand]
        private async void ReadFromJson()
        {
            var words = await _vocabularyService.LoadWordsAsync();
            FlashCards = words;
        }
        [RelayCommand]
        private void ShowMenu(FlashCard flashCard)
        {
            // Логика для показа меню с действиями для карточки
            // Возможно, вам понадобится использовать Popup или другой метод для отображения меню
        }
    }
}
