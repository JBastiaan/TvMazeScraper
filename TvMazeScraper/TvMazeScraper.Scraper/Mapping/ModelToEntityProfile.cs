using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TvMazeScraper.Persistance.Entities;
using TvMazeScraper.Scraper.Models;
using Actor = TvMazeScraper.Scraper.Models.Actor;
using Show = TvMazeScraper.Persistance.Entities.Show;

namespace TvMazeScraper.Scraper.Mapping
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<Models.Show, Show>()
                .ForMember(
                    show => show.ShowActors, 
                    opt => opt.MapFrom(source => source));

            CreateMap<Models.Show, IEnumerable<ShowActor>>()
                .ConvertUsing<ShowToShowActorsConverter>();

            CreateMap<Models.Person, Persistance.Entities.Actor>();
        }
    }

    public class PersonToActorConverter : ITypeConverter<Models.Person, Persistance.Entities.Actor>
    {
        public Persistance.Entities.Actor Convert(Person source, Persistance.Entities.Actor destination, ResolutionContext context)
        {
            return new Persistance.Entities.Actor
            {
                Id = source.Id,
                Name = source.Name,
                Birthday = source.Birthday
            };
        }
    }

    public class ShowToShowActorsConverter : ITypeConverter<Models.Show, IEnumerable<ShowActor>>
    {
        public IEnumerable<ShowActor> Convert(Models.Show source, IEnumerable<ShowActor> destination, ResolutionContext context)
        {
            return source
                .Cast
                .Select(person => new ShowActor
                    {
                        ShowId = source.Id,
                        ActorId = person.Id
                    })
                .ToList();
        }
    }
}