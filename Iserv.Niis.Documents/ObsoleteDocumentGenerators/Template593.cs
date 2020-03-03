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
	[DocumentGenerator(4071, "POL2_NMPT")]
	public class Template593 : DocumentGeneratorBase
	{
		public Template593(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
		{
            QrCodePosition = QrCodePosition.Header;
        }

		protected override Content PrepareValue()
		{
			return new Content(
				BuildField(TemplateFieldName.CorrespondenceContact),
				BuildField(TemplateFieldName.CorrespondenceAddress),
				BuildImage(TemplateFieldName.Image),
				BuildField(TemplateFieldName.CurrentYear),
				BuildField(TemplateFieldName.RequestNumber),
				BuildField(TemplateFieldName.RequestDate),
				BuildField(TemplateFieldName.Priority31),
				BuildField(TemplateFieldName.Priority32),
				BuildField(TemplateFieldName.Priority33),
				BuildField(TemplateFieldName.Declarants),
				BuildField(TemplateFieldName.ApplicantAddress),
				BuildField(TemplateFieldName.Icgs511),
				BuildField(TemplateFieldName.Colors),
				BuildField(TemplateFieldName.Disclaimer),
				BuildField(TemplateFieldName.DisclaimerReason),
				BuildField(TemplateFieldName.CurrentUser)
			);
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}
	}
}
