using Newtonsoft.Json;
using SigmaWord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            LoadOrCreateSettings();
        }
        /// <summary>
        /// Загружает в _appSettings данные из файла с настройками или создает новый файл
        /// </summary>
        private void LoadOrCreateSettings()
        {
            if (File.Exists(_settingsFilePath))
            {
                var json = File.ReadAllText(_settingsFilePath);
                _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            }
            else
            {
                _appSettings = new AppSettings
                {
                    DailyWordGoal = 10,
                    IsPronunciationEnabled = false,
                    SelectedTheme = Themes.DarkPurple.ToString(),
                };
                SaveSettings();
            }
        }
        /// <summary>
        /// Записываает настройки из _appSettings в файл
        /// </summary>
        private void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(_appSettings);
            File.WriteAllText(_settingsFilePath, json);
        }
        /// <summary>
        /// Возвращает Ежедневную цель по изучению слов из настроек
        /// </summary>
        /// <returns></returns>
        public int GetDailyWordGoal()
        {
            return _appSettings.DailyWordGoal;
        }
        /// <summary>
        /// Устонавливает новое значение ежедневной цели по изучению слов
        /// </summary>
        /// <param name="goal"></param>
        public void SetDailyWordGoal(int goal)
        {
            if (goal > 1)
            {
                _appSettings.DailyWordGoal = goal;
                SaveSettings();
            }
        }
        /// <summary>
        /// Возврощает bool знаение, включена ли озвучка слов
        /// </summary>
        /// <returns></returns>
        public bool GetPronunciationEnabled()
        {
            return _appSettings.IsPronunciationEnabled;
        }
        /// <summary>
        /// Устонавливает настройку отвечающую за включение озвучки
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetPronunciationEnabled(bool isEnabled)
        {
            _appSettings.IsPronunciationEnabled = isEnabled;
            SaveSettings();
        }
    }
}
