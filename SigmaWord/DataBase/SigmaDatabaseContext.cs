using Microsoft.EntityFrameworkCore;
using SigmaWord.DataBase.Configurations;
using SigmaWord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.DataBase
{
    public class SigmaDatabaseContext(DbContextOptions<SigmaDatabaseContext> options) 
        : DbContext(options)
    {
        DbSet<FlashCard> flashcards;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FlashCardConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
