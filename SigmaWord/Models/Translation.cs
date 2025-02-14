using SQLite;

namespace SigmaWord.Models
{
    [Table("Translation")]
    public class Translation
    {
        [PrimaryKey]
        [AutoIncrement]
        public Guid Id { get; set; }
        // Перевод слова
        public string Word { get; set; }
        // Внешний ключ для связи с FlashCard
        public Guid FlashCardId { get; set; }
    }
}
