using AutoMapper;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Models;

namespace Iserv.Niis.ExternalServices.Features.Mapping.System
{
    public class StatusInfoDtoProfile : Profile
    {
        public StatusInfoDtoProfile()
        {
            CreateMap<StatusInfoDto, IntegrationSituationCenter.Models.StatusInfo>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.MessageRu, opt => opt.MapFrom(src => src.MessageRu))
                .ForMember(dst => dst.MessageKz, opt => opt.MapFrom(src => src.MessageKz))
                .ReverseMap();

            CreateMap<StatusInfoDto, StatusInfo>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.MessageRu, opt => opt.MapFrom(src => src.MessageRu))
                .ForMember(dst => dst.MessageKz, opt => opt.MapFrom(src => src.MessageKz))
                .ReverseMap();

            CreateMap<StatusInfoDto, IntegrationIntelStatusSender.External.StatusInfo>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.MessageRu, opt => opt.MapFrom(src => src.MessageRu))
                .ForMember(dst => dst.MessageKz, opt => opt.MapFrom(src => src.MessageKz))
                .ReverseMap();

            CreateMap<StatusInfoDto, IntegrationContract.Models.StatusInfo>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.DescriptionRu, opt => opt.MapFrom(src => src.MessageRu))
                .ForMember(dst => dst.DescriptionKz, opt => opt.MapFrom(src => src.MessageKz))
                .ReverseMap();

            CreateMap<IntegrationContract.Models.StatusInfo, StatusInfoDto>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.MessageRu, opt => opt.MapFrom(src => src.DescriptionRu))
                .ForMember(dst => dst.MessageKz, opt => opt.MapFrom(src => src.DescriptionKz))
                .ReverseMap();
        }
    }
}