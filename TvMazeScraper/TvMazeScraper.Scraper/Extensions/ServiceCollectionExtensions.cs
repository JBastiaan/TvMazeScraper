using System;
using System.IO;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Configuration;
using TvMazeScraper.Scraper.Repositories;

namespace TvMazeScraper.Scraper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static TvMazeScraperConfiguration _configuration;

        public static IServiceCollection AddConfiguration(
            this IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var iConfig = configBuilder.Build();
            _configuration = new TvMazeScraperConfiguration();

            iConfig.Bind(_configuration);

            services.AddSingleton(_configuration);

            return services;
        }

        public static IServiceCollection AddScraper(
            this IServiceCollection services)
        {
            services
                .AddTransient<ITvMazeScraper, TvMazeScraper>()
                .AddScraperContext()
                .AddScraperClient()
                .AddRepositories();

            return services;
        }


        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services
                .AddTransient<ITvMazeScraper, TvMazeScraper>()
                .AddTransient<IActorRepository, ActorRepository>()
                .AddTransient<IShowRepository, ShowRepository>();

            return services;
        }

        private static IServiceCollection AddScraperContext(
            this IServiceCollection services)
        {
            services
                .AddDbContext<TvMazeScraperContext>(options => options.UseSqlServer(_configuration.ConnectionStrings.TvMazeScraperDb));

            return services;
        }

        private static IServiceCollection AddScraperClient(
            this IServiceCollection services)
        {
            services
                .AddHttpClient(
                    nameof(TvMazeClient),
                    (s, cfg) =>
                    {
                        cfg.BaseAddress = new Uri(_configuration.TvMazeScraperApi.BaseAddress);
                        cfg.DefaultRequestHeaders.Accept.ParseAdd(MediaTypeNames.Application.Json);
                    })
                .AddPolicyHandler(TvMazeClient.GetRetryPolicy())
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddTypedClient<ITvMazeClient, TvMazeClient>();

            return services;
        }
    }
}