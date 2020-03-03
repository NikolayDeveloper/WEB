using AutoMapper;
using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Mappings.Request
{
    public class RequestItemDtoProfile: Profile
    {
        public RequestItemDtoProfile()
        {
            CreateMap<Domain.Entities.Request.Request, RequestItemDto>()
                .ForMember(dest => dest.ProtectionDocTypeName, opt => opt.MapFrom(src=>src.ProtectionDocType.NameRu))
                .ForMember(dest => dest.ProtectionDocTypeCode, opt => opt.MapFrom(src => src.ProtectionDocType.Code));

            CreateMap<RequestItemDto, Domain.Entities.Request.Request>();
        }
    }
}