using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ExpertSearch;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ConclusionGenerators
{
    [DocumentGenerator(4224, DicDocumentTypeCodes.TZPOL555)]
    public class ExpertConclusionByRequest : DocumentGeneratorBase, IComplexDataDocumentGenerator
    {
        private readonly IFileStorage _fileStorage;

        public ExpertConclusionByRequest(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper, 
            IFileStorage fileStorage)
            : base(
                executor,
                templateFieldValueFactory,
                fileConverter,
                docxTemplateHelper)
        {
            _fileStorage = fileStorage;
            DefaultUserInputValue = "Не выявлено";
            QrCodePosition = QrCodePosition.Header;
        }

        readonly ExpertConclusionByRequestHelper _expertConclusionByRequestHelper = new ExpertConclusionByRequestHelper();

        protected override Content PrepareValue()
        {
            return _expertConclusionByRequestHelper.FillData(Executor,_fileStorage,Parameters, DicDocumentTypeCodes.TZPOL555);
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId", UserInputFieldsParameterName };
        }

    }
}