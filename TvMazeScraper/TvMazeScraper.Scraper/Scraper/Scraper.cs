using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Models;

namespace TvMazeScraper.Scraper
{
    public class Scraper : IScraper
    {
        private readonly ITvMazeClient _client;
        private readonly IMapper _mapper;

        public Scraper(
            ITvMazeClient client,
            IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task ScrapeAsync()
        {
            //Get all shows
            var result = new List<Show>();
            var pageIndex = 1;
            while (pageIndex < 5)
            {

                var shows = await _client.GetShowsAsync(pageIndex);

                if (!shows.Any())
                {
                    break;
                }

                result.AddRange(shows);
                pageIndex++;
            }

            //Get All actors for shows





            throw new System.NotImplementedException();
        }
    }
}
