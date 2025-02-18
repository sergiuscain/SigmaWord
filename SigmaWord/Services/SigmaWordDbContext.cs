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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "SigmaWordDb.db");
            var downloadPath = $"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SigmaWordDb.db")}";
            var pathOnAndroid = $"Filename = {dbPath}";
            optionsBuilder.UseSqlite(pathOnAndroid);
        }
    }
}
