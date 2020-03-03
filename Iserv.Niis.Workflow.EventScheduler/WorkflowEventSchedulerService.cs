using System;
using Iserv.Niis.Workflow.EventScheduler.WorkflowEventSchedulerImpl;
using Microsoft.Owin.Hosting;

namespace Iserv.Niis.Workflow.EventScheduler
{
    public class WorkflowEventSchedulerService
    {
        private IDisposable _app;

        public void OnStart()
        {
            _app = WebApp.Start<Startup>("http://localhost:8085");
        }

        public void OnStop()
        {
            WorkflowAutoEvents.StopAllEvents();
            _app?.Dispose();
        }
    }
}
