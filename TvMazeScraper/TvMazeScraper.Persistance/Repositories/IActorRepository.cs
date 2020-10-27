using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance.Repositories
{
    public interface IActorRepository
    {
        Task AddActorsAsync(IEnumerable<Actor> newActors);
    }
}