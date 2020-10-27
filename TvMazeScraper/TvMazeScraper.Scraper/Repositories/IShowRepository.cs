using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Scraper.Repositories
{
    public interface IShowRepository
    {
        Task<int> GetLatestShowIdAsync();
        Task<IEnumerable<Show>> GetShowsAsync(int pageIndex);
        Task AddShowsAsync(IEnumerable<Show> shows);
        Task<int> SaveChangesAsync();
    }
}