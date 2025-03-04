using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Data.Entities;
using SigmaWord.Services;

namespace SigmaWord.ViewModels
{
    
    public partial class SelectCategoryToStudyViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        [ObservableProperty]
        public List<Category> categories;
        public SelectCategoryToStudyViewModel(DbService dbService)
        {
            _dbService = dbService;
        }
        [RelayCommand]
        public async Task StartLearning(int categoryId)
        {
            await _dbService.AddCategoryToLearning(categoryId);
        }
        [RelayCommand]
        public async Task StopLearning(int categoryId)
        {
            await _dbService.DeleteCategoryFromLearning(categoryId);
        }
        public async Task LoadCategories()
        {
            Categories = await _dbService.GetAllCategoriesAsync();
        }
    }
}
