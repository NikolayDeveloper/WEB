using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Model.Models.Contract;

namespace Iserv.Niis.Business.Abstract
{
    public interface IContractRequestRelationUpdater
    {
        Task UpdateAsync(int contractId, ICollection<ContractRequestRelationDto> relationDtos);
    }
}