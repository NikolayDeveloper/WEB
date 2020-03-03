using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
	[DocumentGenerator(4218, DicDocumentTypeCodes.NotificationOfAnswerTimeExpiration)]
	public class Template69 : DocumentGeneratorBase
	{
		public Template69(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
		{
            QrCodePosition = QrCodePosition.Header;
        }

		protected override Content PrepareValue()
		{
			//ToDo: Поля не реализованы так как нет функционала календаря
			return new Content(
				BuildField(TemplateFieldName.CorrespondenceContact),
				BuildField(TemplateFieldName.CorrespondenceAddress),
				BuildField(TemplateFieldName.RequestNumber),
				BuildField(TemplateFieldName.RequestDate),
				BuildField(TemplateFieldName.DeclarantsAndAddress),
				BuildField(TemplateFieldName.Icgs511),
				BuildField(TemplateFieldName.Colors),
				BuildField(TemplateFieldName.Priority31WithoutCode),
				BuildField(TemplateFieldName.Priority32WithoutCode),
				BuildField(TemplateFieldName.Priority33WithoutCode),
				//BuildField(TemplateFieldName.ResponseDeadline),
				//BuildField(TemplateFieldName.ExpirationDate),
				BuildField(TemplateFieldName.CurrentUser),
				BuildField(TemplateFieldName.CurrentUserPhoneNumber)
			);
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId", DocumentGeneratorBase.UserInputFieldsParameterName };
		}
	}
}