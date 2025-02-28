using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.ViewModels
{
    
    public partial class SelectCategoryToStudyViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        [ObservableProperty]
        public ObservableCollection<Category> categories;
        public SelectCategoryToStudyViewModel(DbService dbService, List<Category> categories)
        {
            _dbService = dbService;
            this.categories = new ObservableCollection<Category>(categories);
        }
        [RelayCommand]
        public async Task StartLearning(Category category)
        {
            await _dbService.AddCategoryToLearning(category);
        }
        [RelayCommand]
        public async Task StopLearning(Category category)
        {
            await _dbService.DeleteCategoryFromLearning(category);
        }
    }
}
