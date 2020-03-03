
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestICGSRequestRelations
{
    public class CreateContractRequestIcgsRequestRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public CreateContractRequestIcgsRequestRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestIcgsRequestRelationRepository = Uow.GetRepository<ContractRequestICGSRequestRelation>();

            foreach (var contractRequestRelationDto in contractRequestRelationDtos)
            {
                var icgsRelationDtos = contractRequestRelationDto.ICGSRequestRelations;
                if (icgsRelationDtos == null)
                {
                    continue;
                }
                var addedIcgsRelationDtos = icgsRelationDtos.Where(r => r.Id == 0);
                var addedIcgsRelations = _mapper.Map<IEnumerable<ContractRequestICGSRequestRelation>>(addedIcgsRelationDtos);
                contractRequestIcgsRequestRelationRepository.CreateRange(addedIcgsRelations);
            }

            Uow.SaveChanges();
        }
    }
}