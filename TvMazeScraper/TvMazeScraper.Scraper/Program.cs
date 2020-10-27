using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance;
using TvMazeScraper.Scraper.Extensions;

namespace TvMazeScraper.Scraper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = Startup();

            await serviceProvider.GetService<TvMazeScraperContext>().Database.EnsureCreatedAsync();

            var scraper = serviceProvider.GetService<ITvMazeScraper>();
            await scraper.ScrapeAsync();

            Console.ReadKey();
        }

        private static ServiceProvider Startup()
        {

            var services = new ServiceCollection();

            services
                .AddConfiguration()
                .AddScraper()
                .AddAutoMapper(typeof(Program).Assembly);

            return services.BuildServiceProvider();
        }
    }
}