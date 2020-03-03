using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Iserv.Niis.Model.Mappings.Request
{
    public class RequestProfile: Profile
    {
        public RequestProfile()
        {
            CreateMap<Domain.Entities.Request.Request, Domain.Entities.Request.Request>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentInvoices, opt => opt.Ignore())
                .ForMember(dest => dest.RequestConventionInfos, opt => opt.Ignore())
                .ForMember(dest => dest.ICGSRequests, opt => opt.Ignore())
                .ForMember(dest => dest.IPCRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ICISRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ColorTzs, opt => opt.Ignore())
                .ForMember(dest => dest.EarlyRegs, opt => opt.Ignore())
                .ForMember(dest => dest.Icfems, opt => opt.Ignore())
                .ForMember(dest => dest.Contracts, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflow, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflowId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestCustomers, opt => opt.Ignore())
                .ForMember(dest => dest.Addressee, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocType, opt => opt.Ignore())
                .ForMember(dest => dest.AdditionalDocs, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicantType, opt => opt.Ignore())
                .ForMember(dest => dest.ConventionType, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Division, opt => opt.Ignore())
                .ForMember(dest => dest.ExpertSearchSimilarities, opt => opt.Ignore())
                .ForMember(dest => dest.FlDivision, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.NotificationStatuses, opt => opt.Ignore())
                .ForMember(dest => dest.RequestInfo, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiveType, opt => opt.Ignore())
                .ForMember(dest => dest.RequestType, opt => opt.Ignore())
                .ForMember(dest => dest.RequestNum, opt => opt.Ignore())
                .ForMember(dest => dest.RequestProtectionDocSimilarities, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.SpeciesTradeMark, opt => opt.Ignore())
                .ForMember(dest => dest.TypeTrademark, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore())
                .ForMember(dest => dest.Workflows, opt => opt.Ignore());
        }
    }
}
