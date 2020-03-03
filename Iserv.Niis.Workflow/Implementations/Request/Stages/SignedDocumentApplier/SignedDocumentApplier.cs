using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier
{
    public class SignedDocumentApplier : ISignedDocumentApplier<Domain.Entities.Request.Request>
    {
        private readonly NiisWebContext _context;
        private readonly ILogicFactory _logicFactory;

        public SignedDocumentApplier(NiisWebContext context, ILogicFactory logicFactory)
        {
            _context = context;
            _logicFactory = logicFactory;
        }

        public async Task ApplyAsync(int userId, params int[] documentsIds)
        {
            var user = _context.Users
                .Include(u => u.Position)
                .Single(u => u.Id == userId);

            var requestDocuments = await _context.RequestsDocuments
                .Include(rd => rd.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(rd => rd.Request).ThenInclude(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(rd => rd.Request).ThenInclude(r => r.CurrentWorkflow).ThenInclude(r => r.FromStage)
                .Include(rd => rd.Document).ThenInclude(d => d.Type)
                .Where(rd => documentsIds.Contains(rd.DocumentId))
                .ToListAsync();

            var tasks = requestDocuments
                .Select(rd => _logicFactory.Create(rd.Request.ProtectionDocType.Code)?.ApplyAsync(user, rd) ?? Task.CompletedTask)
                .ToArray();

            Task.WaitAll(tasks);

            await _context.SaveChangesAsync();
        }
    }
}