using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertRequestsHandler : BaseHandler
    {
        private NewNiisRequestService _newNiisRequestService;
        private readonly OldNiisRequestService _oldNiisRequestService;
        private readonly AttachmentFileHelper _attachmentFileHelper;
        private readonly AppConfiguration _appConfiguration;
        private readonly InsertRequestRelationsHandler _insertRequestRelationsHandler;

        private NiisWebContextMigration _context;
        public InsertRequestsHandler(
            NiisWebContextMigration context,
            OldNiisRequestService oldNiisRequestService,
            AttachmentFileHelper attachmentFileHelper,
            AppConfiguration appConfiguration,
            InsertRequestRelationsHandler insertRequestRelationsHandler) : base(context)
        {
            _context = context;

            _oldNiisRequestService = oldNiisRequestService;
            _attachmentFileHelper = attachmentFileHelper;
            _appConfiguration = appConfiguration;
            _insertRequestRelationsHandler = insertRequestRelationsHandler;
        }

        public void Execute()
        {
            var packageIndex = 1;
            var allRequestsCount = _oldNiisRequestService.GetRequestsCount();
            var requestsCommited = 0;
            var requestsNeedToTransfer = 0;

            var stopwatch = Stopwatch.StartNew();

            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            while (true)
            {
                using (var db = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _newNiisRequestService = new NewNiisRequestService(db);
                    if (requestsCommited == 0)
                    {
                        requestsCommited = _newNiisRequestService.GetRequetsCount();
                        requestsNeedToTransfer = allRequestsCount - requestsCommited;
                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var lastBarcode = _newNiisRequestService.GetLastBarcodeOfRequest();

                            var requests = _oldNiisRequestService.GetRequests(_appConfiguration.PackageSize, lastBarcode ?? 0);

                            if (requests.Any() == false)
                            {
                                break;
                            }
                            //_attachmentFileHelper.AttachFilesToRequests(requests);
                            _newNiisRequestService.CreateRangeRequests(requests);
                            InsertRequestWorkflows(requests);
                            _newNiisRequestService.UpdateRangeRequests(requests);

                            var requestIds = requests.Select(r => r.Id).ToList();
                            InsertRequestInfos(requestIds);
                            _insertRequestRelationsHandler.Execute(requestIds, db);

                            transaction.Commit();

                            Console.Write($"\rRequests commited - {packageIndex * _appConfiguration.PackageSize}/{requestsNeedToTransfer}. Time elapsed: {stopwatch.Elapsed}");
                            packageIndex++;
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex);
                            Log.LogError(ex?.InnerException?.Message);
                            Log.LogError(ex?.InnerException?.StackTrace);
                            Log.LogError(ex?.StackTrace);
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        #region Private methods

        private void InsertRequestWorkflows(List<Request> requests)
        {
            var manyRequestWorkflows = _oldNiisRequestService.GetRequestWorkflows(requests.Select(r => r.Id).ToList());
            foreach (var request in requests)
            {
                var requestWorkflows = manyRequestWorkflows.Where(w => w.OwnerId == request.Id).ToList();
                if (requestWorkflows.Any() == false)
                {
                    continue;
                }
                _newNiisRequestService.CreateRangeRequestWorkflows(requestWorkflows);
                request.CurrentWorkflowId = requestWorkflows.OrderBy(w => w.DateCreate).LastOrDefault()?.Id;
            }

        }

        private void InsertRequestInfos(List<int> requestIds)
        {
            var reuquestInfos = _oldNiisRequestService.GetRequestInfos(requestIds);
            if (reuquestInfos.Any())
            {
                _newNiisRequestService.CreateRangeRequestInfos(reuquestInfos);
            }
        }
        #endregion
    }
}
