using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Workflow;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Применяет workflow к бизнес-объекту
    /// </summary>
    public interface IWorkflowApplier<T>
    {
        /// <summary>
        /// Метод применения workflow к бизнес-объекту
        /// </summary>
        /// <param name="workflow">Workflow</param>
        Task ApplyAsync(BaseWorkflow workflow);
        
        /// <summary>
        /// Применяет workflow к бизнес-объекту
        /// </summary>
        /// <param name="obj">Бизнес-объект</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task ApplyInitialAsync(T obj, int userId);
    }
}