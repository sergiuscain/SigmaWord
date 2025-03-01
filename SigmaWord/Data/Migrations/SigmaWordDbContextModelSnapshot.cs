﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SigmaWord.Services;

#nullable disable

namespace SigmaWord.Data.Migrations
{
    [DbContext(typeof(SigmaWordDbContext))]
    partial class SigmaWordDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.13");

            modelBuilder.Entity("CategoryFlashCard", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WordCardsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoriesId", "WordCardsId");

                    b.HasIndex("WordCardsId");

                    b.ToTable("CategoryFlashCard");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.DailyStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalKnownWords")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("TotalRepeats")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalWordsStarted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("TotalWordsStudied")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Date")
                        .IsUnique();

                    b.ToTable("DailyStatistics");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.ExampleSentence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Sentence")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WordCardId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WordCardId");

                    b.ToTable("ExampleSentence");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.FlashCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentRepetitions")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastRepeatDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NextRepeatDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("RequiredRepetitions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FlashCards");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DailyWordGoal")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SelectedTheme")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("CategoryFlashCard", b =>
                {
                    b.HasOne("SigmaWord.Data.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SigmaWord.Data.Entities.FlashCard", null)
                        .WithMany()
                        .HasForeignKey("WordCardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.ExampleSentence", b =>
                {
                    b.HasOne("SigmaWord.Data.Entities.FlashCard", "WordCard")
                        .WithMany("ExampleSentences")
                        .HasForeignKey("WordCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordCard");
                });

            modelBuilder.Entity("SigmaWord.Data.Entities.FlashCard", b =>
                {
                    b.Navigation("ExampleSentences");
                });
#pragma warning restore 612, 618
        }
    }
}
