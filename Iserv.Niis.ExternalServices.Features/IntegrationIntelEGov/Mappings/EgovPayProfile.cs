using System;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings
{
    public class EgovPayProfile: Profile
    {
        public EgovPayProfile()
        {
            CreateMap<EGovPay, IntegrationEGovPay>()
                .ForMember(dst => dst.PayCode, opt => opt.MapFrom(src => src.PayCode))
                //.ForMember(dst => dst.PayDate, opt => opt.MapFrom(src => new DateTimeOffset(src.Date)))
                .ForMember(dst => dst.PayDate, opt => opt.MapFrom(src => src.Date.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime || src.Date.ToUniversalTime() >= DateTimeOffset.MaxValue.UtcDateTime
                   ? DateTimeOffset.MinValue
                   : new DateTimeOffset(src.Date)))
                .ForMember(dst => dst.PaySum, opt => opt.MapFrom(src => src.Sum))
                .ForMember(dst => dst.PayXin, opt => opt.MapFrom(src => src.XIN))
                .ForMember(dst => dst.PayXml, opt => opt.MapFrom(src => src.XML));
        }

    }
}