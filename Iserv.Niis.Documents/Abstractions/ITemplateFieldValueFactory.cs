using System.Collections.Generic;
using Iserv.Niis.Documents.Enums;

namespace Iserv.Niis.Documents.Abstractions
{
    public interface ITemplateFieldValueFactory
    {
        ITemplateFieldValue Create(TemplateFieldName templateFieldName);
        IEnumerable<ITemplateFieldValue> CreateSome(params TemplateFieldName[] templateFieldNames);
    }
}