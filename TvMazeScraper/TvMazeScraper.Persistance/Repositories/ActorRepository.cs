using System.Collections.Generic;
using System.Linq;
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

        public async Task AddActorsAsync(IEnumerable<Actor> newActors)
        {
            var actorsToAdd = new List<Actor>();
            
            foreach (var actor in newActors.Where(newActor => !_context.Actors.Any(actor => actor.Id == newActor.Id)))
            {
                actorsToAdd.Add(actor);
            }

            await _context.AddRangeAsync(actorsToAdd);
        }
    }
}