using SigmaWord.Views;

namespace SigmaWord
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Регистрация маршрута для WordsPage
            Routing.RegisterRoute(nameof(WordsPage), typeof(WordsPage));
        }

    }
}
