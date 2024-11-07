﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReactJokes.Data;

#nullable disable

namespace ReactJokes.Data.Migrations
{
    [DbContext(typeof(ReactJokesDbContext))]
    [Migration("20241031174121_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ReactJokes.Data.Joke", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Punchline")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Setup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Jokes");
                });

            modelBuilder.Entity("ReactJokes.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ReactJokes.Data.UserJoke", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("JokeId")
                        .HasColumnType("int");

                    b.Property<bool>("Liked")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "JokeId");

                    b.HasIndex("JokeId");

                    b.ToTable("UserJokes");
                });

            modelBuilder.Entity("ReactJokes.Data.UserJoke", b =>
                {
                    b.HasOne("ReactJokes.Data.Joke", "Joke")
                        .WithMany("UserJokes")
                        .HasForeignKey("JokeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ReactJokes.Data.User", "User")
                        .WithMany("UserJokes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Joke");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReactJokes.Data.Joke", b =>
                {
                    b.Navigation("UserJokes");
                });

            modelBuilder.Entity("ReactJokes.Data.User", b =>
                {
                    b.Navigation("UserJokes");
                });
#pragma warning restore 612, 618
        }
    }
}
