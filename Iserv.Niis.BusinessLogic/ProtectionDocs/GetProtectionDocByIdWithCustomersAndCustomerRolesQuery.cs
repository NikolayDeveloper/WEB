using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    /// <summary>
    /// Запрос, возвращающий охранный документ по его идентификатору 
    /// и связанных контрагентов с их ролями.
    /// </summary>
    public class GetProtectionDocByIdWithCustomersAndCustomerRolesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="protectionDocId">Идентификатор охранного документа.</param>
        /// <returns>Охранный документ.</returns>
        public async Task<ProtectionDoc> ExecuteAsync(int protectionDocId)
        {
            IRepository<ProtectionDoc> repository = Uow.GetRepository<ProtectionDoc>();

            return await repository.AsQueryable()
                .Include(protectionDoc => protectionDoc.ProtectionDocCustomers)
                .ThenInclude(customer => customer.CustomerRole)
                .FirstOrDefaultAsync(protectionDoc => protectionDoc.Id == protectionDocId);
        }
    }
}
