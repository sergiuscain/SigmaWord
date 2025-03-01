using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using SigmaWord.Views;

namespace SigmaWord.ViewModels
{
    public partial class TeachViewModel : ObservableObject
    {
        private DbService _dbService;

        public TeachViewModel()
        {
            _dbService = new DbService(new SigmaWordDbContext());
        }

        public ISeries[] Series { get; set; }
        public List<string> Dates { get; set; }
        public Axis XAxes { get; set; }

        public async Task LoadStatistics()
        {
            var statistics = await _dbService.GetDailyStatisticsAsync(14);
            Dates = statistics.Select(s => s.Date.ToString("dd/MM/yyyy")).ToList();

            var repeatsSeries = new LineSeries<int>
            {
                Values = statistics.Select(s => s.TotalRepeats).ToArray(),
                Name = "Повторений:"
            };

            var wordsStudiedSeries = new StackedStepAreaSeries<int>
            {
                Values = statistics.Select(s => s.TotalWordsStudied).ToArray(),
                Name = "Полностью выученных:"
            };
            var knownSeries = new LineSeries<int>
            {
                Values = statistics.Select(s => s.TotalKnownWords).ToArray(),
                Name = "Уже известно:"
            };
            var startedSeries = new LineSeries<int>
            {
                Values = statistics.Select(s => s.TotalWordsStarted).ToArray(),
                Name = "Выучено новых слов:"
            };

            Series = new ISeries[] { repeatsSeries, wordsStudiedSeries, knownSeries, startedSeries };
            OnPropertyChanged(nameof(Series)); // Уведомляем об изменении Series
            OnPropertyChanged(nameof(Dates)); // Уведомляем об изменении Dates
        }
        [RelayCommand]
        public async Task OpenCategoryMenu()
        {
            List<Category> categories = await _dbService.GetAllCategoriesAsync();
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new SelectCategoryToStudyViewModel(_dbService, categories);
            var page = new SelectCategoryToStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
        [RelayCommand]
        public async Task ChangeDailyGoal()
        {

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
