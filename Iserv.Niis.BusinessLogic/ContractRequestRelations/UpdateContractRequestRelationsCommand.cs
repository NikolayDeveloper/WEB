using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestRelations
{
    public class UpdateContractRequestRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public UpdateContractRequestRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(int contractId, List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestRelationRepository = Uow.GetRepository<ContractRequestRelation>();
            var existRequestRelations = contractRequestRelationRepository
                .AsQueryable()
                .Where(rr => rr.ContractId == contractId);
            var existRequestRelationIds = existRequestRelations.Select(rr => rr.Id);

            var updatedRequestRelationDtos = contractRequestRelationDtos.Where(r => existRequestRelationIds.Contains(r.Id)).ToList();
            if (updatedRequestRelationDtos.Any() == false)
            {
                return;
            }
            
            foreach (var contractRequestRelation in existRequestRelations)
            {
                var contractRequestRelationDto = updatedRequestRelationDtos.FirstOrDefault(rr => rr.Id == contractRequestRelation.Id);
                if (contractRequestRelationDto == null)
                {
                    continue;
                }
                //todo: убрать стрингу opt.Items["ContractId"]
                _mapper.Map(contractRequestRelationDto, contractRequestRelation, opt => opt.Items["ContractId"] = contractId);
            }
            contractRequestRelationRepository.UpdateRange(existRequestRelations);
            Uow.SaveChanges();
        }


        public void Execute(ContractRequestRelation requestContract)
        {
            var repo = Uow.GetRepository<ContractRequestRelation>();

            repo.Update(requestContract);
            Uow.SaveChanges();
        }
    }
}