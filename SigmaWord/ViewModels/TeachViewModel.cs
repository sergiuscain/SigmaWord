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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

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

        // Определяем цвета для каждой серии
        private static readonly SKColor[] SeriesColors = new[]
        {
            new SKColor(255, 204, 0, 255), // Яркий желтый
            new SKColor(0, 255, 0, 255),   // Яркий зеленый
            new SKColor(0, 150, 255, 255), // Ярко-синий
            new SKColor(0, 102, 204, 255)  // Яркий голубой
        };

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

            // Создаем массив серий
            Series = new ISeries[]
            {
                CreateLineSeries(totalRepeatsArr, "Повторений:", SeriesColors[0]),
                CreateLineSeries(totalWordsStudiedArr, "Полностью выученных:", SeriesColors[1]),
                CreateLineSeries(totalKnownWordsArr, "Уже известно:", SeriesColors[2]),
                CreateLineSeries(totalWordsStartedArr, "Начато учиться:", SeriesColors[3]),
            };

            // Настройка осей
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Dates,
                    LabelsRotation = 45,
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(SKColors.LightGray),
                    TicksAtCenter = true
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                    TicksPaint = new SolidColorPaint(SKColors.LightGray),
                    Labeler = value => value.ToString("N0")
                }
            };

            // Уведомляем об изменении Series, XAxes и YAxes
            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(XAxes));
            OnPropertyChanged(nameof(YAxes));
        }

        // Метод для создания линии с заданным цветом
        private LineSeries<int> CreateLineSeries(int[] values, string name, SKColor color)
        {
            return new LineSeries<int>
            {
                Values = values,
                Name = name,
                Stroke = new SolidColorPaint(color) { StrokeThickness = 3 },
                Fill = new LiveChartsCore.SkiaSharpView.Painting.LinearGradientPaint(
                    new SKColor[] { color.WithAlpha(100), color.WithAlpha(50) },
                    new SKPoint(0.5f, 0),
                    new SKPoint(0.5f, 1)
                ),
                GeometrySize = 2,
                GeometryStroke = new SolidColorPaint(color) { StrokeThickness = 3 },
                GeometryFill = new SolidColorPaint(color)
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
            var viewModel = new SelectCategoryToStudyViewModel(_dbService);
            var page = new SelectCategoryToStudyPage(viewModel);
            await viewModel.LoadCategories();
            await Shell.Current.Navigation.PushAsync(page);
        }

        [RelayCommand]
        public async Task ChangeDailyGoal()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Изменить цель",
                "Введите новое значение для ежедневной цели:",
                "OK",
                "Cancel",
                DailyGoal.ToString());

            if (int.TryParse(result, out int newGoal))
            {
                var setting = await _dbService.GetSettings();
                setting.DailyWordGoal = newGoal;
                await _dbService.UpdateSettings(setting);
                await LoadDailyGoal();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректное число.", "OK");
            }
        }

        [RelayCommand]
        public async Task OpenStudyTab()
        {
            var viewModel = new WordStudyViewModek(_dbService, WordStatus.ToLearn);
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }

        [RelayCommand]
        public async Task OpenReviewTab()
        {
            var viewModel = new WordStudyViewModek(_dbService, WordStatus.Learning);
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
