using Microsoft.EntityFrameworkCore;
using SigmaWord.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Services
{
    public class DbService
    {
        private readonly SigmaWordDbContext _context;
        public DbService(SigmaWordDbContext context)
        {
            _context = context;
        }
        public async Task AddCardAsync(FlashCard card)
        {
            await _context.AddAsync(card);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCardAsync(FlashCard card)
        {
            _context.FlashCards.Remove(card);
            await _context.SaveChangesAsync();
        }
        public async Task<List<FlashCard>> GetAllCardsAsync()
        {
            var cards = await _context.FlashCards.ToListAsync();
            return cards;
        }
    }
}
