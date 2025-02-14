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
        public string Sentence { get; set; }
        //Перевод предложения
        public string Translation { get; set; }
        // Внешний ключ для связи с FlashCard
        public Guid FlashCardId { get; set; } 
    }
}
