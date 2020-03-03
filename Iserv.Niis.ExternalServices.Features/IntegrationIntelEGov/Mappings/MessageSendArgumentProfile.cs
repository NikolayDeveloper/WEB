using AutoMapper;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings
{
    public class MessageSendArgumentProfile: Profile
    {
        public MessageSendArgumentProfile()
        {
            CreateMap<MessageSendArgument, DicCustomer>()
                .ForMember(dst => dst.NameRu, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dst => dst.TypeId, opt => opt.ResolveUsing((src, dest, member, context) => context.Options.Items[nameof(dest.TypeId)]))
                .ForMember(dst => dst.PhoneFax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Email));
        }
    }
}