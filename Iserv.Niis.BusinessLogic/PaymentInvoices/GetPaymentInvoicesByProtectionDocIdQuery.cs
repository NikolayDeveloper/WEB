using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос для получения списка счетов по идентификатору охранного документа (ОД).
    /// </summary>
    public class GetPaymentInvoicesByProtectionDocIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="protectionDocId">Идентификатор охранного документа (ОД).</param>
        /// <returns>Список выставленных счетов.</returns>
        public List<PaymentInvoice> Execute(int protectionDocId)
        {
            var protectionDocRepo = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = protectionDocRepo.GetById(protectionDocId);
            if(protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);

            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentInvoicesQuery = paymentInvoiceRepository
                .AsQueryable()
                .Include(pi => pi.ProtectionDoc).ThenInclude(c => c.Type)
                .Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(pi => pi.PaymentUses)
                .Include(pi => pi.Tariff)
                .Include(pi => pi.Status)
                .Include(pi => pi.CreateUser)
                .Where(pi => pi.ProtectionDocId == protectionDocId);

            var paymentInvoices = paymentInvoicesQuery.ToList();


            return paymentInvoices
                .OrderByDescending(pi => pi.DateCreate)
                .ToList();
        }
    }
}