using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Mappings.BibliographicData
{
    public class ConventionInfoDtoProfile : Profile
    {
        public ConventionInfoDtoProfile()
        {
            CreateMap<RequestConventionInfo, ConventionInfoDto>();
            CreateMap<ConventionInfoDto, RequestConventionInfo>()
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.Ignore());
            CreateMap<ProtectionDocConventionInfo, ConventionInfoDto>();
            CreateMap<ConventionInfoDto, ProtectionDocConventionInfo>()
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.Ignore());
        }
    }
}