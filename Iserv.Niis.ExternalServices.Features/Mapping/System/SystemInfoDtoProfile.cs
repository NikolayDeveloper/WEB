using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Models;

namespace Iserv.Niis.ExternalServices.Features.Mapping.System
{
    public class SystemInfoDtoProfile : Profile
    {
        public SystemInfoDtoProfile()
        {
            CreateMap<SystemInfoDto, IntegrationSituationCenter.Models.SystemInfo>()
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ReverseMap();

            CreateMap<SystemInfoDto, SystemInfo>()
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ReverseMap();

            CreateMap<SystemInfoDto, IntegrationIntelStatusSender.External.SystemInfo>()
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ReverseMap();

            CreateMap<SystemInfoDto, IntegrationContract.Models.SystemInfo>()
                .ForPath(dst => dst.SenderInfo.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.StatusInfo, opt => opt.MapFrom(src => src.Status))
                .ForPath(dst => dst.SenderInfo.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForPath(dst => dst.SenderInfo.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForPath(dst => dst.SenderInfo.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForPath(dst => dst.SenderInfo.Sender, opt => opt.MapFrom(src => src.Sender))
                .ReverseMap();

            CreateMap<IntegrationContract.Models.SystemInfo, SystemInfoDto>()
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.SenderInfo.AdditionalInfo))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.StatusInfo))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.SenderInfo.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.SenderInfo.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.SenderInfo.Sender))
                .ReverseMap();
        }
    }
}
