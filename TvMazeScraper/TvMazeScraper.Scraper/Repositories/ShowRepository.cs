using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Persistance;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Scraper.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly TvMazeScraperContext _context;

        public ShowRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public async Task<int> GetLatestShowIdAsync()
        {
            return await _context
                .Shows
                .Select(show => show.Id)
                .OrderByDescending(id => id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsAsync(int pageIndex)
        {
            return await _context
                .Shows
                .Skip(pageIndex * Constants.TvMazePageSize)
                .Take(Constants.TvMazePageSize)
                .ToListAsync();
        }

        public async Task AddShowsAsync(IEnumerable<Show> shows)
        {
            await _context
                .Shows
                .AddRangeAsync(shows);
        }

        public async Task<int> SaveChangesAsync()
        {
            int affectedRows;
            await _context.Database.OpenConnectionAsync();
            try
            {
                //https://docs.microsoft.com/en-us/ef/core/saving/explicit-values-generated-properties#saving-an-explicit-value-during-add
                //await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Shows ON");
                //await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Actors ON");
                affectedRows = await _context.SaveChangesAsync();
                //await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Shows OFF");
                //await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Actors OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return affectedRows;
        }
    }
}