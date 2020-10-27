using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Persistance.Repositories;
using TvMazeScraper.Scraper.Clients;
using TvMazeScraper.Scraper.Dto;
using Actor = TvMazeScraper.Persistance.Entities.Actor;
using Show = TvMazeScraper.Persistance.Entities.Show;

namespace TvMazeScraper.Scraper
{
    public class TvMazeScraper : ITvMazeScraper
    {
        private readonly ITvMazeClient _client;
        private readonly IShowRepository _showRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TvMazeScraper(
            ITvMazeClient client,
            IShowRepository showRepository,
            IActorRepository actorRepository,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            _client = client;
            _showRepository = showRepository;
            _actorRepository = actorRepository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger(nameof(TvMazeScraper));
        }

        /// <summary>
        /// Scrapes TvMaze for all Shows that are not yet persisted
        /// </summary>
        /// <returns></returns>
        public async Task ScrapeAsync()
        {
            var latestShowId = await GetLatestShowIdAsync();

            var pageIndex = latestShowId / Constants.TvMazePageSize;
            if (pageIndex == 0)
            {
                pageIndex++;
            }

            var showsToAdd = new List<ShowDto>();
            var personsToAdd = new List<PersonDto>();
            while (true)
            {
                _logger.LogInformation($"Retrieving shows for page {pageIndex}");
                var shows = await _client.GetShowsAsync(pageIndex);

                if (!shows.Any())
                {
                    _logger.LogInformation($"No shows found on page {pageIndex}");
                    break;
                }

                foreach (var show in shows)
                {
                    if (show.Id > latestShowId)
                    {
                        _logger.LogInformation($"Retrieving cast for show '{show.Name}'");
                        var cast = await _client.GetCastAsync(show.Id);

                        personsToAdd.AddRange(cast.Select(c => c.PersonDto));

                        //1 actor can play as multiple characters
                        show.Cast = cast.Select(actor => actor.PersonDto).Distinct().ToList();
                        showsToAdd.Add(show);
                    }
                }

                _logger.LogInformation($"Persisting new shows for page {pageIndex}");

                await _showRepository
                    .AddShowsAsync(_mapper.Map<IEnumerable<Show>>(showsToAdd));

                await _actorRepository
                    .AddActorsAsync(_mapper.Map<IEnumerable<Actor>>(personsToAdd.Distinct()));

                await _showRepository.SaveChangesAsync();

                pageIndex++;
            }


        }

        private async Task<int> GetLatestShowIdAsync()
        {
            _logger.LogInformation("Retrieving latest known showId");
            var latestShowId = await _showRepository.GetLatestShowIdAsync();
            _logger.LogInformation($"Latest known showId is {latestShowId}");

            return latestShowId;
        }
    }
}