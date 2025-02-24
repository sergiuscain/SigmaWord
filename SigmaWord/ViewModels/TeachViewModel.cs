using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;

namespace SigmaWord.ViewModels
{
    public partial class TeachViewModel : ObservableObject
    {
        public List<string> DaysOfWeek { get; set; }
        public List<Axis> XAxes { get; set; }

        public LineSeries<double> StudiedWordsSeries { get; set; }

        public LineSeries<double> ReviewedWordsSeries { get; set; }

        public List<ISeries> Series { get; set; }
        public TeachViewModel()
        {
            // Пример данных для графиков
            DaysOfWeek = new List<string> { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };

            StudiedWordsSeries = new LineSeries<double>
            {
                Values = new double[] { 5, 5, 7, 5, 10, 5, 7 },
                Name = "Изученные слова",
                Stroke = new SolidColorPaint(SKColors.Red), // Задаем красный цвет для линии
            };

            ReviewedWordsSeries = new LineSeries<double>
            {
                Values = new double[] { 10, 15, 15, 10, 30, 15, 20 },
                Name = "Повторенные слова",
                Stroke = new SolidColorPaint(SKColors.Blue) // Задаем Синий цвет для линии
            };

            Series = new List<ISeries>
            {
                StudiedWordsSeries,
                ReviewedWordsSeries
            };
            XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = DaysOfWeek,
                }
            };
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
