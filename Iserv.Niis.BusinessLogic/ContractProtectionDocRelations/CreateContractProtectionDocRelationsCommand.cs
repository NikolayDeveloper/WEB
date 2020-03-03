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
    public class CreateContractProtectionDocRelationsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public CreateContractProtectionDocRelationsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void Execute(int contractId, List<ContractProtectionDocRelationDto> contractProtectionDocRelationDtos)
        {
            var repository = Uow.GetRepository<ContractProtectionDocRelation>();
            var addedRelationsDtos = contractProtectionDocRelationDtos.Where(r => r.Id == 0);
            //todo: убрать стрнгу opt.Items["ContractId"]
            var addedRelation = _mapper.Map<IEnumerable<ContractProtectionDocRelation>>(addedRelationsDtos, opt => opt.Items["ContractId"] = contractId).ToList();
            repository.CreateRange(addedRelation);
            Uow.SaveChanges();
        }
    }
}
