using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Themes;
using SigmaWord.Resources.Styles;
using SigmaWord.Services;
using SigmaWord.Views;

namespace SigmaWord.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        string text = "В будущем здесь что-то будет))";
        private readonly SpeechService _speechService;
        private readonly SettingsService _settingsService;
        [ObservableProperty]
        private int dailyGoal;
        [ObservableProperty]
        private string dailyGoalText;
        [ObservableProperty]
        private bool isPronunciationEnabled;
        [ObservableProperty]
        private string isPronunciationEnabledText;
        [ObservableProperty]
        private string selectedTheme;
        public List<string> Themes { get; }
        public SettingsViewModel(SpeechService speechService)
        {
            _speechService = speechService;
            _settingsService = new SettingsService();
            Themes = new List<string> { "Темная", "Светлая", "Темно_фиолетовая"};
            // Изначально скрываем Picker
        }
        public void LoadDailyGoal()
        {
            DailyGoal = _settingsService.GetDailyWordGoal();
            DailyGoalText = $"Цель на день: {DailyGoal}";
        }
        public void LoadPronunciationStatus()
        {
            IsPronunciationEnabled = _settingsService.GetPronunciationEnabled();
            if (IsPronunciationEnabled == true)
                IsPronunciationEnabledText = "Озвучка слов: Вкл";
            else 
                IsPronunciationEnabledText = "Озвучка слов: Выкл";
        }
        [RelayCommand]
        public async Task ChangePronunciationStatus()
        {
            IsPronunciationEnabled = !IsPronunciationEnabled;
            await _settingsService.SetPronunciationEnabledAsync(IsPronunciationEnabled);
            LoadPronunciationStatus();
        }
        [RelayCommand]
        public async Task ChangeDailyGoal()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Изменить цель",
                "Введите новое значение для ежедневной цели:",
                "OK",
                "Cancel",
                DailyGoal.ToString());

            if (int.TryParse(result, out int newGoal))
            {
                await _settingsService.SetDailyWordGoalAsync(newGoal);
                LoadDailyGoal();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректное число.", "OK");
            }
        }
        [RelayCommand]
        private async Task GoToResume()
        {
            var page = new ResumePage();
            await Shell.Current.Navigation.PushAsync(page);
        }
        [RelayCommand]
        private async Task GoToTG()
        {
            await Launcher.OpenAsync(new Uri("https://t.me/+zf_utiYiZsY1MjBi"));
        }
        [RelayCommand]
         partial void OnSelectedThemeChanged(string selectedTheme)
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
            Preferences.Set("SelectedTheme", selectedTheme.ToString());
        }
    }
}
