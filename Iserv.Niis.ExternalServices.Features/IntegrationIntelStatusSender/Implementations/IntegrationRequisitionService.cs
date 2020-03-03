using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations
{
    public class IntegrationRequisitionService : IIntegrationRequisitionService
    {
        private readonly NiisWebContext _niisWebContext;

        public IntegrationRequisitionService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public IntegrationRequisition GetRequisition(int requestBarcode)
        {
            return _niisWebContext.IntegrationRequisitions.FirstOrDefault(x => x.RequestBarcode == requestBarcode);
        }
    }
}