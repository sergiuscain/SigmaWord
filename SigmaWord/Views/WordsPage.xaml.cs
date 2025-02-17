using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordsPage : ContentPage
{
	public WordsPage(WordsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}