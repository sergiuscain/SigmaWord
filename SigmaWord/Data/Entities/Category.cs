namespace SigmaWord.Data.Entities
{
    public class Category
    {
        public int Id { get; set; } // Уникальный идентификатор категории
        public string Name { get; set; } // Название категории
        public ICollection<FlashCard> WordCards { get; set; } // Список карточек слов в категории
    }
}