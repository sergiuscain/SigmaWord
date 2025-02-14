using SQLite;

namespace SigmaWord.Models
{
    [Table("FlashCard")]
    public class FlashCard
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("category")]
        public string CategoryName { get; set; }
        [Column("word_original")]
        public string Word { get; set; }
        [Column("word_translate")]
        public int NeedToRepeat { get; set; }
        [Column("already_repeated")]
        public int AlreadyRepeated { get; set; }
        [Column("is_in_studying")]
        public bool IsInStudying { get; set; }

        // Связи с примерами предложений и переводами
        [Ignore]
        public List<ExampleSentence> ExampleSentences { get; set; }

        [Ignore]
        public List<Translation> Translations { get; set; }
    }
}
