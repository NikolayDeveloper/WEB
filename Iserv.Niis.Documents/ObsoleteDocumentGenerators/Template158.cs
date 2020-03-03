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
    [DocumentGenerator(4093, "DK_ZAKLUCHENIE")]
    public class Template158 : DocumentGeneratorBase
    {
        public Template158(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) 
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.InformationAboutAttachedToContract),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.ContractType),
                BuildField(TemplateFieldName.ContractTypeKz),
                BuildField(TemplateFieldName.ContractStorona1NameRu),
                BuildField(TemplateFieldName.ContractStorona1NameKz),
                BuildField(TemplateFieldName.ContractStorona2NameRu),
                BuildField(TemplateFieldName.ContractStorona2NameKz),
                BuildField(TemplateFieldName.ContractStorona1Address),
                BuildField(TemplateFieldName.ContractStorona1AddressKz),
                BuildField(TemplateFieldName.ContractStorona2Address),
                BuildField(TemplateFieldName.ContractStorona2AddressKz),
                BuildField(TemplateFieldName.ContractProtectionDocTypeKz),
                BuildField(TemplateFieldName.ContractProtectionDocType));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", DocumentGeneratorBase.UserInputFieldsParameterName};
        }
    }
}
