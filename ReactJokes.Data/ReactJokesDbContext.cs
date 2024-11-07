using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ReactJokes.Data
{
    public class ReactJokesDbContext : DbContext
    {
        private string _connectionString;
        public ReactJokesDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<UserJoke>()
                .HasKey(uj => new { uj.UserId, uj.JokeId });

            modelBuilder.Entity<UserJoke>()
                .HasOne(u => u.User)
                .WithMany(j => j.UserJokes)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserJoke>()
                .HasOne(j => j.Joke)
                .WithMany(u => u.UserJokes)
                .HasForeignKey(j => j.JokeId);

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserJoke> UserJokes { get; set; }
    }

}