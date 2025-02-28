using CommunityToolkit.Mvvm.ComponentModel;

namespace SigmaWord.ViewModels
{
    public partial class WordStudyViewModek : ObservableObject
    {
        public WordStudyViewModek(string test) 
        { 
            this.test = test;
        }
        [ObservableProperty]
        public string test;
    }
}
