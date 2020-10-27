using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Scraper.Repositories
{
    public interface IActorRepository
    {
        Task AddActorsAsync(IEnumerable<Actor> actors);
        Task<int> SaveChangesAsync();
    }
}