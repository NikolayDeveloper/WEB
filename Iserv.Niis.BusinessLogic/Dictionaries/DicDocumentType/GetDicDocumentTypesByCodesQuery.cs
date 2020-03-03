using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    /// <summary>
    /// Получает типа документов по их кодам.
    /// </summary>
    public class GetDicDocumentTypesByCodesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="codes"> Коды типов документов.</param>
        /// <returns> Типы документов.</returns>
        public async Task<List<Domain.Entities.Dictionaries.DicDocumentType>> ExecuteAsync(
            IEnumerable<string> codes)
        {
            var repository = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();

            return await repository
                .AsQueryable()
                .Where(documentType => codes.Contains(documentType.Code))
                .ToListAsync();
        }
    }
}