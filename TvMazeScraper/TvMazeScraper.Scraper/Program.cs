using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Repositories;

namespace TvMazeScraper.Scraper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //setup our DI
            var services = new ServiceCollection();
            services
                .AddTransient<ITvMazeScraper, TvMazeScraper>()
                .AddTransient<IShowRepository, ShowRepository>()
                .AddScoped<TvMazeScraperContext>()
                .AddAutoMapper(typeof(Program).Assembly)
                .AddHttpClient(
                    nameof(TvMazeClient),
                    (s, cfg) =>
                    {
                        cfg.BaseAddress = new Uri("http://api.tvmaze.com");
                        cfg.DefaultRequestHeaders.Accept.ParseAdd(MediaTypeNames.Application.Json);
                    })
                .AddPolicyHandler(TvMazeClient.GetRetryPolicy())
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddTypedClient<ITvMazeClient, TvMazeClient>();
            
            var provider = services.BuildServiceProvider();

            var scraper = provider.GetService<ITvMazeScraper>();


            var context = new TvMazeScraperContext();

            Console.WriteLine("Entity Framework Core Code-First sample");
            Console.WriteLine();

            await TvMazeScraperContextSeeder.Seed(context);

            await scraper.ScrapeAsync();
            

            Console.ReadKey();


        }
    }
}