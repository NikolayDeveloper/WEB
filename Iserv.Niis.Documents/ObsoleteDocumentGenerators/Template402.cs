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
	[DocumentGenerator(4274, "P001.1")]
	public class Template402 : DocumentGeneratorBase
	{
		public Template402(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
		{
            QrCodePosition = QrCodePosition.Header;
        }

		protected override Content PrepareValue()
		{
			return new Content(
				BuildField(TemplateFieldName.CorrespondenceContact),
				BuildField(TemplateFieldName.CorrespondenceAddress),
				BuildField(TemplateFieldName.DocumentNumber),
				BuildField(TemplateFieldName.CurrentUser),
				BuildField(TemplateFieldName.CurrentUserPhoneNumber),
				BuildField(TemplateFieldName.PatentOwner),
				BuildField(TemplateFieldName.ApplicantAddress),		
				BuildField(TemplateFieldName.DateTimeNow)					
			);
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}
	}
}