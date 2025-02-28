	using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class TeachPage : ContentPage
{
    private TeachViewModel _viewModel;
    public TeachPage(TeachViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadStatistics(); // ��������� ���������� ��� ������ ��������� ��������
    }
}