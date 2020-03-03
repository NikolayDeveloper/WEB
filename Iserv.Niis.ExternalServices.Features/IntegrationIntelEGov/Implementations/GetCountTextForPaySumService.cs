using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetCountTextForPaySumService : IGetCountTextForPaySumService
    {
        private readonly NiisWebContext _niisWebContext;

        public GetCountTextForPaySumService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public string GetCountTextForPaySumResult(
            int documentTypeId, 
            int mainDocumentTypeId)
        {
             return _niisWebContext.IntegrationPaymentCalcs
                .FirstOrDefault(p => 
                    p.CorId == documentTypeId
                    && p.PatentType == mainDocumentTypeId)?
                .CountName;
        }
    }
}