using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private const int MaxPageSize = 250;
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
            IQueryable<Show> query = _context
                .Shows
                .Include(show => show.ShowActors)
                .ThenInclude(sa => sa.Actor);

            if (pageIndex > 1)
            {
                query = query.Skip(pageIndex - 1 * MaxPageSize);
            }

            return await query
                .Take(MaxPageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddShowsAsync(IEnumerable<Show> newShows)
        {
            var showsToAdd = new List<Show>();

            foreach (var show in newShows.Where(newShow => !_context.Shows.Any(show => show.Id == newShow.Id)))
            {
                showsToAdd.Add(show);
            }

            await _context
                .Shows
                .AddRangeAsync(showsToAdd);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}