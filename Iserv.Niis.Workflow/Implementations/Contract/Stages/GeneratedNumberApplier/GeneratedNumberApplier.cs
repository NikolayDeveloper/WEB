using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.GeneratedNumberApplier
{
    public class GeneratedNumberApplier : IGeneratedNumberApplier<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly Func<StageLogic> _logicFactory;

        public GeneratedNumberApplier(NiisWebContext context, Func<StageLogic> logicFactory)
        {
            _context = context;
            _logicFactory = logicFactory;
        }

        public async Task ApplyAsync(params int[] documentsIds)
        {
            var contractDocuments = await _context.ContractsDocuments
                .Include(rd => rd.Contract).ThenInclude(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(rd => rd.Contract).ThenInclude(r => r.ProtectionDocType)
                .Include(rd => rd.Contract).ThenInclude(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(rd => rd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(rd => rd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(rd => rd.Contract).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(rd => rd.Contract).ThenInclude(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(rd => rd.Document).ThenInclude(d => d.Type)
                .Where(rd => documentsIds.Contains(rd.DocumentId))
                .ToListAsync();

            var tasks = contractDocuments
                .Select(cd => _logicFactory().ApplyAsync(cd))
                .ToArray();

            Task.WaitAll(tasks);

            await _context.SaveChangesAsync();
        }
    }
}