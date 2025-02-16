using Microsoft.Extensions.Logging;
using SigmaWord.Services;
using SigmaWord.ViewModels;
using SigmaWord.Views;

namespace SigmaWord
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Сервис по обработки слов.
            builder.Services.AddSingleton<VocabularyService>();

            //Добавляем сервис для работы с базой данных SQLite
            builder.Services.AddDbContext<SigmaWordDbContext>();

            //Сервисы для каждой вьбшки и её модели представления.
            builder.Services.AddTransient<DictionaryViewModel>();
            builder.Services.AddTransient<DictionaryPage>();
            builder.Services.AddTransient<TeachViewModel>();
            builder.Services.AddTransient<TeachPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
