using System;
using AutoMapper;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings
{
    public class RequisitionSendArgumentProfile: Profile
    {
        public RequisitionSendArgumentProfile()
        {
            CreateMap<RequisitionSendArgument, DicCustomer>()
                .ForMember(dst => dst.NameRu, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dst => dst.TypeId, opt => opt.ResolveUsing((src, dest, member, context) => context.Options.Items[nameof(dest.TypeId)]))
                .ForMember(dst => dst.PhoneFax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dst => dst.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dst => dst.Xin, opt => opt.MapFrom(src => src.Xin))
                .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Email));
            CreateMap<RequisitionSendArgument, Request>()
                .ForMember(dst=>dst.NameRu, opt => opt.MapFrom(src=> string.IsNullOrEmpty(src.NameRu) ? src.Breed : src.NameRu))
                .ForMember(dst => dst.NameKz, opt => opt.MapFrom(src => src.NameKz))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.PageCount, opt => opt.MapFrom(src => src.PageCount))
                .ForMember(dst => dst.CopyCount, opt => opt.MapFrom(src => src.CopyCount))
                .ForMember(dst => dst.DateCreate, opt => opt.UseValue(DateTimeOffset.Now));
            CreateMap<RequisitionSendArgument, RequestInfo>()
                .ForMember(dst => dst.BreedCountry, opt=>opt.Ignore())
                .ForMember(dst => dst.FlagTtw, opt => opt.MapFrom(src => src.LawTTWp2s10))
                .ForMember(dst => dst.FlagNine, opt => opt.MapFrom(src => src.RequirementsLaw))
                .ForMember(dst => dst.FlagTat, opt => opt.MapFrom(src => src.AssignmentAuthorTAT))
                .ForMember(dst => dst.FlagTpt, opt => opt.MapFrom(src => src.AssignmentTPT))
                .ForMember(dst => dst.FlagHeirship, opt => opt.MapFrom(src => src.HeirshipTN))
                .ForMember(dst => dst.IzCollectiveTZ, opt => opt.MapFrom(src => src.IsCollectiveTradeMark))
                .ForMember(dst => dst.Transliteration, opt => opt.MapFrom(src => src.Transliteration))
                .ForMember(dst => dst.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dst => dst.BreedingNumber, opt => opt.MapFrom(src => src.BreedingNumber))
                .ForMember(dst => dst.Breed, opt => opt.MapFrom(src => src.Breed))
                .ForMember(dst => dst.Genus, opt => opt.MapFrom(src => src.Genus))
                .ForMember(dst => dst.ProductSpecialProp, opt => opt.MapFrom(src => src.ProductSpecialProp))
                .ForMember(dst => dst.ProductPlace, opt => opt.MapFrom(src => src.ProductPalce))
                .ForMember(dst => dst.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dst => dst.BreedCountryId, opt => opt.Ignore())
                .ForMember(dst => dst.DateCreate, opt => opt.UseValue(DateTimeOffset.Now));  
        }
    }
}