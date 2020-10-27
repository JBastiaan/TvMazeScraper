using System;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance.Extensions;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Configuration;

namespace TvMazeScraper.Scraper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly TvMazeScraperConfiguration Configuration = new TvMazeScraperConfiguration();

        public static IServiceCollection AddConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            configuration.Bind(Configuration);

            services.AddSingleton(Configuration);

            return services;
        }

        public static IServiceCollection AddScraper(
            this IServiceCollection services)
        {
            services
                .AddTransient<ITvMazeScraper, TvMazeScraper>()
                .AddScraperContext(Configuration.ConnectionStrings.TvMazeScraperDb)
                .AddScraperClient()
                .AddRepositories();

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
                        cfg.BaseAddress = new Uri(Configuration.TvMazeScraperApi.BaseAddress);
                        cfg.DefaultRequestHeaders.Accept.ParseAdd(MediaTypeNames.Application.Json);
                    })
                .AddPolicyHandler(TvMazeClient.GetRetryPolicy())
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddTypedClient<ITvMazeClient, TvMazeClient>();

            return services;
        }
    }
}