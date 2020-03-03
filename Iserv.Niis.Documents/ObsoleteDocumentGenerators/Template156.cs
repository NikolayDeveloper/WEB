using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(2691, "IZ-3A-KZ")]
    class Template156 : DocumentGeneratorBase
    {
        public Template156(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.NumberApxWork),
                BuildField(TemplateFieldName.CurrentYear),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority86WithoutCode),
                BuildField(TemplateFieldName.DeclarantsKz),
                BuildField(TemplateFieldName.IpcCodes),
                BuildField(TemplateFieldName.PatentNameKz),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.TransferDateWithCode),
                BuildField(TemplateFieldName.Authors),
                BuildField(TemplateFieldName.AuthorsCountryCodesKz),
                BuildField(TemplateFieldName.PatentOwnerCountryCodesKz),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.DescriptionReferat),
                BuildField(TemplateFieldName.PatentOwnerKz)
              );
           
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", DocumentGeneratorBase.UserInputFieldsParameterName };
        }
    }
}
