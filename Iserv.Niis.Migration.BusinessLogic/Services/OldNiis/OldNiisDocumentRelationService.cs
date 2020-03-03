using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisDocumentRelationService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesHelper;

        public OldNiisDocumentRelationService(
            OldNiisContext context, 
            DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesHelper = dictionaryTypesHelper;
        }

        public List<DocumentEarlyReg> GetDocumentEarlyRegs(List<int> documentIds)
        {
            var documentEarlyRegs = _context.WtPtEarlyRegs
                .AsNoTracking()
                .Where(re => documentIds.Contains(re.DOC_ID ?? 0) && re.ETYPE_ID != null)
                .OrderBy(re => re.U_ID)
                .ToList();

            var oldCountriesIds = documentEarlyRegs.Where(d => d.REQ_COUNTRY != null).Select(d => d.REQ_COUNTRY.GetValueOrDefault(0)).Distinct().ToList();
            var countries = _dictionaryTypesHelper.GetCountryIds(oldCountriesIds);

            return documentEarlyRegs.Select(de => new DocumentEarlyReg
            {
                Id = de.U_ID,
                DocumentId = de.DOC_ID.Value,
                EarlyRegTypeId = de.ETYPE_ID.Value,
                CountryId = countries.Any(d => d == de.REQ_COUNTRY) ? de.REQ_COUNTRY : null,
                RegNumber = de.REQ_NUMBER,
                NameSD = de.SA_NAME,
                RegDate = de.REQ_DATE
            }).ToList();
        }

        public List<DocumentCustomer> GetDocumentCustomers(List<int> documentIds)
        {
            var documentCustomers = _context.RfCustomers
                .AsNoTracking()
                .Where(rc => documentIds.Contains(rc.DocId))
                .OrderBy(rc => rc.Id)
                .ToList();

            return documentCustomers.Select(dc => new DocumentCustomer
            {
                Id = dc.Id,
                CustomerId = dc.CustomerId,
                CustomerRoleId = dc.CType,
                DocumentId = dc.DocId,
                AddressId = dc.AddressId,
                DateCreate = dc.DateCreate ?? DateTime.Now,
            }).ToList();
        }

        public List<DocumentExecutor> GetDocumentExecutors(List<int> documentIds)
        {
            var documentExecutors = _context.RFDocumentExecutors
                .AsNoTracking()
                .Where(de => de.DocumentId > 0 && documentIds.Contains(de.DocumentId))
                .ToList();

            return documentExecutors.Select(de => new DocumentExecutor
            {
                //TODO: УБРАЛ ID, Не переносится почему то 
                //Id = de.Id,
                DocumentId = de.DocumentId,
                UserId = de.UserId,
            }).ToList();
        }

        public DocumentUserSignature GetDocumentUserSignature(DocumentWorkflow documentWorkflow, List<int> ignoreSignaturies)
        {
            var documentUserSingrature = _context.DocumentUsersSignaturies
                .AsNoTracking()
                .Where(us => us.DocId == documentWorkflow.OwnerId && documentWorkflow.CurrentUserId == us.UserId && !ignoreSignaturies.Contains(us.Id))
                .FirstOrDefault();

            if (documentUserSingrature == null) return null;

            ignoreSignaturies.Add(documentUserSingrature.Id);

            return new DocumentUserSignature
            {
                //TODO: УБРАЛ ID, Не переносится почему то 
                //Id = documentUserSingrature.Id,
                UserId = documentUserSingrature.UserId,
                WorkflowId = documentWorkflow.Id,
                PlainData = documentUserSingrature.FingerPrint,
                SignedData = documentUserSingrature.SignedData,
                SignerCertificate = documentUserSingrature.SignerCertificate,
                DateCreate = documentUserSingrature.SignDate ?? DateTime.Now,
            };
        }

        public List<PaymentRegistryData> GetPaymentRegistryDatas(List<int> documentIds)
        {
            var paymentRegistryDatas = _context.DdPaymentData
                .AsNoTracking()
                .Where(d => documentIds.Contains(d.DocumentId ?? 0))
                .ToList();

            return paymentRegistryDatas.Select(p => new PaymentRegistryData
            {
                //TODO: УБРАЛ ID, Не переносится почему то 
                //Id = p.Id,
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                DocumentId = p.DocumentId,
                PaymentInvoiceId = p.FixPayId
            }).ToList();
        }
    }
}
