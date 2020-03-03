using System;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
    public class PaymentApplier : IPaymentApplier<Domain.Entities.Request.Request>
    {
        private readonly NiisWebContext _context;
        private readonly ILogicFactory _logicFactory;

        public PaymentApplier(NiisWebContext context, ILogicFactory logicFactory)
        {
            _context = context;
            _logicFactory = logicFactory;
        }

        public async Task ApplyAsync(int paymentUseId)
        {
            var paymentUse = await _context.PaymentUses
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Request)
                .ThenInclude(r => r.Documents)
                .ThenInclude(rd => rd.Document)
                .ThenInclude(d => d.Type)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Request)
                .ThenInclude(r => r.ProtectionDocType)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Request)
                .ThenInclude(r => r.CurrentWorkflow)
                .ThenInclude(cw => cw.CurrentStage)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Request)
                .ThenInclude(r => r.CurrentWorkflow)
                .ThenInclude(cw => cw.FromStage)
                .Include(pu => pu.PaymentInvoice)
                .ThenInclude(pi => pi.Request)
                .ThenInclude(r => r.Documents)
                .ThenInclude(rd => rd.Document)
                .ThenInclude(d => d.Type)
	            .Include(pu => pu.PaymentInvoice)
	            .ThenInclude(pi => pi.Request)
	            .ThenInclude(r => r.PaymentInvoices)
				.ThenInclude(pi => pi.Tariff)
				.Include(pu => pu.PaymentInvoice).ThenInclude(pi => pi.Tariff)
                .Include(pu => pu.PaymentInvoice).ThenInclude(pi => pi.Status)
                .SingleOrDefaultAsync(p => p.Id == paymentUseId);

            if (paymentUse.PaymentInvoice.Status.Code.Equals("notpaid")) return;

            var request = paymentUse.PaymentInvoice.Request;
            if (request == null) throw new ApplicationException($"Payment with id: {paymentUse.PaymentId} hasn't request");

            var logic = _logicFactory.Create(paymentUse.PaymentInvoice.Request.ProtectionDocType.Code);
            if (logic != null) await logic.ApplyAsync(paymentUse);

            await _context.SaveChangesAsync();
        }
    }
}