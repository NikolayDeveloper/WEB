using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Model.Models.Contract;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractItemDtoProfile: Profile
    {
        public ContractItemDtoProfile()
        {
            CreateMap<Domain.Entities.Contract.Contract, ContractItemDto>()
                .ForMember(dest => dest.StatusNameRu,
                    opt => opt.MapFrom(src => src.StatusId.HasValue ? src.Status.NameRu : string.Empty))
                .ForMember(dest => dest.CurrentStageNameRu,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue ? src.CurrentWorkflow.CurrentStage.NameRu : string.Empty))
                .ForMember(dest => dest.CategoryNameRu,
                    opt => opt.MapFrom(src => src.CategoryId.HasValue ? src.Category.NameRu : string.Empty))
                .ForMember(dest => dest.TypeNameRu,
                    opt => opt.MapFrom(src => src.TypeId.HasValue ? src.Type.NameRu : string.Empty))
                .ForMember(dest => dest.SideOneNameRu,
                    opt => opt.MapFrom(src =>
                        src.ContractCustomers.Any(cc => cc.CustomerRole.Code == DicCustomerRoleCodes.SideOne)
                            ? src.ContractCustomers.First(cc => cc.CustomerRole.Code == DicCustomerRoleCodes.SideOne)
                                .Customer.NameRu
                            : string.Empty))
                .ForMember(dest => dest.SideTwoNameRu,
                    opt => opt.MapFrom(src =>
                        src.ContractCustomers.Any(cc => cc.CustomerRole.Code == DicCustomerRoleCodes.SideTwo)
                            ? src.ContractCustomers.First(cc => cc.CustomerRole.Code == DicCustomerRoleCodes.SideTwo)
                                .Customer.NameRu
                            : string.Empty))
                .ForMember(dest => dest.Initiator,
                    opt => opt.MapFrom(src =>
                        src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst) != null
                            ? src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst).CurrentUser.NameRu
                            : string.Empty))
                .ForMember(dest => dest.Executor,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                        ? src.CurrentWorkflow.CurrentUserId.HasValue
                            ? src.CurrentWorkflow.CurrentUser.NameRu
                            : string.Empty
                        : string.Empty));
        }
    }
}

