using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    //[TemplateFieldName(TemplateFieldName.ChangedListOfDeclaredGoods)]
    //internal class ChangedListOfDeclaredGoods : TemplateFieldValueBase
    //{
    //    public ChangedListOfDeclaredGoods(IExecutor executor) : base(executor)
    //    {
    //    }

    //    protected override IEnumerable<string> RequiredParameters()
    //    {
    //        return new[] { "RequestId" };
    //    }

    //    protected override dynamic GetInternal(Dictionary<string, object> parameters)
    //    {
    //        var request = Executor.GetQuery<GetRequestByIdQuery>()
    //             .Process(q => q.Execute((int)parameters["RequestId"]));

    //        return request?.NameRu ?? string.Empty;
    //    }
    //}
}
