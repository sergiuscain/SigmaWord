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

        [ObservableProperty]
        public ObservableCollection<FlashCard> flashCards;
        [ObservableProperty]
        public FlashCard currentFlashCard;
        private int _currentIndex;

        [ObservableProperty]
        private bool isWordVisible;

        [ObservableProperty]
        private bool isTranslationVisible;

        [ObservableProperty]
        private bool isExamplesVisible;

        [ObservableProperty]
        private string exampleTranslations;

        [ObservableProperty]
        private string resultMessage;

        [ObservableProperty]
        private bool isResultVisible;

        [ObservableProperty]
        private Color resultTextColor;

        [ObservableProperty]
        private bool isButtonsVisible; // Для управления видимостью кнопок "Вспомнил" и "Не вспомнил"
        [ObservableProperty]
        private bool isShowVisibleTranslateButtonVisible;

        public WordStudyViewModek(DbService dbService, WordStatus status)
        {
            _dbService = dbService;
            FlashCards = new ObservableCollection<FlashCard>();
            LoadFlashCards(status).Wait();
        }

        private async Task LoadFlashCards(WordStatus status)
        {
            List<FlashCard> flashCards = new List<FlashCard>();
            if (status == WordStatus.ToLearn)
            {
                int wordStartedToLearn = (await _dbService.GetTodayStatisticsAsync()).TotalWordsStarted;
                int dailyGoal = (await _dbService.GetSettings()).DailyWordGoal;
                int needToStartLearn = dailyGoal - wordStartedToLearn;
                flashCards = await _dbService.GetFlashCardsByStatusAsync(status, needToStartLearn);
            }
            else
            {
                flashCards = await _dbService.GetFlashCardsByStatusAsync(WordStatus.Learning);
            }
            foreach (var card in flashCards)
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
                IsWordVisible = true;
                IsTranslationVisible = false; // Скрываем перевод
                IsExamplesVisible = true; // Показываем примеры
                IsButtonsVisible = false; // Скрываем кнопки "Вспомнил" и "Не вспомнил"
                IsShowVisibleTranslateButtonVisible = true;
                CurrentFlashCard = FlashCards[_currentIndex];
                _currentIndex++;

                OnPropertyChanged(nameof(CurrentFlashCard));
            }
            else
            {
                // Логика завершения
            }
        }

        [RelayCommand]
        public void ShowTranslation()
        {
            IsTranslationVisible = true; // Показываем перевод
            IsButtonsVisible = true; // Показываем кнопки "Вспомнил" и "Не вспомнил"
            IsShowVisibleTranslateButtonVisible = false; //Скрываем кнопку для отображения перевода
        }

        [RelayCommand]
        public async void Remembered()
        {
            // Логика, как в методе CheckAnswer при правильном ответе
            await CheckAnswerLogic(true);
        }

        [RelayCommand]
        public async void NotRemembered()
        {
            // Логика, как в методе CheckAnswer при неправильном ответе
            await CheckAnswerLogic(false);
            await _dbService.UpdateFlashCard(CurrentFlashCard);
        }

        private async Task CheckAnswerLogic(bool isCorrect)
        {
            // Логика обработки ответа
            if (isCorrect)
            {
                // Правильный ответ
                ResultMessage = "Правильный ответ!";
                ResultTextColor = Colors.Green;
                UpdateFlashCard(true);
            }
            else
            {
                // Неправильный ответ
                ResultMessage = "Неправильный ответ.";
                ResultTextColor = Colors.Red;
                UpdateFlashCard(false);
            }
            IsResultVisible = true;
            await Task.Delay(500); // Задержка в 2 секунды
            ResultMessage = "";
            await _dbService.UpdateFlashCard(CurrentFlashCard);
            ShowNextFlashCard();
        }

        public async void UpdateFlashCard(bool isCorrect)
        {
            DateTime now = DateTime.Now;

            // Корректировка процента выученности при неправильном ответе
            if (!isCorrect)
            {
                CurrentFlashCard.CurrentRepetitions = Math.Max(0, CurrentFlashCard.CurrentRepetitions - 10); // Уменьшаем на 10%, не ниже 0
            }
            else if (CurrentFlashCard.CurrentRepetitions == 0)
            {
                // 0% - добавляем 30 минут
                CurrentFlashCard.NextRepeatDate = now.AddMinutes(30);
                CurrentFlashCard.CurrentRepetitions += 10;
                if (CurrentFlashCard.Status == WordStatus.ToLearn)
                {
                    await _dbService.AddStatistics(TypeStatisticses.TotalWordsStarted.ToString());
                    CurrentFlashCard.Status = WordStatus.Learning;
                }
                else
                {
                    await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
                }
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 20)
            {
                // 1-20% - добавляем 1 день
                CurrentFlashCard.NextRepeatDate = now.AddDays(1);
                CurrentFlashCard.CurrentRepetitions += 10;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 40)
            {
                // 21-40% - добавляем 3 дня
                CurrentFlashCard.NextRepeatDate = now.AddDays(3);
                CurrentFlashCard.CurrentRepetitions += 10;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 60)
            {
                // 41-60% - добавляем 1 неделю
                CurrentFlashCard.NextRepeatDate = now.AddDays(7);
                CurrentFlashCard.CurrentRepetitions += 10;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 80)
            {
                // 61-80% - добавляем 2 недели
                CurrentFlashCard.NextRepeatDate = now.AddDays(14);
                CurrentFlashCard.CurrentRepetitions += 10;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if(CurrentFlashCard.CurrentRepetitions < 99)
            {
                // 81-99% - добавляем 1 месяц
                CurrentFlashCard.NextRepeatDate = now.AddMonths(1);
                CurrentFlashCard.CurrentRepetitions += 10;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else
            {
                CurrentFlashCard.Status = WordStatus.Mastered;
                await _dbService.AddStatistics(TypeStatisticses.TotalWordsStudied.ToString());
            }
        }

        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(".."); // Возврат на предыдущую страницу
        }
    }
}
