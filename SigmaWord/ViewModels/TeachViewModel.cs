using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.ViewModels
{
    public partial class TeachViewModel : ObservableObject
    {
        public List<ISeries> Series { get; set; }
        public TeachViewModel()
        {
            Series = new List<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new double[] { 3, 5, 7, 4 }
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
