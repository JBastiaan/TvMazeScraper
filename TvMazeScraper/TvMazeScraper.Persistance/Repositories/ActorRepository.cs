using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance.Repositories
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
    }
}