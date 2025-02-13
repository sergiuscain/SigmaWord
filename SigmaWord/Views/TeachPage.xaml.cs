using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class TeachPage : ContentPage
{
	public TeachPage(TeachViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}