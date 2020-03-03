using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Models;

namespace Iserv.Niis.ExternalServices.Features.Mapping.System
{
    public class LogSystemInfoProfile : Profile
    {
        public LogSystemInfoProfile()
        {
            CreateMap<SystemInfoDto, LogSystemInfo>()
                .ForMember(dst => dst.DbDateTime, opt => opt.Ignore())
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTimeOffset.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dst => dst.StatusCode,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.Code : null))
                .ForMember(dst => dst.StatusMessageRu,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.MessageRu : null))
                .ForMember(dst => dst.StatusMessageKz,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.MessageKz : null));

            CreateMap<IntegrationIntelStatusSender.External.SystemInfo, LogSystemInfo>()
                .ForMember(dst => dst.DbDateTime, opt => opt.Ignore())
                .ForMember(dst => dst.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.ChainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTimeOffset.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dst => dst.StatusCode,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.Code : null))
                .ForMember(dst => dst.StatusMessageRu,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.MessageRu : null))
                .ForMember(dst => dst.StatusMessageKz,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.MessageKz : null));

            CreateMap<IntegrationIntelStatusSender.ExternalPEP.RequestInfo, LogSystemInfo>()
                .ForMember(dst => dst.DbDateTime, opt => opt.Ignore())
                .ForMember(dst => dst.AdditionalInfo, opt => opt.Ignore())
                .ForMember(dst => dst.ChainId, opt => opt.MapFrom(src => src.chainId))
                .ForMember(dst => dst.MessageDate, opt => opt.MapFrom(src => DateTimeOffset.Now))
                .ForMember(dst => dst.MessageId, opt => opt.MapFrom(src => src.messageId))
                .ForMember(dst => dst.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dst => dst.StatusCode,
                    opt => opt.MapFrom(src => src.status != null ? src.status.Code : null))
                .ForMember(dst => dst.StatusMessageRu,
                    opt => opt.MapFrom(src => src.status != null ? src.status.Message : null))
                .ForMember(dst => dst.StatusMessageKz,
                    opt => opt.MapFrom(src => src.status != null ? src.status.MessageKz : null));
        }
    }
}