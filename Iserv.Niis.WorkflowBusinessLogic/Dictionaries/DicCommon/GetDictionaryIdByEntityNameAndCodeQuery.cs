using System;
using System.Linq;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS.Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicCommon
{
    public class GetDictionaryIdByEntityNameAndCodeQuery : BaseQuery
    {
        public int? Execute(string entityName, string code)
        {
            var repo = Uow.GetRepository();

            var entityClrType = repo.GetEntityClrType(entityName);
            dynamic entity = Activator.CreateInstance(entityClrType);

            IQueryable dicData = repo.AsQueriable(entity);

            return dicData.Cast<DictionaryEntity<int>>().FirstOrDefault(d => d.IsDeleted == false && d.Code == code)?.Id;
        }
    }
}
