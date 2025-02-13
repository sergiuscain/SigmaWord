using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class DictionaryPage : ContentPage
{
	public DictionaryPage(DictionaryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}