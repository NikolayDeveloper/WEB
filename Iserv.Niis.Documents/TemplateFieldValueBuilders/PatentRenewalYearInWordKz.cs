﻿using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Год продления
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentRenewalYearInWordKz)]
    internal class PatentRenewalYearInWordKz : TemplateFieldValueBase
    {
        public PatentRenewalYearInWordKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Год продления
            return string.Empty;
        }
    }
}