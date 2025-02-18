using SigmaWord.ViewModels;

namespace SigmaWord.Views;

public partial class WordsPage : ContentPage
{
    public string CategoryName { get; set; }
    public WordsPage(WordsViewModel vm, string categoryName)
	{
        InitializeComponent();
        BindingContext = vm;
        vm.CategoryName = categoryName; // Устанавливаем categoryName
        vm.LoadWords().Wait(); // Вызов метода для загрузки слов
    }
}