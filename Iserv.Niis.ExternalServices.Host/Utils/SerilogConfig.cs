using Serilog;

namespace Iserv.Niis.ExternalServices.Host.Utils
{
    public class SerilogConfig
    {
        public static void Configuration()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
        }
    }
}
