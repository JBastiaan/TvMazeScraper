using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Persistance;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Scraper.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly TvMazeScraperContext _context;

        public ActorRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public async Task AddActorsAsync(IEnumerable<Actor> actors)
        {
            await _context
                .Actors
                .AddRangeAsync(actors);
        }

        public async Task<int> SaveChangesAsync()
        {
            int affectedRows;
            await _context.Database.OpenConnectionAsync();
            try
            {
                //https://docs.microsoft.com/en-us/ef/core/saving/explicit-values-generated-properties#saving-an-explicit-value-during-add
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Actors ON");
                affectedRows = await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Actors OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return affectedRows;
        }
    }
}