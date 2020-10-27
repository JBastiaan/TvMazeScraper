using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TvMazeScraper.Persistance.Entities;
using TvMazeScraper.Scraper.Dto;
using Show = TvMazeScraper.Persistance.Entities.Show;

namespace TvMazeScraper.Scraper.Mapping
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<ShowDto, Show>()
                .ForMember(
                    show => show.ShowActors, 
                    opt => opt.MapFrom(source => source));

            CreateMap<ShowDto, IEnumerable<ShowActor>>()
                .ConvertUsing<ShowToShowActorsConverter>();

            CreateMap<PersonDto, Actor>();
        }
    }

    public class PersonToActorConverter : ITypeConverter<PersonDto, Actor>
    {
        public Actor Convert(PersonDto source, Actor destination, ResolutionContext context)
        {
            return new Actor
            {
                Id = source.Id,
                Name = source.Name,
                Birthday = source.Birthday
            };
        }
    }

    public class ShowToShowActorsConverter : ITypeConverter<ShowDto, IEnumerable<ShowActor>>
    {
        public IEnumerable<ShowActor> Convert(ShowDto source, IEnumerable<ShowActor> destination, ResolutionContext context)
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