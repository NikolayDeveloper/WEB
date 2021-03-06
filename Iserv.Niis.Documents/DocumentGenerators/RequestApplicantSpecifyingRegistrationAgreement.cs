﻿using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using TemplateEngine.Docx;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Enums;


namespace Iserv.Niis.Documents.DocumentGenerators
{
    // TODO: пустой шаблон
    [DocumentGenerator(12363, DicDocumentTypeCodes.DK_ZAPROS)]
    public class RequestApplicantSpecifyingRegistrationAgreement : DocumentGeneratorBase
    {
        public RequestApplicantSpecifyingRegistrationAgreement(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
        IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
        templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }


        protected override Content PrepareValue()
        {
            return new Content(new FieldContent("Test", "Test"));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId" };
        }
    }
}
