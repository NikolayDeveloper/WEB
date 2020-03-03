using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using NIIS.DBConverter.Entities.References;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisMainEntityRelationService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly NiisWebContextMigration _newContext;
        private readonly DictionaryTypesHelper _dictionaryTypesHelper;

        public OldNiisMainEntityRelationService(
            OldNiisContext context,
            NiisWebContextMigration newContext,
            DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _newContext = newContext;
            _dictionaryTypesHelper = dictionaryTypesHelper;
        }

        public List<ContractProtectionDocRelation> GetContractProtectionDocRelations(int packageSize, int lastId)
        {
            var contractTypeIds = _dictionaryTypesHelper.GetContractTypeIds();

            var contractProtectionDocRelations = _context.DDDocuments
                .AsNoTracking()
                .Where(d => d.Id > lastId
                            && contractTypeIds.Contains(d.DocTypeId)
                            && d.DocId > 0)
                .OrderBy(d => d.Id)
                .Take(packageSize)
                .ToList();

            return contractProtectionDocRelations.Select(d => new ContractProtectionDocRelation
            {
                ExternalId = d.Id,
                DateCreate = d.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = d.DateUpdate ?? DateTimeOffset.Now,
                ContractId = d.Id,
                ProtectionDocId = d.DocId.Value
            }).ToList();
        }

        public List<ProtectionDocDocument> GetProtectionDocDocumentRelations(int packageSize, int lastId)
        {
            var documentTypeIds = _dictionaryTypesHelper.GetDocumentTypeIds();

            var documentProtectionDocRelations = _context.DDDocuments
                .AsNoTracking()
                .Where(d => d.Id > lastId
                            && documentTypeIds.Contains(d.DocTypeId)
                            && d.DocId > 0)
                .OrderBy(d => d.Id)
                .Take(packageSize)
                .ToList();


            return documentProtectionDocRelations.Select(d => new ProtectionDocDocument
            {
                ExternalId = d.Id,
                DateCreate = d.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = d.DateUpdate ?? DateTimeOffset.Now,
                DocumentId = d.Id,
                ProtectionDocId = d.DocId.Value
            }).ToList();
        }

        public List<ContractDocument> GetContractDocumentRelations(int packageSize, int lastId)
        {
            packageSize /= 2;
            var contractTypeIds = _dictionaryTypesHelper.GetContractTypeIds();
            var documentTypeIds = _dictionaryTypesHelper.GetDocumentTypeIds();

            var contractDocumentRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && contractTypeIds.Contains(r.DDDocument.DocTypeId)
                            && documentTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();

            var documentContractRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && documentTypeIds.Contains(r.DDDocument.DocTypeId)
                            && contractTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();

            contractDocumentRelations.AddRange(documentContractRelations);

            var newItems = _newContext.Contracts.Where(d => contractDocumentRelations.Any(c => c.DocumentId.Value == d.Id)).Select(d => d.Id);

            return contractDocumentRelations.Where(d => newItems.Contains(d.DocumentId.Value)).Select(r => new ContractDocument
            {
                ExternalId = r.Id,
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.DateCreate ?? DateTimeOffset.Now,
                ContractId = r.DocumentId.Value,
                DocumentId = r.RefDocumentId
            }).ToList(); ;
        }

        public List<ContractRequestRelation> GetContractRequestRelations(int packageSize, int lastId)
        {
            packageSize /= 2;
            var contractTypeIds = _dictionaryTypesHelper.GetContractTypeIds();
            var requestTypeIds = _dictionaryTypesHelper.GetRequestTypeIds();

            var contractRequestRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && contractTypeIds.Contains(r.DDDocument.DocTypeId)
                            && requestTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();

            var requestContractRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && requestTypeIds.Contains(r.DDDocument.DocTypeId)
                            && contractTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();

            contractRequestRelations.AddRange(requestContractRelations);


            var newContract = _newContext.Contracts.Where(d => contractRequestRelations.Any(c => c.DocumentId.Value == d.Id)).Select(d => d.Id);

            return contractRequestRelations.Where(d => newContract.Contains(d.DocumentId.Value)).Select(r => new ContractRequestRelation
            {
                ExternalId = r.Id,
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.DateCreate ?? DateTimeOffset.Now,
                ContractId = r.DocumentId.Value,
                RequestId = r.RefDocumentId,
            }).ToList();
        }

        public List<RequestDocument> GetRequestDocumentRelations(int packageSize, int lastId)
        {
            packageSize /= 2;
            var requestTypeIds = _dictionaryTypesHelper.GetRequestTypeIds();
            var documentTypeIds = _dictionaryTypesHelper.GetDocumentTypeIds();

            
            var requestDocumentRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && r.RefDocumentId > 0
                            && requestTypeIds.Contains(r.DDDocument.DocTypeId)
                            && documentTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();


            var documentRequestRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.DocumentId > 0
                            && r.RefDocumentId > 0
                            && documentTypeIds.Contains(r.DDDocument.DocTypeId)
                            && requestTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .Select(dr => new RFMessageDocument
                {
                    Id = dr.Id,
                    DateCreate = dr.DateCreate,
                    IsAnswer = dr.IsAnswer,
                    DocumentId = dr.RefDocumentId,
                    RefDocumentId = dr.DocumentId.Value
                })
                .ToList();

            requestDocumentRelations.AddRange(documentRequestRelations);

            //var newDocument = _newContext.Documents.Where(d => requestDocumentRelations.Any(c => c.RefDocumentId == d.Id)).Select(d => d.Id).ToList();

            //return requestDocumentRelations.Where(d => newDocument.Contains(d.DocumentId.Value)).Select(r => new RequestDocument
            return requestDocumentRelations.Where(d => d.RefDocumentId > 0).Select(r => new RequestDocument
            {
                ExternalId = r.Id,
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.DateCreate ?? DateTimeOffset.Now,
                RequestId = r.DocumentId.Value,
                DocumentId = r.RefDocumentId
            }).ToList();
        }

        public List<DocumentDocumentRelation> GetDocumentDocumentRelations(int packageSize, int lastId)
        {
            var documentTypeIds = _dictionaryTypesHelper.GetDocumentTypeIds();

            var oldDocumentDocumentRelations = _context.RFMessageDocuments
                .AsNoTracking()
                .Where(r => r.Id > lastId
                            && r.RefDocumentId > 0
                            && r.DocumentId > 0
                            && documentTypeIds.Contains(r.DDDocument.DocTypeId)
                            && documentTypeIds.Contains(r.RefDDDocument.DocTypeId))
                .OrderBy(r => r.Id)
                .Take(packageSize)
                .ToList();

            var documentDocumentRelations = new List<DocumentDocumentRelation>();

            //var newContract = _newContext.Documents.Where(d => oldDocumentDocumentRelations.Any(c => c.DocumentId.Value == d.Id)).Select(d => d.Id);

            //foreach (var oldDocumentDocumentRelation in oldDocumentDocumentRelations.Where(d => newContract.Contains(d.DocumentId.Value)))
            foreach (var oldDocumentDocumentRelation in oldDocumentDocumentRelations)
            {
                if (!documentDocumentRelations.Any(r => r.ChildId == oldDocumentDocumentRelation.RefDocumentId && r.ParentId == oldDocumentDocumentRelation.DocumentId.Value))
                {
                    documentDocumentRelations.Add(new DocumentDocumentRelation
                    {
                        ExternalId = oldDocumentDocumentRelation.Id,
                        DateCreate = oldDocumentDocumentRelation.DateCreate ?? DateTimeOffset.Now,
                        DateUpdate = oldDocumentDocumentRelation.DateCreate ?? DateTimeOffset.Now,
                        ParentId = oldDocumentDocumentRelation.DocumentId.Value,
                        ChildId = oldDocumentDocumentRelation.RefDocumentId,
                        IsAnswer = CustomConverter.StringToNullableBool(oldDocumentDocumentRelation.IsAnswer)
                    });
                }
            }

            return documentDocumentRelations;

            /*return documentDocumentRelations.Select(r => new DocumentDocumentRelation
            {
                Id = r.Id,
                ExternalId = r.Id,
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.DateCreate ?? DateTimeOffset.Now,
                ParentId = r.DocumentId.Value,
                ChildId = r.RefDocumentId,
                IsAnswer = CustomConverter.StringToNullableBool(r.IsAnswer)
            }).ToList();*/
        }

        public List<ProtectionDocProtectionDocRelation> GetProtectionDocProtectionDocRelations(int packageSize, int lastId)
        {
            var protectionDocProtectionDocRelations = _context.RfPatPatDks
                .AsNoTracking()
                .Where(p => p.u_id > lastId)
                .OrderBy(p => p.u_id)
                .Take(packageSize)
                .ToList();

            return protectionDocProtectionDocRelations.Select(r => new ProtectionDocProtectionDocRelation
            {
                ExternalId = r.u_id,
                DateCreate = r.date_create,
                DateUpdate = r.date_create,
                ParentId = r.flParentDocId,
                ChildId = r.flChildDocId,
            }).ToList();
        }

    }
}
