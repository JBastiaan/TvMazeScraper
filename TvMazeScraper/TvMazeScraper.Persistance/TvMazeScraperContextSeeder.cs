using System.Threading.Tasks;

namespace TvMazeScraper.Persistance
{
    public class TvMazeScraperContextSeeder
    {
        public static async Task Seed(TvMazeScraperContext context)
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}