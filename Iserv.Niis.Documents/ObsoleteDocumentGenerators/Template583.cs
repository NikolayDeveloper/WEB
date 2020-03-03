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
	[DocumentGenerator(3772, "POL3_NVPT")]
	public class Template583 : DocumentGeneratorBase
	{
		public Template583(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, 
			IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor, 
				templateFieldValueFactory, fileConverter, docxTemplateHelper)
		{
            QrCodePosition = QrCodePosition.Header;
        }

		protected override Content PrepareValue()
		{
			return new Content(
				BuildField(TemplateFieldName.CurrentUser),
				BuildField(TemplateFieldName.RequestNameRu),
				BuildField(TemplateFieldName.PlaceOfOrigin),
				BuildField(TemplateFieldName.SpecialPropertiesOfGood),
				BuildField(TemplateFieldName.TypeOfGoods),
				BuildField(TemplateFieldName.RequestNumber),
				BuildField(TemplateFieldName.RequestDate),
				BuildField(TemplateFieldName.CorrespondenceAddress),
				BuildField(TemplateFieldName.CorrespondenceContact),
				BuildField(TemplateFieldName.CurrentYear),
				BuildField(TemplateFieldName.DeclarantsAndAddress));
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}
	}
}
