namespace Iserv.Niis.WorkflowBusinessLogic
{
    public abstract class WorkflowRequest<TEntity> where TEntity : class, new()
    {
        public TEntity CurrentWorkflowObject { get; set; }
        public bool IsAuto { get; set; }
    }
}
