using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Data.Entities
{
    public class UserSettings
    {
        public int Id { get; set; } // Уникальный идентификатор настроек
        public int DailyWordGoal { get; set; } = 0; // Цель выученных слов на день
        public string SelectedTheme { get; set; } = Themes.DarkPurple.ToString(); // Выбранная тема приложения
    }
}
