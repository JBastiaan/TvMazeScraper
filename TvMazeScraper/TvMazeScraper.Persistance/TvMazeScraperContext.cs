using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance
{
    public class TvMazeScraperContext : DbContext
    {
        public TvMazeScraperContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowActor> ShowActors { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShowActor>()
                .HasKey(sp => new { sp.ShowId, sp.ActorId });

            modelBuilder.Entity<Show>()
                .HasKey("Id");

            modelBuilder.Entity<Show>()
                .Property(s => s.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Actor>()
                .HasKey("Id");

            modelBuilder.Entity<Actor>()
                .Property(c => c.Id)
                .ValueGeneratedNever();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TvMazeScraper;User Id=sa;Password=Development_;");
        }
    }
}