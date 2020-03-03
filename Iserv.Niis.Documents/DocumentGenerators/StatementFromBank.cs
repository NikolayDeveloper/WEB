using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    /// <summary>
    /// Генератор документа для типа документа "Выписка из банка". Код: <see cref="DicDocumentTypeCodes.StatementFromBank"/>.
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.StatementFromBank)]
    public class StatementFromBank : DocumentGeneratorBase
    {
        public StatementFromBank(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.ServiceName),
                BuildField(TemplateFieldName.ServiceCode),
                BuildField(TemplateFieldName.OwnerNumber),
                BuildField(TemplateFieldName.PaymentNumber),
                BuildField(TemplateFieldName.PaymentId),
                BuildField(TemplateFieldName.PaymentUseDateCreate),
                BuildField(TemplateFieldName.PaymentUseDate),
                BuildField(TemplateFieldName.Payer),
                BuildField(TemplateFieldName.PayerXin),
                BuildField(TemplateFieldName.PaymentDate),
                BuildField(TemplateFieldName.Payment1CNumber),
                BuildField(TemplateFieldName.ChargeAmount),
                BuildField(TemplateFieldName.PaymentAmount),
                BuildField(TemplateFieldName.PaymentDisturbed),
                BuildField(TemplateFieldName.PaymentRemainder),
                BuildField(TemplateFieldName.IsAdvancePayment),
                BuildField(TemplateFieldName.PaymentPurpose),
                BuildField(TemplateFieldName.ChargerUser)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"PaymentUse", "Payment"};
        }
    }
}
