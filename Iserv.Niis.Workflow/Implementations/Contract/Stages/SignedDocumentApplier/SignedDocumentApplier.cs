using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.SignedDocumentApplier
{
    public class SignedDocumentApplier : ISignedDocumentApplier<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly StageLogic _stageLogic;

        public SignedDocumentApplier(NiisWebContext context, StageLogic stageLogic)
        {
            _context = context;
            _stageLogic = stageLogic;
        }

        public async Task ApplyAsync(int userId, params int[] documentsIds)
        {
            var user = _context.Users
                .Include(u => u.Position)
                .Single(u => u.Id == userId);

            var contractDocuments = await _context.ContractsDocuments
                .Include(cd => cd.Contract).ThenInclude(r => r.ProtectionDocType)
                .Include(cd => cd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(cd => cd.Contract).ThenInclude(r => r.CurrentWorkflow).ThenInclude(r => r.FromStage)
                .Include(cd => cd.Document).ThenInclude(d => d.Type)
                .Where(cd => documentsIds.Contains(cd.DocumentId))
                .ToListAsync();

            var tasks = contractDocuments
                .Select(cd => _stageLogic.ApplyAsync(user, cd))
                .ToArray();

            Task.WaitAll(tasks);

            await _context.SaveChangesAsync();
        }
    }
}