using System;

namespace TvMazeScraper.Scraper.Models
{

    public class Person : IEquatable<Person>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public bool Equals(Person x, Person y)
        {
            return Equals(x?.Id, y?.Id);
        }

        public int GetHashCode(Person obj)
        {
            return obj.Id.GetHashCode();
        }

        public bool Equals(Person other)
        {
            return Id == other?.Id ;
        }

        public override bool Equals(object obj)
        {
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}