using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Models
{
    class AppSettings
    {
        /// <summary>
        /// Ежедневная цель по изучению слов
        /// </summary>
        public int DailyWordGoal { get; set; }
        /// <summary>
        /// Отвечает за включение озвучки слов
        /// </summary>
        public bool IsPronunciationEnabled { get; set; }
        /// <summary>
        /// Выбранная тема приложения
        /// </summary>
        public string SelectedTheme { get; set; } = ThemesEnum.Темная.ToString();
    }
}
