using System;
using System.Linq;
using System.Threading;
using Autofac;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Scheduler
{
    internal class SchedulerWorker
    {
        private readonly IContainer _container;
        private readonly Timer _timer;
        private const int TimerPeriod = 1000;

        public SchedulerWorker(IContainer container)
        {
            _container = container;
            _timer = new Timer(Callback, null, Timeout.Infinite, TimerPeriod);
        }

        public void Start()
        {
            _timer.Change(0, TimerPeriod);
        }

        private void Callback(object state)
        {
            _timer.Change(Timeout.Infinite, TimerPeriod);
            var requestResolver = _container.Resolve<ITaskResolver<Request>>();
            //var contractResolver = _container.Resolve<ITaskResolver<Contract>>();
            requestResolver.Resolve();
            //contractResolver.Resolve();

            _timer.Change(0, TimerPeriod);
        }
    }
}