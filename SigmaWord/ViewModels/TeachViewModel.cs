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
            LoadStatistics();
        }
        public ISeries[] Series { get; set; }
        public List<string> Dates { get; set; }
        public Axis XAxes { get; set; }

        private async void LoadStatistics()
        {
            _dbService = new DbService(new SigmaWordDbContext());
            var statistics = await _dbService.GetDailyStatisticsAsync(14);
            Dates = statistics.Select(s => s.Date.ToString("dd/MM/yyyy")).ToList();
            // Создание серий для TotalRepeats и TotalWordsStudied
            var repeatsSeries = new LineSeries<int>
            {
                Values = statistics.Select(s => s.TotalRepeats).ToArray(),
                Name = "Общее количество повторений"
            };

            var wordsStudiedSeries = new StackedStepAreaSeries<int>
            {
                Values = statistics.Select(s => s.TotalWordsStudied).ToArray(),
                Name = "Количество выученных слов"
            };
            //XAxes = new Axis
            //{

            //};

            // Объединение серий
            Series = new ISeries[] { repeatsSeries, wordsStudiedSeries };
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
            var viewModel = new WordStudyViewModek("Изучение новых слов:");
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
        [RelayCommand]
        public async Task OpenReviewTab()
        {
            // Создаем новую страницу и устанавливаем свойство
            var viewModel = new WordStudyViewModek("Повторение слов:");
            var page = new WordStudyPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
