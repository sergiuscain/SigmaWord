using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordStudyPage : ContentPage
{
	public WordStudyPage(WordStudyViewModek vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}