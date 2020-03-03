using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    ///     Механизм применения бизнес-логики этапов при генерации номера исходящего документа
    /// </summary>
    public interface IGeneratedNumberApplier<T>
    {
        Task ApplyAsync(params int[] documentsIds);
    }
}