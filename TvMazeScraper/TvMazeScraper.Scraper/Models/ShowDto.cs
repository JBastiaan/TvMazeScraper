using System.Collections.Generic;

namespace TvMazeScraper.Scraper.Models
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PersonDto> Cast { get; set; }
    }
}