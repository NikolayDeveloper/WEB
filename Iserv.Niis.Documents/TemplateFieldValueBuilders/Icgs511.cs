using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.Icgs511)]
    internal class Icgs511 : TemplateFieldValueBase
    {
        private const string DescriptionMktuCode = "511";

        public Icgs511(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
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
                                && (r.IsPartialRefused.HasValue == false || r.IsPartialRefused == false)).Select(i => i.Icgs.Code);
                        if (icgs.Any())
                        {
                            return string.Join(", ", icgs);
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
                                && (r.IsPartialRefused.HasValue == false || r.IsPartialRefused == false)).Select(i => i.Icgs.Code);
                        if (icgs.Any())
                        {
                            return string.Join(", ", icgs);
                        }
                    }

                    return "";
                default:
                    return string.Empty;
            }
            
        }
    }
}