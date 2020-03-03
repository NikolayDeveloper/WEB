using AutoMapper;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Model.Models.Calendar;

namespace Iserv.Niis.Model.Mappings.Administration.Calendar
{
    public class EventDtoProfile : Profile
    {
        public EventDtoProfile()
        {
            CreateMap<EventDto, Event>()
                .ReverseMap();
        }
    }
}