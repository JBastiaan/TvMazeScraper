using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Persistance.Repositories
{
    public interface IShowRepository
    {
        Task<int> GetLatestShowIdAsync();
        Task<IEnumerable<Show>> GetShowsAsync(int pageIndex);
        Task AddShowsAsync(IEnumerable<Show> shows);
        Task<int> SaveChangesAsync();
    }
}