using System.Threading.Tasks;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Механизм определения категории договора по странам сторон договора
    /// </summary>
    public interface IContractCategoryIdentifier
    {
        /// <summary>
        /// Процедура определения категории договора по странам сторон договора
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        Task IdentifyAsync(int? contractId);
    }
}