using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    public class IntegrationEgovPayHelper
    {
        private readonly NiisWebContext _niisContext;
        private readonly IMapper _mapper;

        public IntegrationEgovPayHelper(NiisWebContext niisContext, IMapper mapper)
        {
            _niisContext = niisContext;
            _mapper = mapper;
        }

        public void CreatePay(EGovPay pay)
        {
            if (pay == null) return;
            var integrationEGovPay = _mapper.Map<EGovPay, IntegrationEGovPay>(pay);
            _niisContext.IntegrationEGovPays.Add(integrationEGovPay);
            _niisContext.SaveChanges();
        }
    }
}