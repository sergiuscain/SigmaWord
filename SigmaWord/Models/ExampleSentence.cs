using Newtonsoft.Json;
using SQLite;

namespace SigmaWord.Models
{
    [Table("ExampleSentence")]
    public class ExampleSentence
    {
        [PrimaryKey]
        [AutoIncrement]
        public Guid Id { get; set; }
        //Предложение
        [JsonProperty("sentence")]
        public string Sentence { get; set; }
        //Перевод предложения
        [JsonProperty("translation")]
        public string Translation { get; set; }
        // Внешний ключ для связи с FlashCard
        public Guid FlashCardId { get; set; } 
    }
}
