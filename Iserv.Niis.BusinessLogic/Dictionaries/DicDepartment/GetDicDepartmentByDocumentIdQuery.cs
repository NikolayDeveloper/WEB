using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDepartment
{
    public class GetDicDepartmentByDocumentIdQuery: BaseQuery
    {
        public async Task<Domain.Entities.Dictionaries.DicDepartment> ExecuteAsync(int documentId)
        {
            var documentRepo = Uow.GetRepository<Document>();
            var document = await documentRepo.GetByIdAsync(documentId);
            if(document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, documentId);

            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDepartment>();
            var department = await repo.AsQueryable()
                .Include(d => d.Division)
                .FirstOrDefaultAsync(d => document.DepartmentId == d.Id);

            return department;
        }
    }
}
