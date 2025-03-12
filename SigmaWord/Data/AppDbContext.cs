using Microsoft.EntityFrameworkCore;
using SigmaWord.Data.Entities;
using SigmaWord.Services;

namespace SigmaWord.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<ExampleSentence> ExampleSentences { get; set; }

        public virtual DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<DailyStatistics> DailyStatistics { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка уникальности для DailyStatistics
            modelBuilder.Entity<DailyStatistics>()
                .HasIndex(d => d.Date)
                .IsUnique();
            //Устоновка значений по умолчанию.
            modelBuilder.Entity<DailyStatistics>()
                .Property(d => d.TotalWordsStarted)
                .HasDefaultValue(0);
            modelBuilder.Entity<DailyStatistics>()
                .Property(d => d.TotalKnownWords)
                .HasDefaultValue(0);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasMany(d => d.WordCards).WithMany(p => p.Categories)
                    .UsingEntity<Dictionary<string, object>>(
                        "CategoryFlashCard",
                        r => r.HasOne<FlashCard>().WithMany().HasForeignKey("WordCardsId"),
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoriesId"),
                        j =>
                        {
                            j.HasKey("CategoriesId", "WordCardsId");
                            j.ToTable("CategoryFlashCard");
                            j.HasIndex(new[] { "WordCardsId" }, "IX_CategoryFlashCard_WordCardsId");
                        });
            });

            modelBuilder.Entity<ExampleSentence>(entity =>
            {
                entity.ToTable("ExampleSentence");

                entity.HasIndex(e => e.WordCardId, "IX_ExampleSentence_WordCardId");

                entity.HasOne(d => d.WordCard).WithMany(p => p.ExampleSentences).HasForeignKey(d => d.WordCardId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pathDb = PathDb.GetPath("SigmaWordDb.db");
            optionsBuilder.UseSqlite(pathDb);
        }
    }
}
