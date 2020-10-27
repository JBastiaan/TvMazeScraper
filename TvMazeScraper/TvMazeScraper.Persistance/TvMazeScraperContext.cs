using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance
{
    public class TvMazeScraperContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowActor> ShowActors { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Many to many relationships require some additional configuration in EF Core since a join entity is needed for now
            modelBuilder
                .Entity<ShowActor>()
                .HasKey(sa => new { sa.ShowId, sa.ActorId });
            
            modelBuilder
                .Entity<ShowActor>()
                .HasOne(sa => sa.Show)
                .WithMany(show => show.ShowActors)
                .HasForeignKey(sa => sa.ShowId);
            
            modelBuilder
                .Entity<ShowActor>()
                .HasOne(sa => sa.Actor)
                .WithMany(actor => actor.ShowActors)
                .HasForeignKey(actor => actor.ActorId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TvMazeScraper;User Id=sa;Password=Development_;");
        }
    }
}