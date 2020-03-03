using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    public interface IImportRequestHelper
    {
        /// <summary>
        /// Импорт всех заявок за датау
        /// </summary>
        /// <param name="date">Дата сбора заявок</param>
        /// <returns>Список id импортированных заявок</returns>
        Task<IList<int>> ImportRequestByDate(DateTime date);

        /// <summary>
        /// Импорт заявки по номеру
        /// </summary>
        /// <param name="number">Входящий Номер или номер заявки</param>
        /// <param name="returnNullIfExist">Вернуть NULL если уже создан</param>
        /// <returns>Id импортированной заявки</returns>
        Task<int?> ImportFromDb(string number, bool returnNullIfExist = false);
    }
}
