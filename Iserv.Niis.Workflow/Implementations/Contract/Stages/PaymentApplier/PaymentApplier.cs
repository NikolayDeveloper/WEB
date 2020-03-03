using System;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.PaymentApplier
{
    public class PaymentApplier : IPaymentApplier<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly StageLogic _logic;

        public PaymentApplier(NiisWebContext context, StageLogic logic)
        {
            _context = context;
            _logic = logic;
        }

        public async Task ApplyAsync(int paymentUseId)
        {
            var paymentUse = await _context.PaymentUses
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.Documents)
                .ThenInclude(rd => rd.Document)
                .ThenInclude(d => d.Type)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.ProtectionDocType)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.CurrentWorkflow)
                .ThenInclude(cw => cw.CurrentStage)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.CurrentWorkflow)
                .ThenInclude(cw => cw.FromStage)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.Documents)
                .ThenInclude(rd => rd.Document)
                .ThenInclude(d => d.Type)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Contract)
                .ThenInclude(r => r.PaymentInvoices)
                .ThenInclude(pi => pi.Tariff)
                .Include(pu => pu.PaymentInvoice).ThenInclude(pi => pi.Tariff)
                .Include(pu => pu.PaymentInvoice).ThenInclude(pi => pi.Status)
                .SingleOrDefaultAsync(p => p.Id == paymentUseId);

            if (paymentUse.PaymentInvoice.Status.Code.Equals("notpaid")) return;

            var contract = paymentUse.PaymentInvoice.Contract;
            if (contract == null)
                throw new ApplicationException($"Payment with id: {paymentUse.PaymentId} hasn't contract");

            await _logic.ApplyAsync(paymentUse);

            await _context.SaveChangesAsync();
        }
    }
}