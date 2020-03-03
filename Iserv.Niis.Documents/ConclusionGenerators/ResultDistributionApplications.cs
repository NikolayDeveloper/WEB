using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Workflows;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ConclusionGenerators
{
    [DocumentGenerator(10020, DicDocumentTypeCodes.ResultDistributionRequests)]
    public class ResultDistributionApplications : DocumentGeneratorBase, IComplexDataDocumentGenerator
    {
        private readonly IFileStorage _fileStorage;
        public ResultDistributionApplications(IExecutor executor,
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
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var resultDistributionApplicationsInfo = GetResultDistributionApplicationsInfo();

            return new Content(
                new FieldContent(nameof(ResultDistributionApplicationsInfo.RegistryNumberRegister), resultDistributionApplicationsInfo.RegistryNumberRegister),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.DateOfDistribution), resultDistributionApplicationsInfo.DateOfDistribution),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.RequestNumber), resultDistributionApplicationsInfo.RequestNumber),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.IpcCode), resultDistributionApplicationsInfo.IpcCode),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.Executor), resultDistributionApplicationsInfo.Executor),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.ApplicationDistributionDate), resultDistributionApplicationsInfo.ApplicationDistributionDate),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.UserPosition), resultDistributionApplicationsInfo.UserPosition),
                new FieldContent(nameof(ResultDistributionApplicationsInfo.User), resultDistributionApplicationsInfo.User)
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }

        private ResultDistributionApplicationsInfo GetResultDistributionApplicationsInfo()
        {
            var necessaryRouteStageCodes = new string[] { RouteStageCodes.I_03_2_2_1, RouteStageCodes.I_03_2_2_0, RouteStageCodes.I_03_2_4, RouteStageCodes.UM_03_2 };

            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int)Parameters["RequestId"]));

            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute((int)Parameters["DocumentId"]));

            var requestWorflows = Executor.GetQuery<GetRequestWorkflowsByOwnerIdQuery>().Process(q => q.Execute(request.Id));

            var workflow = requestWorflows
                .Where(w => necessaryRouteStageCodes.Contains(w.CurrentStage.Code))
                .OrderBy(w => document.DateCreate - w.DateCreate)
                .FirstOrDefault();

            var mainIpcCode = request.IPCRequests.FirstOrDefault(i => i.IsMain)?.Ipc.Code ?? string.Empty;

            return new ResultDistributionApplicationsInfo
            {
                RegistryNumberRegister = document?.DocumentNum ?? string.Empty,
                DateOfDistribution = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                RequestNumber = request.RequestNum ?? string.Empty,
                IpcCode = mainIpcCode,
                Executor = workflow?.CurrentUser.NameRu ?? string.Empty,
                ApplicationDistributionDate = DateTimeOffset.Now.ToString("dd.MM.yyyy"),
                UserPosition = workflow?.FromUser.Position.PositionType.NameRu ?? string.Empty,
                User = workflow?.FromUser.NameRu ?? string.Empty

            };
        }
    }

    class ResultDistributionApplicationsInfo
    {
        public string RegistryNumberRegister { get; set; }
        public string DateOfDistribution { get; set; }
        public string RequestNumber { get; set; }
        public string IpcCode { get; set; }
        public string Executor { get; set; }
        public string ApplicationDistributionDate { get; set; }
        public string UserPosition { get; set; }
        public string User { get; set; }
    }
}
