namespace SigmaWord.Data.Entities
{
    public partial class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<FlashCard> WordCards { get; set; } = new List<FlashCard>();
    }
}