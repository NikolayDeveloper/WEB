using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Implementations
{
    public class RfProtectionDocService : IRfProtectionDocService
    {
        private readonly NiisWebContext _niisWebContext;

        public RfProtectionDocService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public void GetRfProtectionDocs(GetRfPatentListArgument argument, GetRfPatentListResult result)
        {
            if (!argument.DateBegin.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateBegin));
            }
            if (!argument.DateEnd.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateEnd));
            }
            var rfPatents = new List<RfPatent>();
            var rfProtectionDocs = _niisWebContext.ProtectionDocParents
                .Where(x => x.DateCreate.Date >= argument.DateBegin.Value.Date &&
                            x.DateCreate.Date <= argument.DateEnd.Value.Date)
                .Select(x => new
                {
                    ParentBarcode = x.Parent.Barcode,
                    ChildBarcode = x.Child.Barcode
                });
            foreach (var rfProtectionDoc in rfProtectionDocs)
            {
                rfPatents.Add(new RfPatent
                {
                    ChildPatentUID = rfProtectionDoc.ChildBarcode,
                    ParentPatentUID = rfProtectionDoc.ParentBarcode
                });
                result.List = rfPatents.ToArray();
            }
        }
    }
}