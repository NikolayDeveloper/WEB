using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractRequestRelations
{
    public class CreateContractRequestRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public CreateContractRequestRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(int contractId, List<ContractRequestRelationDto> contractRequestRelationDtos)
        {
            var contractRequestRelationRepository = Uow.GetRepository<ContractRequestRelation>();
            var addedContractRequestRelationsDtos = contractRequestRelationDtos.Where(r => r.Id == 0);
            //todo: убрать стрнгу opt.Items["ContractId"]
            var addedContractRequestRelation = _mapper.Map<IEnumerable<ContractRequestRelation>>(addedContractRequestRelationsDtos, opt => opt.Items["ContractId"] = contractId).ToList();
            contractRequestRelationRepository.CreateRange(addedContractRequestRelation);
            Uow.SaveChanges();
        }

        public void Execute(ContractRequestRelation requestContract)
        {
            var repo = Uow.GetRepository<ContractRequestRelation>();

            repo.Create(requestContract);
            Uow.SaveChanges();
        }
    }
}