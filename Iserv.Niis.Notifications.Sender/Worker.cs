using System.Threading;
using Autofac;
using Iserv.Niis.Notifications.Abstract;

namespace Iserv.Niis.Notifications.Sender
{
    public class Worker
    {
        private readonly IContainer _container;
        private readonly Timer _timer;
        private const int TimerPeriod = 60000;

        public Worker(IContainer container)
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
            var notificationResolver = _container.Resolve<INotificationTaskResolver>();
            notificationResolver.Resolve();
        }
    }
}
