using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;

namespace Iserv.Niis.Business.Implementations
{
    public class ContractRequestRelationUpdater: IContractRequestRelationUpdater
    {
        private readonly NiisWebContext _context;
        private readonly IMapper _mapper;

        public ContractRequestRelationUpdater(NiisWebContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int contractId, ICollection<ContractRequestRelationDto> relationDtos)
        {
            var existRequestRelations = _context.ContractRequestRelations
                .Where(rr => rr.ContractId == contractId);
            var existRequestRelationIds = existRequestRelations.Select(rr => rr.Id);

            var relationForDelete = existRequestRelations.Where(r => !relationDtos.Select(rd => rd.Id).Contains(r.Id));
            _context.ContractRequestRelations.RemoveRange(relationForDelete);
            await _context.SaveChangesAsync();

            var relationForAdd = relationDtos.Where(r => r.Id == 0);
            _context.ContractRequestRelations.AddRange(_mapper.Map<IEnumerable<ContractRequestRelation>>(relationForAdd, opt => opt.Items["ContractId"] = contractId));
            await _context.SaveChangesAsync();

            var relationForUpdate = relationDtos.Where(r => existRequestRelationIds.Contains(r.Id));
            var item = _mapper.Map<IEnumerable<ContractRequestRelation>>(relationForUpdate,
                opt => opt.Items["ContractId"] = contractId);
            _context.ContractRequestRelations.UpdateRange(item);
            await _context.SaveChangesAsync();

            await UpdateICGSRequestRelation(relationForUpdate);
        }

        private async Task UpdateICGSRequestRelation(IEnumerable<ContractRequestRelationDto> requestRelationDtos)
        {
            var relationForUpdateIds = requestRelationDtos.Select(rd => rd.Id).ToList();
            var existIcgsRelations = _context.ContractRequestICGSRequestRelations
                .Where(rr => relationForUpdateIds.Contains(rr.ContractRequestRelationId));

            var allIcgsRelationDtos = requestRelationDtos.SelectMany(rd => rd.ICGSRequestRelations).ToList();

            var icgsRelationForDelete = existIcgsRelations.Where(r => !allIcgsRelationDtos.Select(rd => rd.Id).Contains(r.Id));
            _context.ContractRequestICGSRequestRelations.RemoveRange(icgsRelationForDelete);
            await _context.SaveChangesAsync();

            foreach (var requestRelationDto in requestRelationDtos)
            {
                var icgsRelationDtos = requestRelationDto.ICGSRequestRelations;

                var icgsRelationForAdd = icgsRelationDtos.Where(r => r.Id == 0);
                _context.ContractRequestICGSRequestRelations.AddRange(_mapper.Map<IEnumerable<ContractRequestICGSRequestRelation>>(icgsRelationForAdd));
                await _context.SaveChangesAsync();

                var icgsRelationForUpdate = icgsRelationDtos.Where(r => existIcgsRelations.Select(rr => rr.Id).Contains(r.Id));
                _context.ContractRequestICGSRequestRelations.UpdateRange(_mapper.Map<IEnumerable<ContractRequestICGSRequestRelation>>(icgsRelationForUpdate));
                await _context.SaveChangesAsync();
            }
        }
    }
}