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
            CreateMap<Models.ShowDto, Show>()
                .ForMember(
                    show => show.ShowActors, 
                    opt => opt.MapFrom(source => source));

            CreateMap<Models.ShowDto, IEnumerable<ShowActor>>()
                .ConvertUsing<ShowToShowActorsConverter>();

            CreateMap<Models.PersonDto, Persistance.Entities.Actor>();
        }
    }

    public class PersonToActorConverter : ITypeConverter<Models.PersonDto, Persistance.Entities.Actor>
    {
        public Persistance.Entities.Actor Convert(PersonDto source, Persistance.Entities.Actor destination, ResolutionContext context)
        {
            return new Persistance.Entities.Actor
            {
                Id = source.Id,
                Name = source.Name,
                Birthday = source.Birthday
            };
        }
    }

    public class ShowToShowActorsConverter : ITypeConverter<Models.ShowDto, IEnumerable<ShowActor>>
    {
        public IEnumerable<ShowActor> Convert(Models.ShowDto source, IEnumerable<ShowActor> destination, ResolutionContext context)
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