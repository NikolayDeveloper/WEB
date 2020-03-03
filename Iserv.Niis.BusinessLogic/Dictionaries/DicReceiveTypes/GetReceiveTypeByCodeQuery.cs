using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicReceiveTypes
{
    public class GetReceiveTypeByCodeQuery : BaseQuery
    {
        public DicReceiveType Execute(string code)
        {
            var repository = Uow.GetRepository<DicReceiveType>();

            var receiveType = repository.AsQueryable().FirstOrDefault(t => t.Code.Equals(code));

            if (receiveType == null)
                throw new DataNotFoundException(nameof(DicDepartment), DataNotFoundException.OperationType.Read, code);

            return receiveType;
        } 
    }
}