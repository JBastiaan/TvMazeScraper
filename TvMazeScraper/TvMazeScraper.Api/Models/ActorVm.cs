using System;

namespace TvMazeScraper.Api.Models
{
    public class ActorVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}