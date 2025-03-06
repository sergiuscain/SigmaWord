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
        await _viewModel.LoadDailyGoal(); //��������� ���� �� ����
        await _viewModel.LoadStatistics("14"); // ��������� ���������� ��� ������ ��������� ��������
        await _viewModel.LoadNeedToRepeatData(); //��������� ������ � ���������� ����������
    }
}