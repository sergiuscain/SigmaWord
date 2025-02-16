namespace SigmaWord.Data.Entities
{
    public class ExampleSentence
    {
        public int Id { get; set; } // Уникальный идентификатор примера
        public string Sentence { get; set; } // Предложение с изучаемым словом
        public string Translation { get; set; } // Перевод предложения
        public int WordCardId { get; set; } // Идентификатор карточки слова
        public FlashCard WordCard { get; set; } // Связь с карточкой слова
    }
}