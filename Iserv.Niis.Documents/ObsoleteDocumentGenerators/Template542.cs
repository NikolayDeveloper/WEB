using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(131, "UVO-4")]
    public class Template542 : DocumentGeneratorBase
    {
        public Template542(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var data = GetData();
            return new Content(
                BuildField(TemplateFieldName.CurrentDate),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.ExpirationDateOD),
                BuildField(TemplateFieldName.CurrentUser),
                new FieldContent("PatentValidYear", data.PatentValidYear.ToString()),
                new FieldContent("Uvo4Pay", TemplateFieldValueExtensions.ToTemplateDateFormat((DateTimeOffset) data.Uvo4Pay)),
                new FieldContent("Sum", $"{data.Sum:F2}")
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        internal class TemplateData
        {
            internal int PatentValidYear { get; set; }
            internal DateTimeOffset Uvo4Pay { get; set; }
            internal decimal Sum { get; set; }
        }

        private TemplateData GetData()
        {
            var protectionDocId = Convert.ToInt32((object) Parameters["RequestId"]);
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocId));
            var result = new TemplateData();
            var supportYear = DateTimeOffset.Now.Year - protectionDoc.DateCreate.Year + 1;
            result.PatentValidYear = protectionDoc.MaintainDate?.Year ?? DateTimeOffset.Now.Year;
            result.Uvo4Pay = protectionDoc.MaintainDate?.AddMonths(6) ?? DateTimeOffset.Now;
            var tariffs = Executor.GetQuery<GetProtectionDocSupportTariffsBySupportYearAndExpiryQuery>()
                .Process(q => q.Execute(supportYear, true, protectionDoc.TypeId));
            var owner = protectionDoc.ProtectionDocCustomers.FirstOrDefault(pc =>
                    new[] { DicCustomerRoleCodes.Owner, DicCustomerRoleCodes.PatentOwner }.Contains(pc.CustomerRole.Code))
                ?.Customer;
            foreach (var tariff in tariffs)
            {
                for (int i = tariff.ProtectionDocSupportYearsFrom ?? 0; i <= tariff.ProtectionDocSupportYearsUntil; i++)
                {
                    decimal price = tariff.Price ?? 0;
                    //switch (protectionDoc.BeneficiaryType?.Code)
                    //{
                    //    case "SMB":
                    //        price = tariff.PriceBusiness ?? 0;
                    //        break;
                    //    case "VET":
                    //        price = tariff.PriceBeneficiary ?? 0;
                    //        break;
                    //}
                    //if (owner?.Type?.Code != "2")
                    //{
                    //    price = tariff.PriceUl ?? 0;
                    //}
                    //else
                    //{
                    //    price = tariff.PriceFl ?? 0;
                    //}
                    result.Sum = price / 100 * 112;
                }
            }
            return result;
        }
    }
}
