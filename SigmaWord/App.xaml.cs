﻿using SigmaWord.Resources.Styles;
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
            MainPage = new AppShell();
            _dbService = dbService;
        }
        protected override async void OnStart()
        {
            await _dbService.InitializeDatabaseAsync();
            await _dbService.InitializeStatisticsAsync(190);

            // Загружаем тему при запуске приложения
            var settingsService = new SettingsService();
            var selectedTheme = settingsService.GetTheme(); // Добавьте метод GetSelectedTheme в SettingsService
            ApplyTheme(selectedTheme); // Применяем тему ко всему приложению
        }
        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var settingsService = new SettingsService();
            var selectedTheme = settingsService.GetTheme();
            ApplyTheme(selectedTheme); // Применяем сохранённую тему, а не системную
        }
        public void ApplyTheme(string selectedTheme)
        {
            // Очищаем все текущие темы
            Application.Current.Resources.MergedDictionaries.Clear();

            // Применяем выбранную тему
            switch (selectedTheme)
            {
                case "Светлая":
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
                case "Темно_фиолетовая":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkPurpleTheme());
                    break;
                case "Темная":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                    break;
                default:
                    // Если тема не выбрана или не распознана, применяем тему по умолчанию
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
            }
        }
    }
}
