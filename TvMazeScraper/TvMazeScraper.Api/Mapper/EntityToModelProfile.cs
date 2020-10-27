using System.Linq;
using AutoMapper;
using TvMazeScraper.Api.Models;
using TvMazeScraper.Persistance.Entities;

namespace TvMazeScraper.Api.Mapper
{
    public class EntityToModelProfile : Profile
    {
        public EntityToModelProfile()
        {
            CreateMap<Show, ShowVm>()
                .ConvertUsing<ShowToShowVmConverter>();
        }
    }

    public class ShowToShowVmConverter : ITypeConverter<Show, ShowVm>
    {
        public ShowVm Convert(Show source, ShowVm destination, ResolutionContext context)
        {
            return new ShowVm
            {
                Id = source.Id,
                Name = source.Name,
                Actors = source.ShowActors
                    .Select(sa => new ActorVm
                        {
                            Id = sa.Actor.Id,
                            Name = sa.Actor.Name,
                            Birthday = sa.Actor.Birthday
                        })
                    .ToList()
            };
        }
    }
}