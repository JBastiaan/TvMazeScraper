using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TvMazeScraper.Api.Configuration;
using TvMazeScraper.Persistance.Extensions;

namespace TvMazeScraper.Api
{
    public class Startup
    {
        public TvMazeScraperApiConfiguration ApiConfiguration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var apiConfiguration = new TvMazeScraperApiConfiguration();
            Configuration.Bind(apiConfiguration);
            ApiConfiguration = apiConfiguration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers();

            services
                .AddRepositories()
                .AddScraperContext(ApiConfiguration.ConnectionStrings.TvMazeScraperDb);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}