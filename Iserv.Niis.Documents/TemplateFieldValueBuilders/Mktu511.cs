using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.Mktu511)]
    internal class Mktu511 : TemplateFieldValueBase
    {
        public Mktu511(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {


            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    if (request.ICGSRequests != null && request.ICGSRequests.Any())
                    {
                        var icgs = request.ICGSRequests.Where(r => r.Icgs != null
                                && (r.IsRefused.HasValue == false || r.IsRefused == false)
                                && (r.IsPartialRefused.HasValue == false || r.IsPartialRefused == false)).Select(i => i.Icgs.Code + " класса - " + i.ClaimedDescription);
                        if (icgs.Any())
                        {
                            return string.Join(Environment.NewLine, icgs);
                        }
                    }

                    return "";
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    if (protectionDoc.IcgsProtectionDocs != null && protectionDoc.IcgsProtectionDocs.Any())
                    {
                        var icgs = protectionDoc.IcgsProtectionDocs.Where(r => r.Icgs != null
                                && (r.IsRefused.HasValue == false || r.IsRefused == false)
                                && (r.IsPartialRefused.HasValue == false || r.IsPartialRefused == false)).Select(i => i.Icgs.Code + " класса - " + i.ClaimedDescription);
                        if (icgs.Any())
                        {
                            return string.Join(Environment.NewLine, icgs);
                        }
                    }

                    return "";
                default:
                    return string.Empty;
            }
        }
    }
}