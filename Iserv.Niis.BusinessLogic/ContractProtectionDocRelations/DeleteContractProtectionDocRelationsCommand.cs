using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.ContractProtectionDocRelations
{
    public class DeleteContractProtectionDocRelationsCommand : BaseCommand
    {
        public void Execute(int contractId, List<ContractProtectionDocRelationDto> contractProtectionDocRelationDtos)
        {
            var repo = Uow.GetRepository<ContractProtectionDocRelation>();
            var existRelations = repo
                .AsQueryable()
                .Where(rr => rr.ContractId == contractId);

            var contractProtectionDocRelationDtoIds = contractProtectionDocRelationDtos.Select(rd => rd.Id);
            var deletedRelations = existRelations.Where(r => contractProtectionDocRelationDtoIds.Contains(r.Id) == false).ToList();

            repo.DeleteRange(deletedRelations);
            Uow.SaveChanges();
        }
    }
}
