using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата уведомления положительного результата формальной экспертизы
    /// </summary>
    [TemplateFieldName(TemplateFieldName.DateOfNotificationOfThePositiveResultOfTheFormalExamination)]
    internal class DateOfNotificationOfThePositiveResultOfTheFormalExamination : TemplateFieldValueBase
    {
        public DateOfNotificationOfThePositiveResultOfTheFormalExamination(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Дата уведомления положительного результата формальной экспертизы
            return string.Empty;
        }
    }
}