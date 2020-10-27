using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Scraper.Dto;

namespace TvMazeScraper.Scraper.Clients
{
    public interface ITvMazeClient
    {
        Task<List<ShowDto>> GetShowsAsync(int pagenumber);
        Task<List<ActorDto>> GetCastAsync(int showId);
    }
}