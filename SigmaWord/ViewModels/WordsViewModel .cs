using CommunityToolkit.Mvvm.ComponentModel;
using SigmaWord.Data.Entities;
using SigmaWord.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.ViewModels
{
    public partial class WordsViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        public ObservableCollection<FlashCard> Words { get; set; } = new ObservableCollection<FlashCard>();
        public WordsViewModel(DbService dbService)
        {
            _dbService = dbService;
        }
    }
}
