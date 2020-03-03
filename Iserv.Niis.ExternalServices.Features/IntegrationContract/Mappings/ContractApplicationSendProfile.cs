using System;
using AutoMapper;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Mappings
{
    public class ContractApplicationSendProfile : Profile
    {
        public ContractApplicationSendProfile()
        {
            CreateMap<ContractRequest, DicCustomer>()
                .ForMember(dst => dst.NameRu, opt => opt.MapFrom(src => src.Addressee.CustomerName))
                .ForMember(dst => dst.TypeId,
                    opt => opt.ResolveUsing((src, dest, member, context) => context.Options.Items[nameof(dest.TypeId)]))
                .ForMember(dst => dst.PhoneFax, opt => opt.MapFrom(src => src.Addressee.Fax))
                .ForMember(dst => dst.Login, opt => opt.MapFrom(src => src.Addressee.Login))
                .ForMember(dst => dst.Street, opt => opt.MapFrom(src => src.Addressee.Street))
                .ForMember(dst => dst.Xin, opt => opt.MapFrom(src => src.Addressee.Xin))
                .ForMember(dst => dst.Region, opt => opt.MapFrom(src => src.Addressee.Region))
                .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.Addressee.Phone));

            CreateMap<ContractRequest, Contract>()
                .ForMember(dst => dst.NameRu, opt => opt.MapFrom(src => src.DocumentDescriptionRu))
                .ForMember(dst => dst.NameKz, opt => opt.MapFrom(src => src.DocumentDescriptionKz))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.DocumentDescriptionEn))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.DocumentDescriptionRu))
                .ForMember(dst => dst.DateCreate, opt => opt.UseValue(DateTimeOffset.Now))
                .ForMember(dst => dst.Addressee,opt => opt.Ignore());
        }
    }
}
