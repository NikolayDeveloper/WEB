using AutoMapper;
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
    public class UpdateContractProtectionDocRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public UpdateContractProtectionDocRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(int contractId, List<ContractProtectionDocRelationDto> contractProtectionDocRelationDtos)
        {
            var repository = Uow.GetRepository<ContractProtectionDocRelation>();
            var existRelations = repository.AsQueryable().Where(rr => rr.ContractId == contractId);
            var existRelationIds = existRelations.Select(rr => rr.Id);

            var updatedRequestRelationDtos = contractProtectionDocRelationDtos.Where(r => existRelationIds.Contains(r.Id)).ToList();
            if (updatedRequestRelationDtos.Any() == false)
                return;

            foreach (var relation in existRelations)
            {
                var contractRelationDto = updatedRequestRelationDtos.FirstOrDefault(rr => rr.Id == relation.Id);
                if (contractRelationDto == null)
                    continue;

                //todo: убрать стрингу opt.Items["ContractId"]
                _mapper.Map(contractRelationDto, relation, opt => opt.Items["ContractId"] = contractId);
            }
            repository.UpdateRange(existRelations);
            Uow.SaveChanges();
        }
    }
}
