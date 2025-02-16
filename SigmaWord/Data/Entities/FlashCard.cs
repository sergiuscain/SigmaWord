namespace SigmaWord.Data.Entities
{
    public class FlashCard
    {
        public int Id { get; set; } // Уникальный идентификатор карточки
        public string Word { get; set; } // Само слово
        public string Translation { get; set; } // Перевод слова
        public ICollection<ExampleSentence> ExampleSentences { get; set; } // Примеры предложений
        public ICollection<Category> Categories { get; set; } // Список категорий
        public int RequiredRepetitions { get; set; } // Необходимое количество повторений
        public int CurrentRepetitions { get; set; } // Текущее количество повторений
        public WordStatus Status { get; set; } // Статус слова

        public string GetLearningStatus()
        {
            if (Status == WordStatus.Learning)
            {
                double percentage = (double)CurrentRepetitions / RequiredRepetitions * 100;
                return $"Изучается: {percentage}%";
            }
            return string.Empty;
        }
    }
}
