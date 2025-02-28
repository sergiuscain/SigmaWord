using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class SelectCategoryToStudyPage : ContentPage
{
	public SelectCategoryToStudyPage(SelectCategoryToStudyViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}