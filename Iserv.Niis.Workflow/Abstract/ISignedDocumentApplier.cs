using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Механизм применения бизнес-логики этапов при подписании документов
    /// </summary>
    public interface ISignedDocumentApplier<T>
    {
        /// <summary>
        /// Процедура применения бизнес-логики этапов при подписании документа/документов
        /// </summary>
        /// <param name="documentsIds">Идентификаторы документов</param>
        Task ApplyAsync(int userId, params int[] documentsIds);
    }
}