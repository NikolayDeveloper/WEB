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
    public class InsertContractsHandler : BaseHandler
    {
        private NewNiisContractService _newNiisContractService;
        private readonly OldNiisContractService _oldNiisContractService;
        private readonly AppConfiguration _appConfiguration;
        private readonly AttachmentFileHelper _attachmentFileHelper;

        public InsertContractsHandler(
            NiisWebContextMigration context,
            OldNiisContractService oldNiisContractService,
            AppConfiguration appConfiguration,
            AttachmentFileHelper attachmentFileHelper) : base(context)
        {
            _oldNiisContractService = oldNiisContractService;
            _appConfiguration = appConfiguration;
            _attachmentFileHelper = attachmentFileHelper;
        }

        public void Migrate()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            var lastBarcode = 0;

            using (var db = new NiisWebContextMigration(optionsBuilder.Options))
            {
                _newNiisContractService = new NewNiisContractService(db);

                lastBarcode = _newNiisContractService.GetLastBarcodeId();
                _newNiisContractService = null;
            }

            var oldContractIds = _oldNiisContractService.GetAllOldContractIds().Where(r => r > lastBarcode).ToList();

            var stopwatch = Stopwatch.StartNew();
            var contractIndex = 0;
            var contractForMigrateCount = oldContractIds.Count;

            foreach (var oldContractId in oldContractIds)
            {
                using (var db = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _newNiisContractService = new NewNiisContractService(db);

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var contract = _oldNiisContractService.GetContractByBarcodeId(oldContractId);

                            if (contract == null) return;

                            _newNiisContractService.SaveContract(contract);

                            var contractWorkFlows = _oldNiisContractService.GetContractWorkflowsByOldContractId(oldContractId);
                            if (contractWorkFlows.Any())
                            {
                                contract.Workflows = contractWorkFlows;
                                contract.CurrentWorkflow = contractWorkFlows.OrderByDescending(c => c.DateCreate).First();
                            }

                            //var mainAttachmentId = _attachmentFileHelper.GetMainAttachmentId(oldContractId, contract.Id);                                       
                            //contract.MainAttachmentId = mainAttachmentId;

                            _newNiisContractService.UpdateContract(contract);

                            var contractCusomers = _oldNiisContractService.GetContractCustomerByOldContractId(oldContractId);
                            _newNiisContractService.SaveContractCustomers(contractCusomers);

                            transaction.Commit();

                            Console.Write($"\rМигрировано договоров {++contractIndex} из {contractForMigrateCount}. Затрачено времени: {stopwatch.Elapsed}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            if (ex.InnerException != null)
                                Console.WriteLine(ex.InnerException);

                            Log.LogError(ex);
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
