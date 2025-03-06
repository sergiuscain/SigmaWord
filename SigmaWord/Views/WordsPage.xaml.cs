using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordsPage : ContentPage
{
    private readonly WordsViewModel _viewModel;
    public WordsPage(WordsViewModel vm, string categoryName)
	{
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
        vm.CategoryName = categoryName;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadWords(); // Вызов метода для загрузки слов
    }
}