using System.Linq;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetPaySumService
    {
        void GetPaySum(GetPaySumResult result, GetPaySumArgument argument, int countForTariff, int? tariffId);
        int GetCountForTariff(int getPaySumArgumentCount, IQueryable<IntegrationPaymentCalc> paymentCalcs);
        IQueryable<IntegrationPaymentCalc> GetPaymentCalcs(int documentTypeId, int mainDocumentTypeId,
            bool minCountIsNull);
    }
}