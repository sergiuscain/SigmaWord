﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.ViewModels
{
    public partial class TeachViewModel : ObservableObject
    {
        [ObservableProperty]
        string text = "Текст для учебы";
    }
}
