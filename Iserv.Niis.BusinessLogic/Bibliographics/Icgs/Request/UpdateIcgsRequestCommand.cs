using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request
{
    public class UpdateIcgsRequestCommand: BaseCommand
    {
        public async Task ExecuteAsync(ICGSRequest icgs)
        {
            var repo = Uow.GetRepository<ICGSRequest>();
            repo.Update(icgs);

            await Uow.SaveChangesAsync();
        }
    }
}
