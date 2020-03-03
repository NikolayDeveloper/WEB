using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Documents
{
    public class GetSignaturesByDocumentId: BaseQuery
    {
        public List<DocumentUserSignature> Execute(int documentId)
        {
            var signatureRepository = Uow.GetRepository<DocumentUserSignature>();
            var signatures = signatureRepository.AsQueryable()
                .Include(s => s.User).ThenInclude(u => u.Department)
                .Where(s => s.Workflow.OwnerId == documentId);

            return signatures.ToList();
        }
    }
}
