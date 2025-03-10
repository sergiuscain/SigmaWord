using Newtonsoft.Json;
using SigmaWord.Models;

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
                    SelectedTheme = Themes.DarkPurple.ToString(),
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
            await SaveSettingsAsync();
        }
    }
}
