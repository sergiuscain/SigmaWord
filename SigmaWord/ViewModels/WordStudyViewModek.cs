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
        private bool isWordtVisible;
        [ObservableProperty]
        private bool iAnswerEntrytVisible;
        [ObservableProperty]
        private bool isButtonCheckAnswerVisible;
        [ObservableProperty]
        private bool isButtonToBackVisible;
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
                IsButtonToBackVisible = false;
                IsButtonCheckAnswerVisible = true;
                IsWordtVisible = true;
                IAnswerEntrytVisible = true;
                CurrentFlashCard = FlashCards[_currentIndex];
                _currentIndex++;
                OnPropertyChanged(nameof(CurrentFlashCard));
                IsResultVisible = false; // Скрыть сообщение при показе новой карточки
            }
            else
            {
                IsButtonCheckAnswerVisible = false;
                IsWordtVisible = false;
                IAnswerEntrytVisible = false;
                ResultMessage = "Вы выучили все слова";
                ResultTextColor = Colors.Green;
                IsButtonToBackVisible = true;

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

                SetNextRepeatDate(CurrentFlashCard, CurrentFlashCard.GetLearningPercentage(),true);
                CurrentFlashCard.LastRepeatDate = DateTime.Now;
                CurrentFlashCard.CurrentRepetitions += 10;
                CurrentFlashCard.Status = WordStatus.Learning;
                // Правильный ответ
                ResultMessage = "Правильный ответ!";
                ResultTextColor = Colors.Green; // Установите цвет текста в зеленый
                IsResultVisible = true;
                await _dbService.AddRepeatToStatistics();
                await _dbService.UpdateFlashCard(CurrentFlashCard);
                await Task.Delay(2000); // Задержка в 2 секунды
                ShowNextFlashCard();
            }
            else
            {
                // Неправильный ответ
                ResultMessage = "Неправильный ответ. Попробуйте снова.";
                SetNextRepeatDate(CurrentFlashCard, CurrentFlashCard.GetLearningPercentage(), true);
                await _dbService.UpdateFlashCard(CurrentFlashCard);
                ResultTextColor = Colors.Red; // Установите цвет текста в красный
                IsResultVisible = true;
                await Task.Delay(2000); // Задержка в 2 секунды
                ShowNextFlashCard();
            }
        }
        public void SetNextRepeatDate(FlashCard card, double learnedPercentage, bool answeredCorrectly)
        {
            DateTime now = DateTime.Now;

            // Корректировка процента выученности при неправильном ответе
            if (!answeredCorrectly)
            {
                learnedPercentage = Math.Max(0, learnedPercentage - 20); // Уменьшаем на 20%, не ниже 0
            }

            if (learnedPercentage == 100)
            {
                // Изменяем статус на Mastered
                card.Status = WordStatus.Mastered;
            }
            else if (learnedPercentage == 0)
            {
                // 0% - добавляем 30 минут
                card.NextRepeatDate = now.AddMinutes(30);
            }
            else if (learnedPercentage <= 20)
            {
                // 1-20% - добавляем 1 день
                card.NextRepeatDate = now.AddDays(1);
            }
            else if (learnedPercentage <= 40)
            {
                // 21-40% - добавляем 3 дня
                card.NextRepeatDate = now.AddDays(3);
            }
            else if (learnedPercentage <= 60)
            {
                // 41-60% - добавляем 1 неделю
                card.NextRepeatDate = now.AddDays(7);
            }
            else if (learnedPercentage <= 80)
            {
                // 61-80% - добавляем 2 недели
                card.NextRepeatDate = now.AddDays(14);
            }
            else
            {
                // 81-99% - добавляем 1 месяц
                card.NextRepeatDate = now.AddMonths(1);
            }
        }
        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(".."); // Возврат на предыдущую страницу
        }
    }
}
