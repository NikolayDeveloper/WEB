using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    public class IntegrationRequisitionInfoHelper
    {
        private readonly NiisWebContext _niisContext;
        private readonly DictionaryHelper _dictionaryHelper;

        public IntegrationRequisitionInfoHelper(NiisWebContext niisContext, DictionaryHelper dictionaryHelper)
        {
            _niisContext = niisContext;
            _dictionaryHelper = dictionaryHelper;
        }

        public List<RequisitionInfo> GetRequisitionInfoByMessageType(int docTypeId, int protectionDocTypeId, string xin)
        {
            var documentIds = GetDocumentIdsByMessageType(docTypeId, protectionDocTypeId, xin);
            var documents = _niisContext.Documents
                .Include(x => x.Workflows)
                .ThenInclude(x => x.CurrentStage)
                .ThenInclude(x => x.OnlineRequisitionStatus)
                .Where(x => documentIds.Contains(x.Id) &&
                            x.CurrentWorkflows.Any(d => d.CurrentStage.OnlineRequisitionStatusId != null))
                .Select(x => new
                {
                    x.Barcode,
                    DocumentDate = x.DateCreate,
                    StatusDate = x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).DateCreate,
                    x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).CurrentStage.OnlineRequisitionStatusId,
                    StatusName = x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).CurrentStage.OnlineRequisitionStatus.NameRu
                });
            return GetRequisitionInfos(documents);
        }

        public List<RequisitionInfo> GetRequistionsListForPayment(int docTypeId, int protectionDocTypeId, string xin)
        {
            var documentIds = GetDocumentIdsForPayment(docTypeId, protectionDocTypeId, xin);
            var documents = _niisContext.Documents
                .Include(x => x.Workflows)
                .ThenInclude(x => x.CurrentStage)
                .ThenInclude(x => x.OnlineRequisitionStatus)
                .Where(x => documentIds.Contains(x.Id) &&
                            x.CurrentWorkflows.Any(d => d.CurrentStage.OnlineRequisitionStatusId != null))
                .Select(x => new
                {
                    x.Barcode,
                    DocumentDate = x.DateCreate,
                    StatusDate = x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).DateCreate,
                    x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).CurrentStage.OnlineRequisitionStatusId,
                    StatusName = x.CurrentWorkflows.First(d => d.CurrentStage.OnlineRequisitionStatusId != null).CurrentStage.OnlineRequisitionStatus.NameRu
                });
            return GetRequisitionInfos(documents);
        }

        private List<int> GetDocumentIdsByMessageType(int docTypeId, int protectionDocTypeId, string xin)
        {
            var dicCustomerRoleDeclarantId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), DicCustomerRole.Codes.Declarant);
            var docIds = new List<int>();
            var documentIds = (from documentProtectionDoc in _niisContext.ProtectionDocDocuments
                join protectionDoc in _niisContext.ProtectionDocs on documentProtectionDoc.ProtectionDocId equals
                    protectionDoc.Id
                join protDocCustomer in _niisContext.ProtectionDocCustomers on protectionDoc.Id equals protDocCustomer
                    .ProtectionDocId
                join document in _niisContext.Documents on documentProtectionDoc.DocumentId equals document.Id
                join customer in _niisContext.DicCustomers on protDocCustomer.CustomerId equals customer.Id
                join aviaCorres in _niisContext.AvailabilityCorrespondences on protectionDoc.TypeId equals aviaCorres
                    .ProtectionDocTypeId
                where document.CurrentWorkflows.All(d => d.IsComplete == false) &&
                      aviaCorres.RouteStageId == document.CurrentWorkflows.First().CurrentStageId &&
                      aviaCorres.DocumentTypeId == docTypeId
                      && xin.Equals(customer.Xin, StringComparison.CurrentCultureIgnoreCase) &&
                      protectionDoc.TypeId == protectionDocTypeId &&
                      protDocCustomer.CustomerRoleId == dicCustomerRoleDeclarantId
                select new {documentProtectionDoc.DocumentId}).ToList();

            var documentIds2 = (from doc in _niisContext.Documents
                join docProtDoc in _niisContext.ProtectionDocDocuments on doc.Id equals docProtDoc.DocumentId
                join docCustomer in _niisContext.DocumentCustomers on doc.Id equals docCustomer.DocumentId
                join customer in _niisContext.DicCustomers on docCustomer.CustomerId equals customer.Id
                join aviaCorres in _niisContext.AvailabilityCorrespondences on doc.CurrentWorkflows.First().CurrentStageId equals
                    aviaCorres.RouteStageId
                where xin.Equals(customer.Xin, StringComparison.CurrentCultureIgnoreCase) &&
                      docCustomer.CustomerRoleId == dicCustomerRoleDeclarantId
                      && doc.CurrentWorkflows.All(d => d.IsComplete == false) && aviaCorres.ProtectionDocTypeId == protectionDocTypeId &&
                      aviaCorres.DocumentTypeId == docTypeId
                select new {doc.Id}).ToList();

            foreach (var item in documentIds)
                docIds.Add(item.DocumentId);
            foreach (var item in documentIds2)
                docIds.Add(item.Id);
            return docIds.Distinct().ToList();
        }

        private List<int> GetDocumentIdsForPayment(int docTypeId, int protectionDocTypeId, string xin)
        {
            var docIds = new List<int>();
            var dicCustomerRoleDeclarantId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), DicCustomerRole.Codes.Declarant);           
            var documentIds = (from documentProtectionDoc in _niisContext.ProtectionDocDocuments
                               join protectionDoc in _niisContext.ProtectionDocs on documentProtectionDoc.ProtectionDocId equals
                    protectionDoc.Id
                join protDocCustomer in _niisContext.ProtectionDocCustomers on protectionDoc.Id equals protDocCustomer
                    .ProtectionDocId
                join customer in _niisContext.DicCustomers on protDocCustomer.CustomerId equals customer.Id
                join linkDocument in _niisContext.DocumentDocumentRelations on documentProtectionDoc.DocumentId equals
                    linkDocument.ParentId
                where xin.Equals(customer.Xin, StringComparison.CurrentCultureIgnoreCase) &&
                      protDocCustomer.CustomerRoleId == dicCustomerRoleDeclarantId
                      && protectionDoc.TypeId == protectionDocTypeId && linkDocument.Child.TypeId == docTypeId
                select new {documentProtectionDoc.DocumentId}).ToList();

            var documentIds2 = (from docRequest in _niisContext.RequestsDocuments
                join request in _niisContext.Requests on docRequest.RequestId equals request.Id
                join document in _niisContext.Documents on docRequest.DocumentId equals document.Id
                join linkDoc in _niisContext.DocumentDocumentRelations on document.Id equals linkDoc.ParentId
                join docCustomer in _niisContext.DocumentCustomers on document.Id equals docCustomer.DocumentId
                join customer in _niisContext.DicCustomers on docCustomer.CustomerId equals customer.Id
                where request.ProtectionDocTypeId == protectionDocTypeId &&
                      docCustomer.CustomerRoleId == dicCustomerRoleDeclarantId
                      && xin.Equals(customer.Xin, StringComparison.CurrentCultureIgnoreCase) &&
                      linkDoc.Child.TypeId == docTypeId
                select new {document.Id}).ToList();

            foreach (var item in documentIds)
                docIds.Add(item.DocumentId);
            foreach (var item in documentIds2)
                docIds.Add(item.Id);
            return docIds.Distinct().ToList();
        }

        private List<RequisitionInfo> GetRequisitionInfos(dynamic docsInfo)
        {
            var requisitionInfos = new List<RequisitionInfo>();
            foreach (var item in docsInfo)
            {
                var docStatus = new RefKey {Note = item.StatusName,UID = item.OnlineRequisitionStatusId };
                requisitionInfos.Add(new RequisitionInfo
                {
                    DocumentID = item.Barcode,
                    DocumentDate = item.DocumentDate.Date,
                    StatusDate = item.StatusDate.Date,
                    DocumentStatus = docStatus
                });
            }
            return requisitionInfos;
        }
    }
}