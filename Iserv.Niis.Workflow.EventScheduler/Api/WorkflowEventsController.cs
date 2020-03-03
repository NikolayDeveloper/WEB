using Iserv.Niis.Workflow.EventScheduler.WorkflowEventSchedulerImpl;
using System.Web.Http;

namespace Iserv.Niis.Workflow.EventScheduler.Api
{
    public class WorkflowEventsController : ApiController
    {
        [HttpGet]
        public string ExistedEvents()
        {
            return "Test";
        }

        [HttpPost]
        public bool StartEvent()
        {
            return true;
        }

        [HttpDelete]
        public bool RemoveEvent(string workflowEventKey)
        {
            WorkflowAutoEvents.RemoveEvent(workflowEventKey);
            return true;
        }
    }
}