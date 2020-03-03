using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Model.Mappings.Subject
{
    public class SubjectProfile: Profile
    {
        public SubjectProfile()
        {
            CreateMap<RequestCustomer, RequestCustomer>()
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerRole, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            CreateMap<ProtectionDocCustomer, ProtectionDocCustomer>()
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerRole, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore());
        }
    }
}
