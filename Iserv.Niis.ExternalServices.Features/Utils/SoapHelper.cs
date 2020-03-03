using System.IO;
using IntegrationSoapReader;
using Iserv.Niis.ExternalServices.Features.Models;

namespace Iserv.Niis.ExternalServices.Features.Utils
{
    public class SoapHelper
    {
        private readonly FileLoggerSettings _settings;

        public SoapHelper(
            AppConfiguration configuration
        )
        {
            _settings = new FileLoggerSettings {FolderBase = configuration.LogXmlDir, FolderDepth = DatePart.Hour};
        }

        public void LogSoap(MemoryStream memoryStream)
        {
            FileLogger.LogSOAP(memoryStream, _settings);
        }
    }
}