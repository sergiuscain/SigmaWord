using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using SkiaSharp;
using System.Collections.Generic;

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
        public Axis[] XAxes { get; set; }

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

            var wordsStudiedSeries = new LineSeries<int>
            {
                Values = statistics.Select(s => s.TotalWordsStudied).ToArray(),
                Name = "Количество выученных слов"
            };

            // Объединение серий
            Series = new ISeries[] { repeatsSeries, wordsStudiedSeries };
        }

        [RelayCommand]
        public async Task OpenCategoryMenu()
        {
            
        }
        [RelayCommand]
        public async Task ChangeDailyGoal()
        {

        }
        [RelayCommand]
        public async Task OpenStudyTab()
        {

        }
        [RelayCommand]
        public async Task OpenReviewTab()
        {

        }
    }
}
