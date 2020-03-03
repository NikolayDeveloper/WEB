using AutoMapper;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractRelationResolver : IValueResolver<Domain.Entities.Contract.Contract, ContractDetailDto, MaterialOwnerDto[]>
    {
        private readonly IMapper _mapper;

        public ContractRelationResolver(IMapper mapper)
        {
            _mapper = mapper;
        }

        public MaterialOwnerDto[] Resolve(Domain.Entities.Contract.Contract contract, ContractDetailDto contractDetailDto, MaterialOwnerDto[] destMember, ResolutionContext context)
        {
            var result = new List<MaterialOwnerDto>();

            result.AddRange(_mapper.Map<ICollection<ContractRequestRelation>, ICollection<MaterialOwnerDto>>(contract.RequestsForProtectionDoc));
            result.AddRange(_mapper.Map<ICollection<ContractProtectionDocRelation>, ICollection<MaterialOwnerDto>>(contract.ProtectionDocs));

            return result.ToArray();
        }
    }
}
