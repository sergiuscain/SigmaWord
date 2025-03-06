using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordStudyPage : ContentPage
{
	public WordStudyPage(WordStudyViewModek vm, WordStatus status)
	{
		InitializeComponent();
		BindingContext = vm;
		if (status == WordStatus.ToLearn)
			Title = "Изучение новых слов";
		else if (status == WordStatus.Learning)
			Title = "Повторение слов";
		else
			Title = " ";
	}
}