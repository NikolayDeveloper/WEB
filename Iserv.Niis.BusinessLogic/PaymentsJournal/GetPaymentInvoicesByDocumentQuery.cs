using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetPaymentInvoicesByDocumentQuery : BaseQuery
    {
        public IList<PaymentInvoiceDto> Execute(int documentId, DocumentCategory documentCategory)
        {
            return GetQuery(documentId, documentCategory)
                .Select(PaymentInvoiceDto.FromPaymentInvoice)
                .ToList();
        }

        private IQueryable<PaymentInvoice> GetQuery(int documentId, DocumentCategory documentCategory)
        {
            switch (documentCategory)
            {
                case DocumentCategory.Request:
                    return GetQuery().Where(x => x.RequestId == documentId);

                case DocumentCategory.ProtectionDoc:
                    return GetQuery().Where(x => x.ProtectionDocId == documentId);

                case DocumentCategory.Contract:
                    return GetQuery().Where(x => x.ContractId == documentId);

                default:
                    return new List<PaymentInvoice>().AsQueryable();
            }
        }

        private IQueryable<PaymentInvoice> GetQuery()
        {
            return Uow.GetRepository<PaymentInvoice>()
                .AsQueryable()
                .AsNoTracking()
                .Include(x => x.Tariff)
                .Include(x => x.Status)
                .Include(x => x.PaymentUses);
        }
    }
}