﻿using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SigmaWord.Data;
using SigmaWord.Services;
using SigmaWord.ViewModels;
using SigmaWord.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace SigmaWord
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp()
                .UseLiveCharts()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Сервис по обработки слов.
            builder.Services.AddSingleton<VocabularyService>();

            builder.Services.AddSingleton<SettingsService>();

            //Сервис синтеза речи
            builder.Services.AddTransient<SpeechService>();

            //Добавляем сервис для работы с базой данных SQLite
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddTransient<DbService>();

            //Сервисы для каждой вьюшки и её модели представления.
            builder.Services.AddTransient<DictionaryViewModel>();
            builder.Services.AddTransient<DictionaryPage>();
            builder.Services.AddTransient<TeachViewModel>();
            builder.Services.AddTransient<TeachPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<WordsViewModel>();
            builder.Services.AddTransient<WordsPage>();
            builder.Services.AddTransient<ResumePage>();
            builder.Services.AddTransient<ResumeViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
