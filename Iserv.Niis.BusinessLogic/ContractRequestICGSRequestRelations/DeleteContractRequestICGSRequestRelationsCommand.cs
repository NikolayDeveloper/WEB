using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestICGSRequestRelations
{
    public class DeleteContractRequestIcgsRequestRelationsCommand : BaseCommand
    {
        public void Execute(List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestIcgsRequestRelationRepository = Uow.GetRepository<ContractRequestICGSRequestRelation>();

            var updatedContractRequestRelationsDtos = contractRequestRelationDtos.Select(rd => rd.Id).ToList();
            var icgsRelationDtoIds = contractRequestRelationDtos.Where(rd => rd.ICGSRequestRelations != null).SelectMany(rd => rd.ICGSRequestRelations).Select(rd => rd.Id).ToList();

            var deletedIcgsRelations = contractRequestIcgsRequestRelationRepository
                .AsQueryable()
                .Where(rr => icgsRelationDtoIds.Contains(rr.Id) == false
                             && updatedContractRequestRelationsDtos.Contains(rr.ContractRequestRelationId));

            contractRequestIcgsRequestRelationRepository.DeleteRange(deletedIcgsRelations);
            Uow.SaveChanges();
        }
    }
}