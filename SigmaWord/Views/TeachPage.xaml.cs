using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class TeachPage : ContentPage
{
    private TeachViewModel _viewModel;
    public TeachPage(TeachViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDailyGoal(); //Загружаем Цель на день
        await _viewModel.LoadStatistics("14"); // Загружаем статистику при каждом появлении страницы
        await _viewModel.LoadNeedToRepeatData(); //Загружаем данные о количестве повторений
    }
}