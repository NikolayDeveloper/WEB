using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.EarlyReg;

namespace Iserv.Niis.Model.Mappings.EarlyReg
{
    public class RequestEarlyRegDtoProfile : Profile
    {
        public RequestEarlyRegDtoProfile()
        {
            CreateMap<RequestEarlyReg, RequestEarlyRegDto>()
                .ForMember(dest => dest.CountryNameRu, opt => opt.MapFrom(src => src.RegCountry.NameRu));
            CreateMap<ProtectionDocEarlyReg, RequestEarlyRegDto>()
                .ForMember(dest => dest.CountryNameRu, opt => opt.MapFrom(src => src.RegCountry.NameRu));
            CreateMap<RequestEarlyRegDto, RequestEarlyReg>()
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.Ignore());
            CreateMap<RequestEarlyRegDto, ProtectionDocEarlyReg>()
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.Ignore());
        }
    }
}