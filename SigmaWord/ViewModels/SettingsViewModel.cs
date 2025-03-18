using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Resources.Styles;
using SigmaWord.Services;
using SigmaWord.Views;

namespace SigmaWord.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
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
            _settingsService = new SettingsService();
            Themes = new List<string> { "Светлая", "Темно_фиолетовая", "Темная", "Лесная", "Pink_Dream", "Океан", "Космос", "Закат", "Зима"};
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
            var settingsService = new SettingsService();
            settingsService.SetTheme(selectedTheme); // Сохраняем тему в настройках
            ((App)Application.Current).ApplyTheme(selectedTheme); // Применяем тему ко всему приложению
        }
    }
}
