using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier
{
    public class TransferedDocumentApplier : ITransferedDocumentApplier
    {
        private readonly NiisWebContext _context;
        private readonly ILogicFactory _logicFactory;

        public TransferedDocumentApplier(NiisWebContext context, ILogicFactory logicFactory)
        {
            _context = context;
            _logicFactory = logicFactory;
        }

        public async Task ApplyAsync(params int[] documentsIds)
        {
            var requestDocuments = await _context.RequestsDocuments
                .Include(rd => rd.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(rd => rd.Request).ThenInclude(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(rd => rd.Request).ThenInclude(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(rd => rd.Request).ThenInclude(d => d.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(rd => rd.Request).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(rd => rd.Request).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(rd => rd.Request).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(rd => rd.Document).ThenInclude(d => d.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Where(rd => documentsIds.Contains(rd.DocumentId))
                .ToListAsync();

            var tasks = requestDocuments
                .Select(rd => _logicFactory.Create(rd.Request.ProtectionDocType.Code)?.ApplyAsync(rd) ?? Task.CompletedTask)
                .ToArray();

            Task.WaitAll(tasks);

            await _context.SaveChangesAsync();
        }
    }
}