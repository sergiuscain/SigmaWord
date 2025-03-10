using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class SettingsPage : ContentPage
{
	private readonly SettingsViewModel _viewModel;
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_viewModel = vm;
	}
    protected override void OnAppearing()
    {
		_viewModel.LoadDailyGoal();
		_viewModel.LoadPronunciationStatus();
        base.OnAppearing();
    }
}