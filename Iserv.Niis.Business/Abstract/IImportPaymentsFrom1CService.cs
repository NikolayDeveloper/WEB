using System;
using System.Threading.Tasks;

namespace Iserv.Niis.Business.Abstract
{
    public interface IImportPaymentsFrom1CService
    {
        DateTimeOffset CalculateDateOf1CPaymentsToImport(DateTimeOffset importDate, int workingDaysBeforeImport);
        Task<int> ImportPaymentsAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int? userId = null, string userName = null, string userPosition = null);
    }
}