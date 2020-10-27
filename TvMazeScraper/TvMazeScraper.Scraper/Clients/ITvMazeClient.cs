using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Scraper.Models;

namespace TvMazeScraper.Scraper.Clients
{
    public interface ITvMazeClient
    {
        Task<IEnumerable<Show>> GetShowsAsync(int pagenumber);
        Task<IEnumerable<Actor>> GetCastAsync(int showId);
    }
}