using System;
using Iserv.Niis.Documents.Enums;

namespace Iserv.Niis.Documents.Attributes
{
    public class TemplateFieldNameAttribute : Attribute
    {
        public TemplateFieldNameAttribute(TemplateFieldName name)
        {
            Name = name;
        }

        public TemplateFieldName Name { get; }
    }
}