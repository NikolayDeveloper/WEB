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
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(27, DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification)]
    public class OUT_UV_Pol_vyd_zayavTZ_v1_19_Template : DocumentGeneratorBase
    {
        public OUT_UV_Pol_vyd_zayavTZ_v1_19_Template(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
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
                BuildImage(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.HeadName),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.CurrentUserPhoneNumber)
            );
        }
    }
}
