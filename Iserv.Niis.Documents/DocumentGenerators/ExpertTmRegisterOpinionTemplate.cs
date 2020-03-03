using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(0, DicDocumentTypeCodes.ExpertTmRegisterOpinion)]
    public class ExpertTmRegisterOpinionTemplate: DocumentGeneratorBase
    {
        private const string PositionHeadDepartmentCode = "029";

        public ExpertTmRegisterOpinionTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var templateData = GetTemplateData();
            return new Content(
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                //BuildField(TemplateFieldName.DeputyDirectorName),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.Mktu511),
                BuildField(TemplateFieldName.Disclaimer),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.DeclarantsAndAddress),
                //BuildField(TemplateFieldName.HeadHeadName),
                BuildField(TemplateFieldName.ExpertName),
                new FieldContent(nameof(TemplateData.ReportPageCount), templateData.ReportPageCount.ToString()),
                new FieldContent(nameof(TemplateData.HeadOfDepartment), templateData.HeadOfDepartment)
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        private TemplateData GetTemplateData()
        {
            return new TemplateData
            {
                ReportPageCount = (int?)Parameters["PageCount"] ?? 0,
                HeadOfDepartment = GetHeadOfDepartment(PositionHeadDepartmentCode)
            };
        }

        private string GetHeadOfDepartment(string positionCode)
        {
            var headOfDepartment = Executor.GetQuery<GetUserByPositionCodeQuery>()
                .Process(q => q.Execute(positionCode));

            return headOfDepartment?.NameRu ?? string.Empty;
        }

        private class TemplateData
        {
            internal string HeadOfDepartment { get; set; }
            internal int ReportPageCount { get; set; }
        }
    }
}
