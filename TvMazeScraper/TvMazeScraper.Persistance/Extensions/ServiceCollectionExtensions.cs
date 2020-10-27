using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistance.Repositories;

namespace TvMazeScraper.Persistance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services
                .AddTransient<IActorRepository, ActorRepository>()
                .AddTransient<IShowRepository, ShowRepository>();

            return services;
        }

        public static IServiceCollection AddScraperContext(
            this IServiceCollection services,
            string connectionString)
        {
            services
                .AddDbContext<TvMazeScraperContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}