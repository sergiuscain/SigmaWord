﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        [RelayCommand]
        private async Task GoToResume()
        {
            var page = new ResumePage();
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
