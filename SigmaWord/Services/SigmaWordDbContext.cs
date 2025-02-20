using Microsoft.EntityFrameworkCore;
using SigmaWord.Data.Entities;

namespace SigmaWord.Services
{
    public class SigmaWordDbContext : DbContext
    {
        public SigmaWordDbContext()
        {

        }
        public DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ExampleSentence> ExampleSentence { get; set; }
        public DbSet<DailyStatistics> DailyStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка уникальности для DailyStatistics
            modelBuilder.Entity<DailyStatistics>()
                .HasIndex(d => d.Date)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var pathDb = PathDb.GetPath("SigmaWordDb.db");
            optionsBuilder.UseSqlite(pathDb);
        }
    }
}
