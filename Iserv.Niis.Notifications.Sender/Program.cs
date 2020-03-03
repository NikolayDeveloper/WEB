using System;

namespace Iserv.Niis.Notifications.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.CloseInRunning();

            var container = AutofacConfig.RegisterDependencies();
            using (var scope = container.BeginLifetimeScope())
            {
                var updater = new Worker(container);
                updater.Start();

                Console.WriteLine("Working in background\nPress any key for exit");
                Console.ReadKey();
            }
        }
    }
}
