using AutoMapper;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractDetailDtoProfile : Profile
    {
        public ContractDetailDtoProfile()
        {
            CreateMap<Domain.Entities.Contract.Contract, ContractDetailDto>()
                .ForMember(dest => dest.Subjects,
                    opt => opt.UseValue(new SubjectDto[0]))
                .ForMember(
                    dest => dest.InvoiceDtos,
                    opt => opt.UseValue(new PaymentInvoiceDto[0]))
                .ForMember(dest => dest.ProtectionDocTypeCode,
                    opt => opt.MapFrom(src => src.ProtectionDocType != null ? src.ProtectionDocType.Code : null))
                .ForMember(dest => dest.TypeCode,
                    opt => opt.MapFrom(src => src.Type != null ? src.Type.Code : null))
                .ForMember(dest => dest.RequestRelations, opt => opt.MapFrom(src => src.RequestsForProtectionDoc))
                .ForMember(dest => dest.ProtectionDocRelations, opt => opt.MapFrom(src => src.ProtectionDocs))
                .ForMember(dest => dest.AddresseeNameRu, opt => opt.MapFrom(src => src.Addressee.NameRu))
                .ForMember(dest => dest.AddresseeXin, opt => opt.MapFrom(src => src.Addressee.Xin))
                .ForMember(dest => dest.Apartment, opt => opt.MapFrom(src => src.Addressee.Apartment))
                .ForMember(dest => dest.DepartmentNameRu, opt => opt.MapFrom(src => src.Department.NameRu))
                .ForMember(dest => dest.DivisionNameRu, opt => opt.MapFrom(src => src.Division.NameRu))
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId.HasValue))
                .ForMember(dest => dest.Owners, opt => opt.MapFrom(src => src.RequestsForProtectionDoc))
                .ForMember(dest => dest.ProtectionDocsOwners, opt => opt.MapFrom(src => src.ProtectionDocs))
                .ReverseMap();

            CreateMap<ContractDetailDto, Domain.Entities.Contract.Contract>()
                .ForMember(dest => dest.ProtectionDocType, src => src.Ignore())
                .ForMember(dest => dest.Type, src => src.Ignore())
                .ForMember(dest => dest.CurrentWorkflowId, src => src.Ignore())
                .ForMember(dest => dest.CurrentWorkflow, src => src.Ignore())
                .ForMember(dest => dest.Workflows, src => src.Ignore());
        }
    }
}
