﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigmaWord.Services;
using SigmaWord.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        string text = "В будущем здесь что-то будет))";
        private readonly SpeechService _speechService;
        public SettingsViewModel(SpeechService speechService)
        {
            _speechService = speechService;
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
        private async Task Speak()
        {
            await _speechService.Speak("Привет, я умею говорить на 250ти тысячах языков. I'ts true, i'm brilliant");
        }
    }
}
