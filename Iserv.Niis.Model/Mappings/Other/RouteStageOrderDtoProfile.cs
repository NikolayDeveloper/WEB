using AutoMapper;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Model.Models.Other;

namespace Iserv.Niis.Model.Mappings.Other
{
    public class RouteStageOrderDtoProfile : Profile
    {
        public RouteStageOrderDtoProfile()
        {
            CreateMap<RouteStageOrder, RouteStageOrderDto>();
        }
    }
}