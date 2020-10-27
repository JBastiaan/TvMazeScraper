using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Api.Models
{
    public class ShowVm
    {
        private List<ActorVm> _actors;

        public int Id { get; set; }
        public string Name { get; set; }

        public List<ActorVm> Actors
        {
            get => _actors
                .OrderByDescending(actor => actor.Birthday)
                .ToList();
            set => _actors = value;
        }
    }
}
