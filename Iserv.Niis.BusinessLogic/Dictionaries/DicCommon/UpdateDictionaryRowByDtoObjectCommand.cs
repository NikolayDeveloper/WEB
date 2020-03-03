using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    public class UpdateDictionaryRowByDtoObjectCommand : BaseCommand
    {
        public async Task Execute(string entityName, dynamic dictionaryObject)
        {
            var repo = Uow.GetRepository();
            
            var entityClrType = repo.GetEntityClrType(entityName);
            dynamic entity = Activator.CreateInstance(entityClrType);

            repo.Update(dictionaryObject);

            await Uow.SaveChangesAsync();
        }
    }
}
