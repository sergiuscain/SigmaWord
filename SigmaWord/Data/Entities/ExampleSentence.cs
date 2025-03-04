namespace SigmaWord.Data.Entities
{
    public partial class ExampleSentence
    {
        public int Id { get; set; }

        public string Sentence { get; set; } = null!;

        public string Translation { get; set; } = null!;

        public int WordCardId { get; set; }

        public virtual FlashCard WordCard { get; set; } = null!;
    }
}