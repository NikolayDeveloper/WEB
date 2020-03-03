using Iserv.Niis.Migration.BusinessLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertDocumentRelationsHandler : BaseHandler
    {
        private readonly OldNiisDocumentRelationService _oldDocumentRelationService;

        private NewNiisDocumentRelationService _newNiisDocumentRelationService;

        public InsertDocumentRelationsHandler(NiisWebContextMigration context,
            OldNiisDocumentRelationService oldDocumentRelationService) : base(context)
        {
            _oldDocumentRelationService = oldDocumentRelationService;
        }

        public void Execute(List<int> documentIds, NiisWebContextMigration niisWebContext)
        {
            _newNiisDocumentRelationService = new NewNiisDocumentRelationService(niisWebContext);

            InsertDocumentEarlyRegs(documentIds);
            InsertDocumentCustomers(documentIds);
            InsertDocumentExecutors(documentIds);
            InsertGetPaymentRegistryDatas(documentIds);
        }

        public void ExecuteDocumentUserSignaturies(List<DocumentWorkflow> documentWorkflows, NiisWebContextMigration niisWebContext)
        {
            _newNiisDocumentRelationService = new NewNiisDocumentRelationService(niisWebContext);

            var ignoreSignaturies = new List<int>();
            documentWorkflows = documentWorkflows.OrderByDescending(dw => dw.Id).ToList();

            foreach (var documentWorkflow in documentWorkflows)
            {
                var documentUserSigrature = _oldDocumentRelationService.GetDocumentUserSignature(documentWorkflow, ignoreSignaturies);
                if (documentUserSigrature != null)
                    _newNiisDocumentRelationService.CreateDocumentUserSignature(documentUserSigrature);
            }
        }

        private void InsertDocumentEarlyRegs(List<int> documentIds)
        {
            var documentEarlyRegs = _oldDocumentRelationService.GetDocumentEarlyRegs(documentIds);
            if (documentEarlyRegs.Any())
                _newNiisDocumentRelationService.CreateRangeDocumentEarlyRegs(documentEarlyRegs);
        }

        private void InsertDocumentCustomers(List<int> documentIds)
        {
            var documentCustomers = _oldDocumentRelationService.GetDocumentCustomers(documentIds);
            if (documentCustomers.Any())
                _newNiisDocumentRelationService.CreateRangeDocumentCustomers(documentCustomers);
        }

        private void InsertDocumentExecutors(List<int> documentIds)
        {
            var documentExecutors = _oldDocumentRelationService.GetDocumentExecutors(documentIds);
            if (documentExecutors.Any())
                _newNiisDocumentRelationService.CreateRangeDocumentExecutors(documentExecutors);
        }

        private void InsertGetPaymentRegistryDatas(List<int> documentIds)
        {
            var paymentRegistryDatas = _oldDocumentRelationService.GetPaymentRegistryDatas(documentIds);
            if (paymentRegistryDatas.Any())
                _newNiisDocumentRelationService.CreateRangePaymentRegistryDatas(paymentRegistryDatas);
        }
    }
}
