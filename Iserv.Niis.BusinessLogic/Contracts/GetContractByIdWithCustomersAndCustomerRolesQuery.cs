using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    /// <summary>
    /// Запрос, возвращающий договор по его идентификатору
    /// и связанных контрагентов с их ролями.
    /// </summary>
    public class GetContractByIdWithCustomersAndCustomerRolesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="contractId">Идентификатор договора.</param>
        /// <returns>Договор.</returns>
        public async Task<Contract> ExecuteAsync(int contractId)
        {
            IRepository<Contract> repository = Uow.GetRepository<Contract>();

            return await repository.AsQueryable()
                .Include(contract => contract.ContractCustomers)
                .ThenInclude(customer => customer.CustomerRole)
                .FirstOrDefaultAsync(contract => contract.Id == contractId);
        }
    }
}
