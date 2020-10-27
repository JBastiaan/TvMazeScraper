using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Persistance;
using TvMazeScraper.Scraper.Extensions;

namespace TvMazeScraper.Scraper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Program starting...");
            var serviceProvider = Startup();

            Console.WriteLine("Creating database if necessary");
            await serviceProvider.GetService<TvMazeScraperContext>().Database.EnsureCreatedAsync();

            var scraper = serviceProvider.GetService<ITvMazeScraper>();
            Console.WriteLine("Scrape started");
            await scraper.ScrapeAsync();
            Console.WriteLine("Scrape completed");
            Console.ReadKey();
        }

        private static ServiceProvider Startup()
        {
            var services = new ServiceCollection();
            var configuration = GetConfiguration();

            services
                .AddConfiguration(configuration)
                .AddLogging(builder => builder
                    .AddConsole()
                    .AddConfiguration(configuration.GetSection("Logging")))
                .AddScraper()
                .AddAutoMapper(typeof(Program).Assembly);

            return services.BuildServiceProvider();
        }

        public static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return configBuilder.Build();
        }
    }
}