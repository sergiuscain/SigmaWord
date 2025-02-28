using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using System.Collections.ObjectModel;

namespace SigmaWord.ViewModels
{
    public partial class WordStudyViewModek : ObservableObject
    {
        private readonly DbService _dbService;
        public ObservableCollection<FlashCard> FlashCards { get; set; }
        public FlashCard CurrentFlashCard { get; set; }
        private int _currentIndex;
        [ObservableProperty]
        private string resultMessage;
        [ObservableProperty]
        private bool isResultVisible;
        [ObservableProperty]
        private Color resultTextColor;
        public WordStudyViewModek(DbService dbService, WordStatus status) 
        {
            _dbService = dbService;
            FlashCards = new ObservableCollection<FlashCard>();
            LoadFlashCards(status);
        }
        private async void LoadFlashCards(WordStatus status)
        {
            var flashcards = await _dbService.GetFlashCardsByStatusAsync(status);
            foreach (var card in flashcards)
            {
                FlashCards.Add(card);
            }
            _currentIndex = 0;
            ShowNextFlashCard();
        }
        public void ShowNextFlashCard()
        {
            if (_currentIndex < FlashCards.Count)
            {
                CurrentFlashCard = FlashCards[_currentIndex];
                _currentIndex++;
                OnPropertyChanged(nameof(CurrentFlashCard));
                IsResultVisible = false; // Скрыть сообщение при показе новой карточки
            }
            else
            {
                // Логика завершения, если карточки закончились
            }
        }
        [RelayCommand]
        public async void CheckAnswer(string userAnswer)
        {
            bool isCorrectAnswer = false;
            foreach(string answer in CurrentFlashCard.Translation.Split(", "))
            {
                if (answer.ToLower() == userAnswer.ToLower())
                {
                    isCorrectAnswer = true;
                    break;
                } 
                    
            }
            if (isCorrectAnswer)
            {
                // Правильный ответ
                ResultMessage = "Правильный ответ!";
                ResultTextColor = Colors.Green; // Установите цвет текста в зеленый
                IsResultVisible = true;
                await Task.Delay(2000); // Задержка в 2 секунды
                ShowNextFlashCard();
            }
            else
            {
                // Неправильный ответ
                ResultMessage = "Неправильный ответ. Попробуйте снова.";
                ResultTextColor = Colors.Red; // Установите цвет текста в красный
                IsResultVisible = true;
            }
        }
    }
}
