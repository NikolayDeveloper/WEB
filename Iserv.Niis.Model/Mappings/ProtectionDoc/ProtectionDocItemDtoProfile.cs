using AutoMapper;
using Iserv.Niis.Model.Models.ProtectionDoc;

namespace Iserv.Niis.Model.Mappings.ProtectionDoc
{
    public class ProtectionDocItemDtoProfile: Profile
    {
        public ProtectionDocItemDtoProfile()
        {
            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, ProtectionDocItemDto>()
                .ForMember(dest => dest.ProtectionDocTypeName, opt => opt.MapFrom(src => src.Type.NameRu))
                .ForMember(dest => dest.ProtectionDocTypeCode, opt => opt.MapFrom(src => src.Type.Code))
                .ForMember(dest => dest.ProtectionDocNum, opt => opt.MapFrom(src => src.GosNumber));

            CreateMap<ProtectionDocItemDto, Domain.Entities.ProtectionDoc.ProtectionDoc>();
        }
    }
}