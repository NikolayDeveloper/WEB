using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Model.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Iserv.Niis.Model.Mappings
{
    public class CustomerShortInfoDtoProfile : Profile
    {
        public CustomerShortInfoDtoProfile()
        {
            CreateMap<DicCustomer, CustomerShortInfoDto>()
                .ForMember(dest => dest.GovReg, opt => opt.MapFrom(src => GetAttorneyGovReg(src)))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.ContactInfos));
        }
        private static string GetAttorneyGovReg(DicCustomer src)
        {
            return src.CustomerAttorneyInfos.Select(a => a.CertNum).ToArray().Join();
        }
    }
}