namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    /// Механизм выполнения отложенных задач бизнес-логики этапов
    /// </summary>
    public interface ITaskResolver<T>
    {
        /// <summary>
        /// Процедура выполнения отложенных задач бизнес-логики этапов
        /// </summary>
        void Resolve();
    }
}
