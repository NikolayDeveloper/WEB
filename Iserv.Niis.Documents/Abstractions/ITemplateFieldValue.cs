using System.Collections.Generic;

namespace Iserv.Niis.Documents.Abstractions
{
    public interface ITemplateFieldValue
    {
        dynamic Get(Dictionary<string, object> parameters);
    }
}