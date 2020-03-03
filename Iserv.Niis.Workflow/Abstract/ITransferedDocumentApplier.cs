using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Механизм применения бизнес-логики этапов при передвижении документа по этапу
    /// </summary>
    public interface ITransferedDocumentApplier
    {
        /// <summary>
        /// Процедура применения бизнес-логики этапов при передвижении документа по этапу
        /// </summary>
        /// <param name="documentsIds">Идентификаторы документов</param>
        Task ApplyAsync(params int[] documentsIds);
    }
}