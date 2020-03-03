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
    /// Заключение о предварительном отказе в регистрации ТЗ - генератор шаблона
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion)]
    public class OUT_Zakl_Pol_Predv_otkaz_Reg_TZ_V1_19_Template: DocumentGeneratorBase
    {
        public OUT_Zakl_Pol_Predv_otkaz_Reg_TZ_V1_19_Template(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.MaterialNum),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.ExpertName),
                BuildField(TemplateFieldName.DivisionHeadPost),
                BuildField(TemplateFieldName.MaterialDateCreate),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority31),
                BuildField(TemplateFieldName.Priority32),
                BuildField(TemplateFieldName.Priority33),
                BuildField(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.Disclaimer),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.DeclarantsAndAddress),
                BuildField(TemplateFieldName.Mktu511),
                BuildImage(TemplateFieldName.Image));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", UserInputFieldsParameterName };
        }
    }
}
