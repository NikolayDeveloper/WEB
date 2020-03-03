using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Механизм применения бизнес-логики этапов при создании документов
    /// </summary>
    public interface IDocumentApplier<T>
    {
        /// <summary>
        /// Процедура применения бизнес-логики этапов при создании документа/документов
        /// </summary>
        /// <param name="documentsIds">Идентификаторы документов</param>
        Task ApplyAsync(params int[] documentsIds);
    }
}