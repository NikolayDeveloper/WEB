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
    //[DocumentGenerator(512, DicDocumentTypeCodes.OUT_Uv_pol_prekr_del_prosb_zayav_v1_19)]
    [DocumentGenerator(512, "TZPOL8")]
    public class Template72 : DocumentGeneratorBase
    {
        public Template72(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.Mktu511),
                BuildField(TemplateFieldName.CurrentUser));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"UserId", "RequestId", DocumentGeneratorBase.UserInputFieldsParameterName};
        }
    }
}