using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iserv.Niis.Notifications.Sender
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
