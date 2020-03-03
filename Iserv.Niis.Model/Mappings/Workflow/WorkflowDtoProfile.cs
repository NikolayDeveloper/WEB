using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Mappings.Workflow
{
    public class WorkflowDtoProfile : Profile
    {
        public WorkflowDtoProfile()
        {
            CreateMap<RequestWorkflow, WorkflowDto>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Owner.StatusId))
                .ForMember(dest => dest.ContractGosDate, opt => opt.Ignore())
                .ForMember(dest => dest.FullExpertiseExecutorId, opt => opt.Ignore());
            CreateMap<WorkflowDto, RequestWorkflow>();

            CreateMap<ContractWorkflow, WorkflowDto>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Owner.StatusId))
                .ForMember(dest => dest.ContractGosDate, opt => opt.MapFrom(src => src.Owner.GosDate))
                .ForMember(dest => dest.FullExpertiseExecutorId, opt => opt.MapFrom(src => src.Owner.FullExpertiseExecutorId))
                .ForMember(dest => dest.ContractNum, opt => opt.MapFrom(src => src.Owner.ContractNum))
                .ForMember(dest => dest.ApplicationDateCreate, opt => opt.MapFrom(src => src.Owner.ApplicationDateCreate));
            CreateMap<WorkflowDto, ContractWorkflow>();

            CreateMap<ProtectionDocWorkflow, WorkflowDto>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Owner.StatusId))
                .ForMember(dest => dest.ContractGosDate, opt => opt.Ignore())
                .ForMember(dest => dest.FullExpertiseExecutorId, opt => opt.Ignore());
            CreateMap<WorkflowDto, ProtectionDocWorkflow>();
        }
    }
}