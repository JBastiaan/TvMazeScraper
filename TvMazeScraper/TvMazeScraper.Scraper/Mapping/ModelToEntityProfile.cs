using AutoMapper;
using TvMazeScraper.Persistance.Entities;
using Actor = TvMazeScraper.Scraper.Models.Actor;

namespace TvMazeScraper.Scraper.Mapping
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<Models.Show, Persistance.Entities.Show>();

            CreateMap<Models.Person, Persistance.Entities.Actor>();

            CreateMap<Models.Actor, ShowActor>()
                .ConvertUsing<ActorToShowActoConverter>();
        }
    }

    public class ActorToShowActoConverter : ITypeConverter<Models.Actor, Persistance.Entities.ShowActor>
    {
        public ShowActor Convert(Actor source, ShowActor destination, ResolutionContext context)
        {
            return new ShowActor
            {
                ActorId = source.Person.Id
            };
        }
    }
}