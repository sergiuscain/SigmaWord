using Newtonsoft.Json;
using SigmaWord.Models;
using SigmaWord.Resources.Styles;
using SigmaWord.Resources.Themes;

namespace SigmaWord.Services
{
    class SettingsService
    {
        /// <summary>
        /// Путь к файлу с настройками
        /// </summary>
        private readonly string _settingsFilePath;
        
        private AppSettings _appSettings;
        public SettingsService()
        {
            _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json");
            LoadOrCreateSettingsAsync().Wait();
        }
        /// <summary>
        /// Загружает в _appSettings данные из файла с настройками или создает новый файл
        /// </summary>
        private async Task LoadOrCreateSettingsAsync()
        {
            if (File.Exists(_settingsFilePath))
            {
                LoadSettings();
            }
            else
            {
                _appSettings = new AppSettings
                {
                    DailyWordGoal = 10,
                    IsPronunciationEnabled = false,
                    SelectedTheme = ThemesEnum.Темная.ToString(),
                };
                await SaveSettingsAsync();
            }
        }

        private void LoadSettings()
        {
            var json = File.ReadAllText(_settingsFilePath);
            _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
        }

        /// <summary>
        /// Записываает настройки из _appSettings в файл
        /// </summary>
        private async Task SaveSettingsAsync()
        {
            var json = JsonConvert.SerializeObject(_appSettings);
            await File.WriteAllTextAsync(_settingsFilePath, json);
        }
        /// <summary>
        /// Возвращает Ежедневную цель по изучению слов из настроек
        /// </summary>
        /// <returns></returns>
        public int GetDailyWordGoal()
        {
            LoadSettings();
            return _appSettings.DailyWordGoal;
        }
        /// <summary>
        /// Устонавливает новое значение ежедневной цели по изучению слов
        /// </summary>
        /// <param name="goal"></param>
        public async Task SetDailyWordGoalAsync(int goal)
        {
            if (goal > 1)
            {
                LoadSettings();
                _appSettings.DailyWordGoal = goal;
                await SaveSettingsAsync();
            }
        }
        /// <summary>
        /// Возврощает bool знаение, включена ли озвучка слов
        /// </summary>
        /// <returns></returns>
        public bool GetPronunciationEnabled()
        {
            LoadSettings();
            return _appSettings.IsPronunciationEnabled;
        }
        /// <summary>
        /// Устонавливает настройку отвечающую за включение озвучки
        /// </summary>
        /// <param name="isEnabled"></param>
        public async Task SetPronunciationEnabledAsync(bool isEnabled)
        {
            LoadSettings();
            _appSettings.IsPronunciationEnabled = isEnabled;
            
        }
        public async Task SetThemeAsync(string theme)
        {
            _appSettings.SelectedTheme = theme;
            await SaveSettingsAsync();
        }
        public void LoadTheme(string selectedTheme)
        {
            
            // Удаляем все текущие темы
            Application.Current.Resources.MergedDictionaries.Clear();

            if (selectedTheme == ThemesEnum.Светлая.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
            else if (selectedTheme == ThemesEnum.Темно_фиолетовая.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new DarkPurpleTheme());
            }
            else if (selectedTheme == ThemesEnum.Темная.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else if (selectedTheme == ThemesEnum.Закат.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new SunsetTheme());
            }
            else if (selectedTheme == ThemesEnum.Зима.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new WinterTheme());
            }
            else if (selectedTheme == ThemesEnum.Космос.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
            }
            else if (selectedTheme == ThemesEnum.Лесная.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new ForestTheme());
            }
            else if (selectedTheme == ThemesEnum.Океан.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new SeaTheme());
            }
            else if (selectedTheme == ThemesEnum.Pink_Dream.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new PinkDreamTheme());
            }
        }
        public void SetTheme(string theme)
        {
            _appSettings.SelectedTheme = theme;
            var json = JsonConvert.SerializeObject(_appSettings);
            File.WriteAllText(_settingsFilePath, json);
        }
        public string GetTheme()
        {
            LoadSettings();
            return _appSettings.SelectedTheme;
        }
        public async Task LoadTheme()
        {
            LoadTheme(_appSettings.SelectedTheme);
        }
    }
}
