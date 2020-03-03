using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Механизм регистрации отложенных задач бизнес-логики этапов
    /// </summary>
    public interface ITaskRegister<T>
    {
        /// <summary>
        /// Процедура регистрации отложенных задач бизнес-логики этапов
        /// </summary>
        /// <param name="requestId">Идентификатор заявки</param>
        /// <returns></returns>
        Task RegisterAsync(int requestId);
    }
}