using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Models
{
    public class FlashCard
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Word { get; set; }
        public List<string> Translation { get; set; }
        public List<string> ExampleSentence { get; set; }
        public int NeedToTepeat { get; set; }
        public int AlreadyRepeated { get; set; }
        public bool IsInStudying { get; set; }
    }
}
