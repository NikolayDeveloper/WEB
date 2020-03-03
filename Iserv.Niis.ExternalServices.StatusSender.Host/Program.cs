using System.ServiceProcess;
using Autofac;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;
using Iserv.Niis.ExternalServices.StatusSender.Host.Container;
using Iserv.Niis.ExternalServices.StatusSender.Host.Utils;

namespace Iserv.Niis.ExternalServices.StatusSender.Host
{
    internal static class Program
    {
        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        private static void Main()
        {
            SetConfiguration();
            var container = AutofacContainerBuilder.BuildContainer();
            var winService = container.Resolve<IWinServiceStatusSender>();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IntelStatusSenderService(winService)
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void SetConfiguration()
        {
            SerilogConfig.Configuration();
        }
    }
}