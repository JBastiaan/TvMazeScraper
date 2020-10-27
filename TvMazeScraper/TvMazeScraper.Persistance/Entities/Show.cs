using System.Collections.Generic;

namespace TvMazeScraper.Persistance.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ShowActor> ShowActors { get; set; }
    }
}