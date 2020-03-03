using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Model.Models.ProtectionDoc;

namespace Iserv.Niis.Model.Mappings.ProtectionDoc
{
    public class ICGSProtectionDocDtoProfile: Profile
    {
        public ICGSProtectionDocDtoProfile()
        {
            CreateMap<ICGSProtectionDoc, ICGSProtectionDocItemDto>()
                .ForMember(dest => dest.IcgsNameRu, opt => opt.MapFrom(src => src.Icgs.NameRu))
                .ForMember(dest => dest.IcgsNameKz, opt => opt.MapFrom(src => src.Icgs.NameKz))
                .ForMember(dest => dest.IcgsNameEn, opt => opt.MapFrom(src => src.Icgs.NameEn));

            CreateMap<ICGSProtectionDocItemDto, ICGSProtectionDoc>()
                .ForMember(dest => dest.Icgs, src => src.Ignore());
        }
    }
}