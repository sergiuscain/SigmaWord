namespace SigmaWord.Views;

public partial class ResumePage : ContentPage
{
	public ResumePage()
	{
		InitializeComponent();
	}
    // Обработчик для GitHub
    private async void OnGitHubClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://github.com/sergiuscain"));
    }

    // Обработчик для HH
    private async void OnHhClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://medyn.hh.ru/resume/a8c3bdadff0d73e5560039ed1f4a4d66723547"));
    }

    // Обработчик для Telegram
    private async void OnTelegramClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://t.me/iamrayff")); 
    }
}