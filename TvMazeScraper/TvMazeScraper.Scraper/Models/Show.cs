using System.Collections.Generic;

namespace TvMazeScraper.Scraper.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Actor> Actors { get; set; }
    }
}