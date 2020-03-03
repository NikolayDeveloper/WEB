using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models;
using Iserv.Niis.ExternalServices.StatusSender.Host.Constants;
using Configuration = Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models.Configuration;

namespace Iserv.Niis.ExternalServices.StatusSender.Host.Container.Modules
{
    public class OptionsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(GetConfiguration())
                .As<Configuration>();
        }
        private Configuration GetConfiguration()
        {
            return new Configuration
            {
                ConStringNiisIntegration = ConfigurationManager.AppSettings[AppSettings.ConStringNiisIntegration],
                ConStringNiisWeb = ConfigurationManager.AppSettings[AppSettings.ConStringNiisWeb],
                CheckPeriodInMinutes = Convert.ToDouble(ConfigurationManager.AppSettings[AppSettings.CheckPeriodInMinutes]),
                DocWaitTimeInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettings.DocWaitTimeInMinutes]),
                KazPatentWebServiceUrl = ConfigurationManager.AppSettings[AppSettings.KazPatentWebServiceUrl],
                PepUrl = ConfigurationManager.AppSettings[AppSettings.PepUrl],
                RequestTimeoutInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettings.RequestTimeoutInMinutes]),
                LogXmlDir = ConfigurationManager.AppSettings[AppSettings.LogXmlDir]
            };
        }
    }
}
