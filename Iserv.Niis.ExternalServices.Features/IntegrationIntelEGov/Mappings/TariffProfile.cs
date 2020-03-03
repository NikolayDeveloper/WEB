using AutoMapper;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings
{
    public class TariffProfile : Profile
    {
        public TariffProfile()
        {
            //TODO Изменилась связь тарифа и типа объекта, один ко многим
            CreateMap<Niis.Domain.Entities.Dictionaries.DicTariff, Tariff>()
                //.ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => src.ProtectionDocTypeId))
                //.ForMember(dst => dst.TypeDescription, opt => opt.MapFrom(src => src.ProtectionDocType.NameRu))
                .ForMember(dst => dst.Descript, opt => opt.MapFrom(src => src.Description));
        }
    }
}
