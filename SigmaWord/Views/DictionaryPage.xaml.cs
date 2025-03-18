using SigmaWord.Data.Entities;
using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class DictionaryPage : ContentPage
{
	private readonly DictionaryViewModel _viewModel;
	public DictionaryPage(DictionaryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_viewModel = vm;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}