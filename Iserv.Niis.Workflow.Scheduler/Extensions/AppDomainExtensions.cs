using System;
using System.Threading;

namespace Iserv.Niis.Workflow.Scheduler
{
    public static class AppDomainExtensions
    {
        private static Mutex Mutex { get; set; }

        public static void CloseInRunning(this AppDomain appDomain)
        {
            bool hasOwnerShip;
            Mutex = new Mutex(true, "Global\\" + AppDomain.CurrentDomain.FriendlyName, out hasOwnerShip);

            if (!hasOwnerShip)
            {
                Console.WriteLine("{0} already running", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("Press any key for exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}