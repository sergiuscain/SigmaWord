using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using System.Collections.ObjectModel;

namespace SigmaWord.ViewModels
{
    public partial class WordStudyViewModek : ObservableObject
    {
        private readonly DbService _dbService;
        private readonly SpeechService _speechService;

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
        private string resultMessage;

        [ObservableProperty]
        private bool isResultVisible;

        [ObservableProperty]
        private Color resultTextColor;

        [ObservableProperty]
        private bool isButtonsVisible;

        [ObservableProperty]
        private bool isShowVisibleTranslateButtonVisible;

        [ObservableProperty]
        private bool isToLearnButtonsVisible;
        private readonly SettingsService _settingsService;
        public bool IsPronunciationEnabled;

        public WordStudyViewModek(DbService dbService, WordStatus status)
        {
            _dbService = dbService;
            _speechService = new SpeechService();
            _settingsService = new SettingsService();
            FlashCards = new ObservableCollection<FlashCard>();
            IsToLearnButtonsVisible = status == WordStatus.ToLearn;
            IsButtonsVisible = status == WordStatus.Learning;
            LoadFlashCards(status).Wait();
        }

        private async Task LoadFlashCards(WordStatus status)
        {
            List<FlashCard> flashCards = new List<FlashCard>();
            if (status == WordStatus.ToLearn)
            {
                int wordStartedToLearn = (await _dbService.GetTodayStatisticsAsync()).TotalWordsStarted;
                int dailyGoal = _settingsService.GetDailyWordGoal();
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

        public async void ShowNextFlashCard()
        {
            if (_currentIndex < FlashCards.Count)
            {
                // Устанавливаем текущую карточку
                CurrentFlashCard = FlashCards[_currentIndex];

                // Показываем элементы интерфейса
                IsWordVisible = true;
                IsTranslationVisible = false;
                IsExamplesVisible = true;
                IsButtonsVisible = FlashCards[_currentIndex].Status == WordStatus.Learning;
                IsToLearnButtonsVisible = FlashCards[_currentIndex].Status == WordStatus.ToLearn;
                IsShowVisibleTranslateButtonVisible = true;

                // Используем Dispatcher для выполнения озвучки после обновления UI
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    if (IsPronunciationEnabled)
                        await _speechService.Speak(CurrentFlashCard.Word);
                });

                _currentIndex++;
            }
            else
            {
                // Скрываем все кнопки и показываем сообщение
                IsButtonsVisible = false;
                IsToLearnButtonsVisible = false;
                IsShowVisibleTranslateButtonVisible = false;
                IsExamplesVisible = false;
                IsWordVisible = false;
                IsTranslationVisible = false;
                IsResultVisible = true;
                ResultTextColor = Colors.Yellow;
                ResultMessage = "Вы выучили все доступные слова!\nЧтобы учить больше, увеличьте цель на день\nили добавьте новые категории для изучения.";
            }
        }

        [RelayCommand]
        public void ShowTranslation()
        {
            IsTranslationVisible = true;
            IsButtonsVisible = CurrentFlashCard.Status == WordStatus.Learning;
            IsToLearnButtonsVisible = CurrentFlashCard.Status == WordStatus.ToLearn;
            IsShowVisibleTranslateButtonVisible = false;
        }
        [RelayCommand]
        private async Task AleadyKnow()
        {
            await _dbService.AddStatistics(TypeStatisticses.TotalKnownWords.ToString());
            CurrentFlashCard.Status = WordStatus.Known;
            CurrentFlashCard.CurrentRepetitions = 100;
            await _dbService.UpdateFlashCard(CurrentFlashCard);
            ShowNextFlashCard();
        }
        [RelayCommand]
        private async Task StartLearning()  
        {
            await _dbService.AddStatistics(TypeStatisticses.TotalWordsStarted.ToString());
            CurrentFlashCard.Status = WordStatus.Learning;
            CurrentFlashCard.CurrentRepetitions = 10; // Начинаем с 10%
            await _dbService.UpdateFlashCard(CurrentFlashCard);
            ShowNextFlashCard();
        }

        [RelayCommand]
        public async void Remembered()
        {
            await CheckAnswerLogic(true);
        }

        [RelayCommand]
        public async void NotRemembered()
        {
            await CheckAnswerLogic(false);
        }

        private async Task CheckAnswerLogic(bool isCorrect)
        {
            if (isCorrect)
            {
                ResultMessage = "Правильный ответ!";
                ResultTextColor = Colors.Green;
                UpdateFlashCard(true);
            }
            else
            {
                ResultMessage = "Неправильный ответ.";
                ResultTextColor = Colors.Red;
                UpdateFlashCard(false);
            }
            IsResultVisible = true;
            await Task.Delay(500);
            IsResultVisible = false;
            await _dbService.UpdateFlashCard(CurrentFlashCard);
            ShowNextFlashCard();
        }

        public async void UpdateFlashCard(bool isCorrect)
        {
            DateTime now = DateTime.Now;
            int countToAdd = 10;
            // Корректировка процента выученности при неправильном ответе
            if (!isCorrect)
            {
                CurrentFlashCard.CurrentRepetitions = Math.Max(0, CurrentFlashCard.CurrentRepetitions -= countToAdd); // Уменьшаем на 10%, не ниже 0
            }
            else if (CurrentFlashCard.CurrentRepetitions == 0)
            {
                // 0% - добавляем 30 минут
                CurrentFlashCard.NextRepeatDate = now.AddMinutes(30);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
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
            else if (CurrentFlashCard.CurrentRepetitions <= 10)
            {
                // 1-10% - добавляем 1 день
                CurrentFlashCard.NextRepeatDate = now.AddDays(1);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 20)
            {
                // 11-20% - добавляем 3 дня
                CurrentFlashCard.NextRepeatDate = now.AddDays(3);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 30)
            {
                // 21-30% - добавляем 1 неделю
                CurrentFlashCard.NextRepeatDate = now.AddDays(7);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 40)
            {
                // 31-40% - добавляем 2 недели
                CurrentFlashCard.NextRepeatDate = now.AddDays(14);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 50)
            {
                // 41-50% - добавляем 1 месяц
                CurrentFlashCard.NextRepeatDate = now.AddMonths(1);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 60)
            {
                // 41-60% - добавляем 1 месяц и 15 дней
                CurrentFlashCard.NextRepeatDate = now.AddMonths(1).AddDays(15);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 70)
            {
                // 41-60% - добавляем 1 месяц
                CurrentFlashCard.NextRepeatDate = now.AddMonths(2);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 80)
            {
                // 41-60% - добавляем 2 месяца и 7 дней
                CurrentFlashCard.NextRepeatDate = now.AddMonths(2).AddDays(7);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else if (CurrentFlashCard.CurrentRepetitions <= 90)
            {
                // 61-80% - добавляем 2 месяца и 14 дней
                CurrentFlashCard.NextRepeatDate = now.AddMonths(2).AddDays(14);
                CurrentFlashCard.CurrentRepetitions += countToAdd;
                await _dbService.AddStatistics(TypeStatisticses.TotalRepeats.ToString());
            }
            else
            {
                CurrentFlashCard.CurrentRepetitions += countToAdd;
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
