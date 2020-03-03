using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestICGSRequestRelations
{
    public class UpdateContractRequestIcgsRequestRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public UpdateContractRequestIcgsRequestRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestIcgsRequestRelationRepository = Uow.GetRepository<ContractRequestICGSRequestRelation>();

            var contractRequestRelationsIds = contractRequestRelationDtos.Select(rd => rd.Id).ToList();

            var contractRequestIcgsRequestRelations = contractRequestIcgsRequestRelationRepository
                .AsQueryable()
                .Where(rr => contractRequestRelationsIds.Contains(rr.ContractRequestRelationId))
                .ToList();

            var existIcgsRelationIds = contractRequestIcgsRequestRelations
                .Select(rr => rr.Id)
                .ToList();

            foreach (var contractRequestRelationDto in contractRequestRelationDtos)
            {
                var icgsRelationDtos = contractRequestRelationDto.ICGSRequestRelations;
                if (icgsRelationDtos == null)
                {
                    continue;
                }
                var updatedIcgsRelationsDtos = icgsRelationDtos
                    .Where(r => existIcgsRelationIds.Contains(r.Id));
                var updatedIcgsRelations = _mapper
                    .Map<IEnumerable<ContractRequestICGSRequestRelation>>(updatedIcgsRelationsDtos).ToArray();
                if (updatedIcgsRelations.Any() == false)
                {
                    continue;
                }

                foreach (var updatedIcgsRelation in updatedIcgsRelations)
                {
                    var originIcgsRelation = contractRequestIcgsRequestRelations.FirstOrDefault(x => x.Id == updatedIcgsRelation.Id);
                    if (originIcgsRelation == null)
                    {
                        continue;
                    }

                    originIcgsRelation.ContractRequestRelationId = updatedIcgsRelation.ContractRequestRelationId;
                    originIcgsRelation.Description = updatedIcgsRelation.Description;
                    originIcgsRelation.ICGSRequestId = updatedIcgsRelation.ICGSRequestId;
                }
            }

            Uow.SaveChanges();
        }
    }
}