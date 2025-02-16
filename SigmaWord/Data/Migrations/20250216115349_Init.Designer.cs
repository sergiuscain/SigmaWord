﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SigmaWord.Services;

#nullable disable

namespace SigmaWord.Data.Migrations
{
    [DbContext(typeof(SigmaWordDbContext))]
    [Migration("20250216115349_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
