using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(132, "UVO-5")]
    public class Template543 : DocumentGeneratorBase
    {
        public Template543(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var data = GetData();
            return new Content(
                BuildField(TemplateFieldName.PatentGosNumber),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.DocumentNum),
                BuildField(TemplateFieldName.DateTimeNow),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.BulletinDate),
                BuildField(TemplateFieldName.EarlyTerminationDate),
                BuildField(TemplateFieldName.ApplicantAddress),
                new TableContent("SumRu", Enumerable.Select<DicTariff, TableRowContent>(data.SupportTariffs, t => new TableRowContent(new FieldContent("TariffInfoRu",
                        $"{data.GetSupportYearsStringRu(t)} - {data.GetSupportTariffSumString(t)}")))
                ),
                new FieldContent("TotalSumRu", $"Итого: {data.GetSupportTotalSum():F2} тенге"),
                new TableContent("SumKz", Enumerable.Select<DicTariff, TableRowContent>(data.SupportTariffs, t => new TableRowContent(new FieldContent("TariffInfoKz",
                        $"{data.GetSupportYearsStringKz(t)} - {data.GetSupportTariffSumString(t)}")))
                ),
                new FieldContent("TotalSumKz", $"Жиыны: {data.GetSupportTotalSum():F2} теңге"),
                new TableContent("TariffTable", Enumerable.Select<DicTariff, TableRowContent>(data.SupportTariffs, t => new TableRowContent(
                        BuildField(TemplateFieldName.GosNumber),
                        new FieldContent("TableName", data.GetSupportYearsStringRu(t)),
                        new FieldContent("TariffCount", $"{data.GetTariffYearsAndPrice(t).YearsCount}"),
                        new FieldContent("TariffSum", $"{data.GetTariffYearsAndPrice(t).Price:F2}"),
                        new FieldContent("TotalSum",
                            $"{data.GetTariffYearsAndPrice(t).YearsCount * data.GetTariffYearsAndPrice(t).Price:F2}")
                    )
                )),
                new FieldContent("CompleteSum", $"{data.GetSupportTotalSum():F2} тг"),
                new FieldContent("NDS", $"{data.GetSupportTotalSum() / 112 * 12:F2} тг")
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", "UserId"};
        }

        private NotificationData GetData()
        {
            var result = new NotificationData();

            var protectionDocId = Convert.ToInt32((object) Parameters["RequestId"]);
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocId));
            result.BeneficiaryType = protectionDoc.BeneficiaryType;
            var owner = protectionDoc.ProtectionDocCustomers.FirstOrDefault(pc =>
                    new[] {DicCustomerRoleCodes.Owner, DicCustomerRoleCodes.PatentOwner}.Contains(pc.CustomerRole.Code))
                ?.Customer;
            result.CustomerType = owner?.Type;
            result.SupportYear = (DateTimeOffset.Now.Year - protectionDoc.DateCreate.Year) + 1;
            var tariffs = Executor.GetQuery<GetProtectionDocSupportTariffsBySupportYearAndExpiryQuery>()
                .Process(q => q.Execute(result.SupportYear, false, protectionDoc.TypeId));
            result.SupportTariffs = tariffs;

            if(Enumerable.Any<DicTariff>(result.SupportTariffs) == false)
            {
                result.SupportTariffs.Add(new DicTariff());
            }

            return result;
        }

        internal class NotificationData
        {
            public int SupportYear { get; set; }
            public List<DicTariff> SupportTariffs { get; set; }
            public DicBeneficiaryType BeneficiaryType { get; set; }
            public DicCustomerType CustomerType { get; set; }

            public string GetSupportYearsStringRu(DicTariff tariff)
            {
                var years = new List<int>();
                for (int i = 0; i < tariff.ProtectionDocSupportYearsUntil; i++)
                {
                    if (i + 1 <= SupportYear)
                    {
                        years.Add(i+1);
                    }
                }
                return $"за {string.Join(", ", years)} годы";
            }

            public string GetSupportYearsStringKz(DicTariff tariff)
            {
                var years = new List<int>();
                for (int i = 0; i < tariff.ProtectionDocSupportYearsUntil; i++)
                {
                    if (i + 1 <= SupportYear)
                    {
                        years.Add(i + 1);
                    }
                }
                return $"{string.Join(", ", years)} жылдарына";
            }

            public string GetSupportTariffSumString(DicTariff tariff)
            {
                var yearsAndPrice = GetTariffYearsAndPrice(tariff);
                return $"{yearsAndPrice.Price:F2} тенге x {yearsAndPrice.YearsCount} = {yearsAndPrice.Price * yearsAndPrice.YearsCount:F2} тг";
            }

            public decimal GetSupportTotalSum()
            {
                decimal result = 0;
                foreach (var tariff in SupportTariffs)
                {
                    result += GetTariffYearsAndPrice(tariff).Price;
                }
                return result;
            }

            public TariffCountAndPrice GetTariffYearsAndPrice(DicTariff tariff)
            {
                var result = new TariffCountAndPrice();
                var years = 0;
                for (int i = 0; i < tariff.ProtectionDocSupportYearsUntil; i++)
                {
                    if (i + 1 <= SupportYear)
                    {
                        years++;
                    }
                }
                result.YearsCount = years;
                decimal price = tariff.Price ?? 0;
                //switch (BeneficiaryType?.Code)
                //{
                //    case "SMB":
                //        price = tariff.PriceBusiness ?? 0;
                //        break;
                //    case "VET":
                //        price = tariff.PriceBeneficiary ?? 0;
                //        break;
                //}
                //if (CustomerType?.Code != "2")
                //{
                //    price = tariff.PriceUl ?? 0;
                //}
                //else
                //{
                //    price = tariff.PriceFl ?? 0;
                //}
                result.Price = price / 100 * 112;
                return result;
            }
        }

        internal class TariffCountAndPrice
        {
            public int YearsCount { get; set; }
            public decimal Price { get; set; }
        }
    }
}