using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Persistance.Repositories;

namespace TvMazeScraper.Api.Controllers
{
    public class ShowController : Controller
    {
        private readonly IShowRepository _showRepository;

        public ShowController(
            IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        [HttpGet]
        [Route("shows")]
        public async Task<ObjectResult> GetShows([FromQuery]int pageIndex)
        {
            var shows = await _showRepository.GetShowsAsync(pageIndex);

            return new ObjectResult(shows);
        }
    }
}