using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Api.Models;
using TvMazeScraper.Persistance.Repositories;

namespace TvMazeScraper.Api.Controllers
{
    public class ShowController : Controller
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public ShowController(
            IShowRepository showRepository,
            IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("shows/{pageIndex}")]
        public async Task<ObjectResult> GetShows([FromRoute]int pageIndex)
        {
            var shows = await _showRepository.GetShowsAsync(pageIndex);
            var showVms = _mapper.Map<IEnumerable<ShowVm>>(shows);

            return new ObjectResult(showVms);
        }
    }
}