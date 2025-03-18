using SigmaWord.Resources.Styles;
using SigmaWord.Services;

namespace SigmaWord
{
    public partial class App : Application
    {
        private readonly DbService _dbService;
        public App(DbService dbService)
        {
            InitializeComponent();
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
            ApplyTheme(Application.Current.RequestedTheme);
            MainPage = new AppShell();
            _dbService = dbService;
        }
        protected override async void OnStart()
        {
            await _dbService.InitializeDatabaseAsync();
            await _dbService.InitializeStatisticsAsync(190);
        }
        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            ApplyTheme(e.RequestedTheme);
        }
        private void ApplyTheme(AppTheme theme)
        {
            if (theme == AppTheme.Dark)
            {
                var lightTheme = Application.Current.Resources.MergedDictionaries.FirstOrDefault(md => md is LightTheme);
                if (lightTheme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(lightTheme);
                }
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                var darkTheme = Application.Current.Resources.MergedDictionaries.FirstOrDefault(md => md is DarkTheme);
                if (darkTheme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(darkTheme);
                }
                Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
        }
    }
}
