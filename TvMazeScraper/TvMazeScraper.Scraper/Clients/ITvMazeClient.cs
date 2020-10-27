using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Scraper.Models;

namespace TvMazeScraper.Scraper.Clients
{
    public interface ITvMazeClient
    {
        Task<List<Show>> GetShowsAsync(int pagenumber);
        Task<List<Actor>> GetCastAsync(int showId);
    }
}