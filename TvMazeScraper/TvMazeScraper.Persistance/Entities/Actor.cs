using System;
using System.Collections.Generic;

namespace TvMazeScraper.Persistance.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public ICollection<ShowActor> ShowActors { get; set; }
    }
}