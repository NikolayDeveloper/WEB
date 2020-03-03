using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertProtectionDocsHandler : BaseHandler
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly OldNiisProtectionDocService _oldNiisProtectionDocService;
        private NewNiisProtectionDocService _newNiisProtectionDocService;
        private readonly InsertProtectionDocRelationsHandler _insertProtectionDocRelationsHandler;

        public InsertProtectionDocsHandler(
            NiisWebContextMigration context,
            AppConfiguration appConfiguration,
            OldNiisProtectionDocService oldNiisProtectionDocService,
            InsertProtectionDocRelationsHandler insertProtectionDocRelationsHandler) : base(context)
        {
            _appConfiguration = appConfiguration;
            _oldNiisProtectionDocService = oldNiisProtectionDocService;
            _insertProtectionDocRelationsHandler = insertProtectionDocRelationsHandler;
        }

        public void Execute()
        {
            var packageIndex = 1;
            var allProtectionDocsCount = _oldNiisProtectionDocService.GetProtectionDocsCount();
            var protectionDocsCommited = 0;
            var requestsNeedToTransfer = 0;

            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            var stopwatch = Stopwatch.StartNew();

            while (true)
            {
                using (var db = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _newNiisProtectionDocService = new NewNiisProtectionDocService(db);

                    if (protectionDocsCommited == 0)
                    {
                        protectionDocsCommited = _newNiisProtectionDocService.GetProtectionDocsCount();
                        requestsNeedToTransfer = allProtectionDocsCount - protectionDocsCommited;
                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var lastBarcode = _newNiisProtectionDocService.GetLastBarcodeOfProtectionDoc();
                            var protectionDocs = _oldNiisProtectionDocService.GetProtectionDocs(_appConfiguration.PackageSize, lastBarcode ?? 0);

                            if (protectionDocs.Any() == false)
                            {
                                break;
                            }
                            
                            _newNiisProtectionDocService.CreateRangeProtectionDocs(protectionDocs);

                            CreateWorkflowForProtectionDocs(protectionDocs, db);

                            var protectionDocIds = protectionDocs.Select(p => p.Id).ToList();
                            _insertProtectionDocRelationsHandler.Execute(protectionDocIds, db);
                            InsertProtectionDocInfos(protectionDocIds);

                            transaction.Commit();

                            Console.Write($"\rProtectionDocs commited - {packageIndex * _appConfiguration.PackageSize}/{requestsNeedToTransfer}. Time elapsed: {stopwatch.Elapsed}");

                            packageIndex++;
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex);
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        #region Privare Methods
        private void InsertProtectionDocInfos(List<int> protectionDocIds)
        {
            var protectionDocInfos = _oldNiisProtectionDocService.GetProtectionDocInfos(protectionDocIds);
            if (protectionDocInfos.Any())
            {
                _newNiisProtectionDocService.CreateRangeProtectionDocInfos(protectionDocInfos);
            }
        }

        private void CreateWorkflowForProtectionDocs(List<ProtectionDoc> protectionDocs, NiisWebContextMigration context)
        {
            var protectionDocRouteId = context.DicRoutes.First(d => d.Code == RouteCodes.ProtectionDoc).Id;
            var firstRouteStageProtectionDoc = context.DicRouteStages
                .First(d => d.RouteId == protectionDocRouteId && d.IsFirst);

            protectionDocs.ForEach(protectionDoc =>
            {
                protectionDoc.CurrentWorkflow = new ProtectionDocWorkflow
                {
                    OwnerId = protectionDoc.Id,
                    DateCreate = DateTimeOffset.Now,
                    RouteId = protectionDocRouteId,
                    CurrentStageId = firstRouteStageProtectionDoc.Id,
                    CurrentUserId = _appConfiguration.MainExecutorId,
                    IsComplete = firstRouteStageProtectionDoc.IsLast,
                    IsSystem = firstRouteStageProtectionDoc.IsSystem,
                    IsMain = firstRouteStageProtectionDoc.IsMain
                };
            });
        }

        #endregion
    }
}
