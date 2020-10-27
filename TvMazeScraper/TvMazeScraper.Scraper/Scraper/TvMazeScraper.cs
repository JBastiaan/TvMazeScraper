using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeScraper.Persistance.Entities;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Repositories;
using Show = TvMazeScraper.Persistance.Entities.Show;

namespace TvMazeScraper.Scraper
{
    public class TvMazeScraper : ITvMazeScraper
    {
        private readonly ITvMazeClient _client;
        private readonly IShowRepository _showRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public TvMazeScraper(
            ITvMazeClient client,
            IShowRepository showRepository,
            IActorRepository actorRepository,
            IMapper mapper)
        {
            _client = client;
            _showRepository = showRepository;
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Scrapes TvMaze for all Shows that are not yet persisted
        /// </summary>
        /// <returns></returns>
        public async Task ScrapeAsync()
        {
            var lastKnownShow = await _showRepository.GetLatestShowIdAsync();
            var pageIndex = lastKnownShow / Constants.TvMazePageSize;
            if (pageIndex == 0)
            {
                pageIndex++;
            }

            var showsToAdd = new List<Models.Show>();
            var personsToAdd = new List<Models.Person>();
            while (true)
            {
                var shows = await _client.GetShowsAsync(pageIndex);

                if (!shows.Any())
                {
                    break;
                }

                foreach (var show in shows)
                {
                    if (show.Id > lastKnownShow)
                    {
                        var cast = await _client.GetCastAsync(show.Id);

                        personsToAdd.AddRange(cast.Select(c => c.Person));
                        showsToAdd.Add(show);
                    }
                }

                pageIndex++;
                //break;
            }

            await _showRepository
                .AddShowsAsync(_mapper.Map<IEnumerable<Show>>(showsToAdd));

            await _actorRepository
                .AddActorsAsync(_mapper.Map<IEnumerable<Actor>>(personsToAdd));

            await _showRepository.SaveChangesAsync();
        }
    }
}