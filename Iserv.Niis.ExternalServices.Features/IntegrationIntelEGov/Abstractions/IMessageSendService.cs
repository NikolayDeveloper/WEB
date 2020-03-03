using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    /// <summary>
    /// Прием материала(переписка)
    /// </summary>
    public interface IMessageSendService
    {
        /// <summary>
        /// Проверяет наличие платежного материала и что документ не платежный документ
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        bool HasFileAndNotPayment(MessageSendArgument argument);

        /// <summary>
        /// Принятие материала
        /// </summary>
        /// <param name="argument">метаданные материала</param>
        /// <returns>Материал</returns>
        Document CorrespondenceAdd(MessageSendArgument argument);

        /// <summary>
        /// Принятие материала
        /// </summary>
        /// <param name="argument">метаданные материала</param>
        /// <param name="documentId">Id родительского документа</param>
        /// <returns>Материал</returns>
        Document PaymentDocumentAdd(MessageSendArgument argument, int documentId);
    }
}