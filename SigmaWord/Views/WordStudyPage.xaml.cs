using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordStudyPage : ContentPage
{
	public WordStudyPage(WordStudyViewModek vm, WordStatus status)
	{
		InitializeComponent();
		BindingContext = vm;
		if (status == WordStatus.ToLearn)
			Title = "�������� ����� ����";
		else if (status == WordStatus.Learning)
			Title = "���������� ����";
		else
			Title = " ";
	}
}