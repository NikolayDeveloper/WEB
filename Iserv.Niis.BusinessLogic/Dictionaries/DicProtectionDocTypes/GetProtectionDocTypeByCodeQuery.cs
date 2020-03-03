using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes
{
    /// <summary>
    /// Запрос получения типа охранного документа по коду охранного документа.
    /// </summary>
    public class GetProtectionDocTypeByCodesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="codes">Массив кодов охранных документов.</param>
        /// <returns>Массив типов охранных докуметов.</returns>
        public List<DicProtectionDocType> Execute(IEnumerable<string> codes)
        {
            var repository = Uow.GetRepository<DicProtectionDocType>();

            return repository.AsQueryable()
                .Where(r => codes.Contains(r.Code))
                .ToList();
        }

        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="codes">Массив кодов охранных документов.</param>
        /// <returns>Массив типов охранных докуметов.</returns>
        public async Task<List<DicProtectionDocType>> ExecuteAsync(IEnumerable<string> codes)
        {
            var repository = Uow.GetRepository<DicProtectionDocType>();

            return await repository.AsQueryable()
                .Where(type => codes.Contains(type.Code))
                .ToListAsync();
        }
    }
}