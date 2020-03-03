using System.Threading.Tasks;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class CreateLogRecordCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(LogRecord log)
        {
            var logRecordRepo = Uow.GetRepository<LogRecord>();
			log.UserId = NiisAmbientContext.Current.User.Identity.UserId;

			await logRecordRepo.CreateAsync(log);

            await Uow.SaveChangesAsync();

            return log.Id;
        }
    }
}
