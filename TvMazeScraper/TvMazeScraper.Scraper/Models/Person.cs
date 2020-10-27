using System;
using System.Collections.Generic;

namespace TvMazeScraper.Scraper.Models
{

    public class Person : IEqualityComparer<Person>
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
    }
}