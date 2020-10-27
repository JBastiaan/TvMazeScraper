using System;

namespace TvMazeScraper.Scraper.Dto
{

    public class PersonDto : IEquatable<PersonDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public bool Equals(PersonDto x, PersonDto y)
        {
            return Equals(x?.Id, y?.Id);
        }

        public int GetHashCode(PersonDto obj)
        {
            return obj.Id.GetHashCode();
        }

        public bool Equals(PersonDto other)
        {
            return Id == other?.Id ;
        }

        public override bool Equals(object obj)
        {
            return Equals((PersonDto) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}