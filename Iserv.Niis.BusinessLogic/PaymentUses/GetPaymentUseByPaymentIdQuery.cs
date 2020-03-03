using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class GetPaymentUseByPaymentIdQuery : BaseQuery
    {
        public List<PaymentUse> Execute(int? paymentId)
        {
            if(paymentId.HasValue == false)
            {
                return new List<PaymentUse>();
            }

            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            var result = paymentUseRepository
                .AsQueryable()
                .Where(r=>r.PaymentId == paymentId)
                .Include(r => r.Request)
                .Include(r => r.ProtectionDoc)
                .Include(r => r.Contract)
                .Include(r => r.DicTariff)
                .Include(r => r.DicProtectionDocType)
                .Include(r => r.DicProtectionDocSubType)
                .ToList();

            return result;
        }
    }
}
