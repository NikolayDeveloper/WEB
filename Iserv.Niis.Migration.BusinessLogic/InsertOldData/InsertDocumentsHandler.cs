using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Diagnostics;
using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertDocumentsHandler : BaseHandler
    {
        private readonly OldNiisDocumentService _oldNiisDocumentService;
        private readonly AttachmentFileHelper _attachmentFileHelper;
        private readonly AppConfiguration _appConfiguration;
        private readonly InsertDocumentRelationsHandler _insertDocumentRelationsHandler;

        private NewNiisDocumentService _newNiisDocumentService;
        private NiisWebContextMigration _context;

        public InsertDocumentsHandler(
            OldNiisDocumentService oldNiisDocumentService,
            NiisWebContextMigration context,
            AttachmentFileHelper attachmentFileHelper,
            AppConfiguration appConfiguration,
            InsertDocumentRelationsHandler insertDocumentRelationsHandler) : base(context)
        {
            _oldNiisDocumentService = oldNiisDocumentService;
            _attachmentFileHelper = attachmentFileHelper;
            _appConfiguration = appConfiguration;
            _insertDocumentRelationsHandler = insertDocumentRelationsHandler;
        }

        public void Execute()
        {
            var packageIndex = 1;
            var allDocumentsCount = _oldNiisDocumentService.GetDocumentsCount();
            var documentsCommited = 0;
            var documentNeedToTransfer = 0;

            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            var stopwatch = Stopwatch.StartNew();

            while (true)
            {
                using (var db = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _context = db;

                    _newNiisDocumentService = new NewNiisDocumentService(db);

                    if (documentsCommited == 0)
                    {
                        documentsCommited = _newNiisDocumentService.GetDocumentsCount();
                        documentNeedToTransfer = allDocumentsCount - documentsCommited;
                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var lastBarcode = _newNiisDocumentService.GetLastBarcodeOfDocument();

                            var documents = _oldNiisDocumentService.GetDocuments(_appConfiguration.PackageSize, lastBarcode ?? 0);

                            if (documents.Any() == false)
                            {
                                break;
                            }

                            _newNiisDocumentService.CreateRangeDocuments(documents);
                            //_attachmentFileHelper.AttachFilesToDocuments(documents);
                            InsertDocumentWorkflows(documents);
                            _newNiisDocumentService.UpdateRangeDocuments(documents);
                            _insertDocumentRelationsHandler.Execute(documents.Select(d => d.Id).ToList(), db);

                            transaction.Commit();

                            Console.Write($"\rDocuments commited - {packageIndex * _appConfiguration.PackageSize}/{documentNeedToTransfer}. Time elapsed: {stopwatch.Elapsed}");
                            packageIndex++;

                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex);
                            if (ex.InnerException != null)
                                Console.WriteLine(ex.InnerException);
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        private void InsertDocumentWorkflows(List<Document> documents)
        {
            var manyDocumentWorkflows = _oldNiisDocumentService.GetDocumentWorkflows(documents.Select(d => d.Id).ToList());
            foreach (var document in documents)
            {
                var documentWorkflows = manyDocumentWorkflows.Where(w => w.OwnerId == document.Id).ToList();

                if (documentWorkflows.Any() == false)
                {
                    continue;
                }
                _newNiisDocumentService.CreateRangeDocumentWorkflows(documentWorkflows);
                //document.CurrentWorkflowId = documentWorkflows.OrderBy(d => d.DateCreate).LastOrDefault()?.Id;
                _insertDocumentRelationsHandler.ExecuteDocumentUserSignaturies(documentWorkflows, _context);
            }
        }
    }
}
