using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Data.Entities
{
    public class DailyStatistics
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }
        public int TotalRepeats { get; set; }
        public int TotalWordsStudied { get; set; }
        public int TotalWordsStarted { get; set; } = 0;
        public int TotalKnownWords { get; set; } = 0; 
    }
}
