using AutoMapper;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Model.Models.Dictionaries;

namespace Iserv.Niis.Model.Mappings.Dictionaries
{
    public class DicRouteStageDtoProfile : Profile
    {
        public DicRouteStageDtoProfile()
        {
            CreateMap<DicRouteStage, DicRouteStageDto>();
        }
    }
}