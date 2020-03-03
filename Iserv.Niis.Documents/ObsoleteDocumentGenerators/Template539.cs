using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{[DocumentGenerator(127, "PM3B")]
    public class Template539 : DocumentGeneratorBase
    {
        public Template539(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDateCreate),
                BuildField(TemplateFieldName.PatentAttorney),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.TransferDateWithCode),
                BuildField(TemplateFieldName.Priority86WithoutCode),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.BulletinDate),
                BuildField(TemplateFieldName.BulletinNumber),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.PatentNameKz),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.CurrentYear),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.Authors),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.DescriptionReferat),
                BuildField(TemplateFieldName.IpcCodes),
                BuildField(TemplateFieldName.AuthorsCountryCodes),
                BuildField(TemplateFieldName.PatentOwnerCountryCodesRu),
                BuildField(TemplateFieldName.PatentOwner)

              );        
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", DocumentGeneratorBase.UserInputFieldsParameterName };
        }
    }
}
