using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;
using Iserv.Niis.Documents.Enums;

namespace Iserv.Niis.Documents.ConclusionGenerators
{
    [DocumentGenerator(4224, DicDocumentTypeCodes.TZPOL555PR)]
    public class ExpertConclusionByRequestPR : DocumentGeneratorBase, IComplexDataDocumentGenerator
    {
        private readonly IFileStorage _fileStorage;

        public ExpertConclusionByRequestPR(
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
            return _expertConclusionByRequestHelper.FillData(Executor, _fileStorage, Parameters, DicDocumentTypeCodes.TZPOL555PR);
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId", UserInputFieldsParameterName };
        }

    }
}
