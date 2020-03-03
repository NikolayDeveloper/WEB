using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    /// <summary>
    /// Команда создания связи между заявкой и охранным документом.
    /// </summary>
    public class CreateRequestProtectionDocSimilarCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="requestProtectionDocSimilar">Связь между заявкой и охранным документом.</param>
        public void Execute(RequestProtectionDocSimilar requestProtectionDocSimilar)
        {
            var requestProtectionDocSimilarRepository = Uow.GetRepository<RequestProtectionDocSimilar>();

            requestProtectionDocSimilarRepository.Create(requestProtectionDocSimilar);

            Uow.SaveChanges();
        }
    }
}
