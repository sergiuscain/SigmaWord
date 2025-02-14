using SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SigmaWord.Models
{
    [Table("FlashCard")]
    public class FlashCard
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public Guid Id { get; set; }

        [JsonProperty("category")]
        [Column("category")]
        public string CategoryName { get; set; }

        [JsonProperty("word")]
        [Column("word_original")]
        public string Word { get; set; }

        [JsonProperty("translations")]
        [Ignore]
        public List<string> Translations { get; set; }

        [JsonProperty("examples")]
        [Ignore]
        public List<ExampleSentence> ExampleSentences { get; set; }

        [Column("word_translate")]
        public double NeedToRepeat { get; set; }

        [Column("already_repeated")]
        public double AlreadyRepeated { get; set; }

        [Column("is_in_studying")]
        public bool IsInStudying { get; set; }

        [Ignore]
        public string ProgressBar => IsInStudying ? $"Прогресс: {(AlreadyRepeated / (float)NeedToRepeat) * 100}%" : "не изучается";
    }
}
