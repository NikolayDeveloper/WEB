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
    [DocumentGenerator(4109, "DK_ZAKLUCHENIE_OTKAZ")]
    public class Template159 : DocumentGeneratorBase
    {
        public Template159(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                //BuildField(TemplateFieldName.RegisteredContractRu),
                //BuildField(TemplateFieldName.OpsType),
                //BuildField(TemplateFieldName.OpsNameRu),
                //BuildField(TemplateFieldName.FirstPartyNameRu),
                BuildField(TemplateFieldName.FirstPartyLegalNameOrFioRu),
                //BuildField(TemplateFieldName.FirstPartyAddressRu),
                BuildField(TemplateFieldName.FirstPartyCountryCode),
                //BuildField(TemplateFieldName.SecondPartyNameRu),
                BuildField(TemplateFieldName.SecondPartyLegalNameOrFioRu),
                //BuildField(TemplateFieldName.SecondPartyAddressRu),
                BuildField(TemplateFieldName.SecondPartyCountryCode),

                //BuildField(TemplateFieldName.RegisteredContractKz),
                BuildField(TemplateFieldName.OpsTypeKz),
                BuildField(TemplateFieldName.OpsNameKz),
                //BuildField(TemplateFieldName.FirstPartyNameKz),
                BuildField(TemplateFieldName.FirstPartyLegalNameOrFioKz),
                //BuildField(TemplateFieldName.FirstPartyAddressKz),
                BuildField(TemplateFieldName.FirstPartyCountryCode),
                //BuildField(TemplateFieldName.SecondPartyNameKz),
                BuildField(TemplateFieldName.SecondPartyLegalNameOrFioKz),
                //BuildField(TemplateFieldName.SecondPartyAddressKz),
                BuildField(TemplateFieldName.SecondPartyCountryCode),
                
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.CurrentUserKz));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }
    }
}