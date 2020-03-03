using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestRelations
{
    public class DeleteContractRequestRelationsCommand : BaseCommand
    {
        public void Execute(int contractId, List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestRelationRepository = Uow.GetRepository<ContractRequestRelation>();
            var existRequestRelations = contractRequestRelationRepository
                .AsQueryable()
                .Where(rr => rr.ContractId == contractId);

            var contractRequestRelationDtoIds = contractRequestRelationDtos.Select(rd => rd.Id);
            var deletedRequestRelations = existRequestRelations.Where(r => contractRequestRelationDtoIds.Contains(r.Id) == false).ToList();

            contractRequestRelationRepository.DeleteRange(deletedRequestRelations);
            Uow.SaveChanges();
        }
    }
}