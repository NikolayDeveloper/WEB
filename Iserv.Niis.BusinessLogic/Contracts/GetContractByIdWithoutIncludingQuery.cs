using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractByIdWithoutIncludingQuery : BaseQuery
    {
        public Contract Execute(int contractId)
        {
            var contractRepository = Uow.GetRepository<Contract>();

            var contract = contractRepository
                .AsQueryable()
                .FirstOrDefault(c => c.Id == contractId);

            return contract;
        }
    }
}