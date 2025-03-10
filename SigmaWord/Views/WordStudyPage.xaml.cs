using SigmaWord.Services;
using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordStudyPage : ContentPage
{
	private readonly WordStudyViewModek _viewMode;
	public WordStudyPage(WordStudyViewModek vm, WordStatus status)
	{
		InitializeComponent();
		BindingContext = vm;
		_viewMode = vm;
		if (status == WordStatus.ToLearn)
			Title = "�������� ����� ����";
		else if (status == WordStatus.Learning)
			Title = "���������� ����";
		else
			Title = " ";
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		SettingsService settings = new SettingsService();
		_viewMode.IsPronunciationEnabled = settings.GetPronunciationEnabled();
    }
}