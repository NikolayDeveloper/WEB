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
    /// Уведомление о принятии к рассмотрению заявки на регистрацию ТЗ_знака обслуживания - генератор шаблона
    /// </summary>
    [DocumentGenerator(27, DicDocumentTypeCodes.TZPRED1)]
    public class OUT_Uv_pred_rassm_reg_TZ_v1_19_Template: DocumentGeneratorBase
    {
        public OUT_Uv_pred_rassm_reg_TZ_v1_19_Template(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "UserId", "RequestId" };
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondenceEmail),
                BuildField(TemplateFieldName.CorrespondencePhone),
                BuildField(TemplateFieldName.RequestDate),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.RequestNumber),
                //BuildField(TemplateFieldName.RequestTypeTrademark),
                //BuildField(TemplateFieldName.HeadHeadName),
                BuildField(TemplateFieldName.DeclarantsAndAddress),
                BuildField(TemplateFieldName.Mktu511),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.ExecutorName)
            );
        }
    }
}
