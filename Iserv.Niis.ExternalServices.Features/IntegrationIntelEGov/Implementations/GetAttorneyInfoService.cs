using System;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetAttorneyInfoService : IGetAttorneyInfoService
    {
        private readonly NiisWebContext _niisContext;

        public GetAttorneyInfoService(NiisWebContext niisContext)
        {
            _niisContext = niisContext;
        }

        public void GetAttorneyInfo(GetAttorneyInfoArgument argument, GetAttorneyInfoResult result)
        {
            var attorney = _niisContext.CustomerAttorneyInfos
                .Include(x => x.Country)
                .Include(x => x.Location)
                .FirstOrDefault(x => argument.IIN.Equals(x.Iin, StringComparison.CurrentCultureIgnoreCase));
            if (attorney == null)
                throw new Exception($"{argument.IIN} - по данному ИИН информация не найдена");
            result.Active = attorney.Active.GetValueOrDefault(false); 
            result.Address = attorney.Address;
            result.CertDate = attorney.CertDate.Date;
            result.CertNum = attorney.CertNum;
            result.Email = attorney.Email;
            result.Fax = attorney.Fax;
            result.IIN = attorney.Iin;
            result.NameFirst = attorney.NameFirst;
            result.NameLast = attorney.NameLast;
            result.NameMiddle = attorney.NameMiddle;
            result.Ops = attorney.Ops;
            result.Phone = attorney.Phone;
            result.RevalidNote = attorney.RevalidNote;
            if (attorney.Country.ExternalId.HasValue)
                result.CountryId = new RefKey {UID = attorney.Country.ExternalId.Value, Note = attorney.Country.NameRu};
            if (attorney.Location.ExternalId.HasValue)
                result.LocationId = new RefKey {UID = attorney.Location.ExternalId.Value, Note = attorney.Location.NameRu};
        }
    }
}