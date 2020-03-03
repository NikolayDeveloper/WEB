using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
//using NetCoreDI;

namespace NetCoreRules
{
    public class BaseRule { }

    public abstract class BaseRule<TWorkflowRequest> : BaseRule
    {
        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = AmbientContext.Current.Resolver.ResolveObject<IExecutor>());

        protected TWorkflowRequest WorkflowRequest;

        public void SetWorkflowRequest(TWorkflowRequest workflowRequest)
        {
            WorkflowRequest = workflowRequest;
        }
    }
}
