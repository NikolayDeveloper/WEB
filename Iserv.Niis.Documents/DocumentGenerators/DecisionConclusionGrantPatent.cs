using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using TemplateEngine.Docx;


namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(0, DicDocumentTypeCodes.ConclusionOfInventionPatentGrant)]
    public class DecisionConclusionGrantPatent : DocumentGeneratorBase
    {
        public DecisionConclusionGrantPatent(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
          IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
          templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }


        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.TransferDateWithCode),
                BuildField(TemplateFieldName.BulletinDate),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.BulletinNumber),
                BuildField(TemplateFieldName.IpcCodes),
                BuildField(TemplateFieldName.PatentAttorney),
                BuildField(TemplateFieldName.RequestDateCreate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Priority86),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.Authors)
            );
        }


        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId" };
        }
    }
}
