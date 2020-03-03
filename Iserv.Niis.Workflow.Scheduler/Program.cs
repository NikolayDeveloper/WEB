using System;

namespace Iserv.Niis.Workflow.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.CloseInRunning();

            var container = AutofacConfig.RegisterDependencies();
            using (var scope = container.BeginLifetimeScope())
            {
                var updater = new SchedulerWorker(container);
                updater.Start();

                Console.WriteLine("Working in background\nPress any key for exit");
                Console.ReadKey();
            }
        }
    }
}
