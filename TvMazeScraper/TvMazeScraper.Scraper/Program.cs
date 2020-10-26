using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance;
using TvMazeScraper.Scraper.Clients;

namespace TvMazeScraper.Scraper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //setup our DI
            var services = new ServiceCollection();
            services
                .AddTransient<IScraper, Scraper>()
                .AddAutoMapper(typeof(Program).Assembly);
            
            services
                .AddPolicyHandler(TvMazeClient.GetRetryPolicy())
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

            var scraper = provider.GetService<IScraper>();

            await scraper.ScrapeAsync();
            

            var context = new TvMazeScraperContext();

            Console.WriteLine("Entity Framework Core Code-First sample");
            Console.WriteLine();

            await TvMazeScraperContextSeeder.Seed(context);

            Console.WriteLine("Database created");
            Console.WriteLine();





            context.SaveChanges();

            Console.WriteLine("Saved changes");
            Console.WriteLine();

            Console.ReadKey();


        }
    }
}