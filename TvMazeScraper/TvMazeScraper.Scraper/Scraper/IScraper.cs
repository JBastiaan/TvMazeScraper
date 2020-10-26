using System.Threading.Tasks;

namespace TvMazeScraper.Scraper
{
    public interface IScraper
    {
        Task ScrapeAsync();
    }
}