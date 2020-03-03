using System;
using System.Configuration;
using Autofac;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Host.Constants;
using Iserv.Niis.ExternalServices.Host.Utils;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class OptionsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(GetEgovConfiguration())
                .As<AppConfiguration>();
        }

        private AppConfiguration GetEgovConfiguration()
        {
            return new AppConfiguration
            {
                LogXmlDir = ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.LogXmlDir),
                ServerPepIp = ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.ServerPepIp),
                AuthorAttachmentDocumentId =
                    Convert.ToInt32(
                        ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.AuthorAttachmentDocumentId)),
                ConStringNiisIntegration =
                    ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.ConStringNiisIntegration),
                ConStringNiisWeb = ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.ConStringNiisWeb),
                FolderRequisitionFile =
                    ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.FolderRequisitionFile),
                UrlServiceKazPatent =
                    ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.UrlServiceKazPatent),
                HashPassword = ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.HashPassword),
                MainExecutorIds = AppSettingsHelper.GetMainExecutorIds(ConfigurationManager.AppSettings.Get(AppSettings.EgovConfiguration.MainExecutorIds)),
            };
        }
    }
}