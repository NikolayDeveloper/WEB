using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.DocumentApplier
{
    public class DocumentApplier : IDocumentApplier<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly Func<StageLogic> _logicFactory;

        public DocumentApplier(NiisWebContext context, Func<StageLogic> logicFactory)
        {
            _context = context;
            _logicFactory = logicFactory;
        }

        public async Task ApplyAsync(params int[] documentsIds)
        {
            var contractDocuments = await _context.ContractsDocuments
                .Include(cd => cd.Contract).ThenInclude(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(cd => cd.Contract).ThenInclude(r => r.ProtectionDocType)
                .Include(cd => cd.Contract).ThenInclude(r => r.Documents).ThenInclude(cd => cd.Document).ThenInclude(d => d.Type)
                .Include(cd => cd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(cd => cd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(cd => cd.Contract).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(cd => cd.Contract).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(cd => cd.Document).ThenInclude(d => d.Type)
                .Where(cd => documentsIds.Contains(cd.DocumentId))
                .ToListAsync();

            var tasks = contractDocuments
                .Select(cd => _logicFactory().ApplyAsync(cd))
                .ToArray();

            Task.WaitAll(tasks);

            await _context.SaveChangesAsync();
        }
    }
}