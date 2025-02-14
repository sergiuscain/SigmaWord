

using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using SigmaWord.Models;
using System.Reflection;

namespace SigmaWord.Services
{
    public class VocabularyService
    {
        public VocabularyService() 
        {
            
        }
        public async Task<List<FlashCard>> LoadWordsAsync()
        {
            List<FlashCard> flashcards = new List<FlashCard>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"SigmaWord.Resources.Words.nature.json";
            var files = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var jsonFile = await reader.ReadToEndAsync();
                        var deserializeResult = JsonConvert.DeserializeObject<List<FlashCard>>(jsonFile);
                        flashcards.AddRange(deserializeResult);
                    }
                }
            }
            return flashcards;
        }
    }
}
