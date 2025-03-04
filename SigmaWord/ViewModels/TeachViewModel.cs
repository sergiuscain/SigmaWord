using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SigmaWord.Data;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using SigmaWord.Views;
using SkiaSharp;

namespace SigmaWord.ViewModels
{
    public partial class TeachViewModel : ObservableObject
    {
        private DbService _dbService;

        public TeachViewModel()
        {
            _dbService = new DbService(new AppDbContext());
        }

        public ISeries[] Series { get; set; }
        public List<string> Dates { get; set; }
        public Axis XAxes { get; set; }
        [ObservableProperty]
        public int dailyGoal;
        [ObservableProperty]
        public string dailyGoalText;
        [ObservableProperty]
        public int needToRepeat;
        [ObservableProperty]
        public string needToRepeatText;

        [ObservableProperty]
        public int totalRepeats;
        [ObservableProperty]
        public int totalWordsStudied;
        [ObservableProperty]
        public int totalKnownWords;
        [ObservableProperty]
        public int totalWordsStarted;

        [ObservableProperty]
        public int totalRepeatsForPeriod;
        [ObservableProperty]
        public int totalWordsStudiedForPeriod;
        [ObservableProperty]
        public int totalKnownWordsForPeriod;
        [ObservableProperty]
        public int totalWordsStartedForPeriod;

        public async Task LoadDailyGoal()
        {
            DailyGoal = (await _dbService.GetSettings()).DailyWordGoal;
            DailyGoalText = $"Цель на день: {DailyGoal}";
        }
        [RelayCommand]
        public async Task LoadStatistics(string daysString)
        {
            int days = int.Parse(daysString);
            var statistics = await _dbService.GetDailyStatisticsAsync(days);
            Dates = statistics.Select(s => s.Date.ToString("dd/MM/yyyy")).ToList();

            // Создаем массивы значений для графиков
            var totalRepeatsArr = statistics.Select(s => s.TotalRepeats).Reverse().ToArray();
            var totalWordsStudiedArr = statistics.Select(s => s.TotalWordsStudied).Reverse().ToArray();
            var totalKnownWordsArr = statistics.Select(s => s.TotalKnownWords).Reverse().ToArray();
            var totalWordsStartedArr = statistics.Select(s => s.TotalWordsStarted).Reverse().ToArray();

            TotalWordsStarted = await _dbService.GetStatisticsCountForAllTime(TypeStatisticses.TotalWordsStarted);
            TotalRepeats = await _dbService.GetStatisticsCountForAllTime(TypeStatisticses.TotalRepeats);
            TotalKnownWords = await _dbService.GetStatisticsCountForAllTime(TypeStatisticses.TotalKnownWords);
            TotalWordsStudied = await _dbService.GetStatisticsCountForAllTime(TypeStatisticses.TotalWordsStudied);

            TotalWordsStartedForPeriod = await _dbService.GetStatisticsCountForPeriod(TypeStatisticses.TotalWordsStarted, days);
            TotalRepeatsForPeriod = await _dbService.GetStatisticsCountForPeriod(TypeStatisticses.TotalRepeats, days);
            TotalKnownWordsForPeriod = await _dbService.GetStatisticsCountForPeriod(TypeStatisticses.TotalKnownWords, days);
            TotalWordsStudiedForPeriod = await _dbService.GetStatisticsCountForPeriod(TypeStatisticses.TotalWordsStudied, days);
            // Определяем цвета для каждой линии
            var seriesColors = new[]
            {
                new SKColor(255, 204, 0, 128), // Яркий желтый
                new SKColor(0, 255, 0, 128),    // Яркий зеленый
                new SKColor(0, 150, 255, 128),  // Ярко-синий
                new SKColor(0, 102, 204, 128)   // Яркий голубой
            };

            // Создаем массив серий
            Series = new ISeries[]
            {
                CreateLineSeries(totalRepeatsArr, "Повторений:", seriesColors[0]),
                CreateLineSeries(totalWordsStudiedArr, "Полностью выученных:", seriesColors[1]),
                CreateLineSeries(totalKnownWordsArr, "Уже известно:", seriesColors[2]),
                CreateLineSeries(totalWordsStartedArr, "Выучено новых слов:", seriesColors[3]),
            };

            // Уведомляем об изменении Series и Dates
            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(Dates));
        }

        // Метод для создания линии с заданным цветом
        private LineSeries<int> CreateLineSeries(int[] values, string name, SKColor color)
        {
            // Определяем начальные и конечные точки градиента
            var startPoint = new SKPoint(0, 0); // Начальная точка (левый верхний угол)
            var endPoint = new SKPoint(1, 0);   // Конечная точка (правый нижний угол)
            return new LineSeries<int>
            {
                Values = values,
                Name = name,
                Stroke = new SolidColorPaint(color),
                Fill = new SolidColorPaint(color),
                GeometrySize = 2
            };  
        }
        public async Task LoadNeedToRepeatData()
        {
            NeedToRepeat = await _dbService.GetNeedToRepeatDataAsync();
            NeedToRepeatText = $"Повторение слов {NeedToRepeat}";
        }
        [RelayCommand]
        public async Task OpenCategoryMenu()
        {
            List<Category> categories = await _dbService.GetAllCategoriesAsync();
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new SelectCategoryToStudyViewModel(_dbService);
            var page = new SelectCategoryToStudyPage(viewModel);
            await viewModel.LoadCategories();
            await Shell.Current.Navigation.PushAsync(page);
        }
        [RelayCommand]
        public async Task ChangeDailyGoal()
        {
            // Отображаем всплывающее окно для ввода
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Изменить цель",
                "Введите новое значение для ежедневной цели:",
                "OK",
                "Cancel",
                DailyGoal.ToString()); // Значение по умолчанию

            // Проверяем, было ли введено значение и является ли оно числом
            if (int.TryParse(result, out int newGoal))
            {
                var setting = await _dbService.GetSettings();
                setting.DailyWordGoal = newGoal;
                await _dbService.UpdateSettings(setting);
                await LoadDailyGoal();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                // Если введенное значение не пустое и не число, можно добавить обработку ошибки
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректное число.", "OK");
            }
        }
        [RelayCommand]
        public async Task OpenStudyTab()
        {
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new WordStudyViewModek(_dbService, WordStatus.ToLearn);
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
        [RelayCommand]
        public async Task OpenReviewTab()
        {
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new WordStudyViewModek(_dbService, WordStatus.Learning);
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }

}
