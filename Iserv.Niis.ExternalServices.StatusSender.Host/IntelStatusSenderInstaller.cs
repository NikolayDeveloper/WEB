using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using Autofac;

namespace Iserv.Niis.ExternalServices.StatusSender.Host
{
    [RunInstaller(true)]
    public partial class IntelStatusSenderInstaller : Installer
    {
        public IntelStatusSenderInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            serviceInstaller.DisplayName = "Интеграция - Отправитель статусов";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = IntelStatusSenderService.Name;
            serviceInstaller.Description = "Передача статусов от НИИС";

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
            InitializeComponent();
        }
    }
}