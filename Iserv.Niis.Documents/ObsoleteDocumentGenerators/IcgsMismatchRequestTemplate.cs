﻿using System.Collections.Generic;
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
    //[DocumentGenerator(0, DicDocumentTypeCodes.OUT_Zap_Pred_Nesootv_MKTU_v1_19)]
    [DocumentGenerator(0, DicDocumentTypeCodes.IcgsMismatchRequest)]
    public class IcgsMismatchRequestTemplate: DocumentGeneratorBase
    {
        public IcgsMismatchRequestTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(new FieldContent("Test", "Test"));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
