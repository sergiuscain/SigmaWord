
namespace SigmaWord.Services
{
    public class SpeechService
    {

        public SpeechService()
        {
            
        }
        public async Task Speak(string wordOrText)
        {
            await TextToSpeech.SpeakAsync(wordOrText);
        }
    }
}
