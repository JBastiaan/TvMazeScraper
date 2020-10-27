using System.Threading.Tasks;

namespace TvMazeScraper.Scraper
{
    public interface ITvMazeScraper
    {
        Task ScrapeAsync();
    }
}