using System.ServiceProcess;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;

namespace Iserv.Niis.ExternalServices.StatusSender.Host
{
    public partial class IntelStatusSenderService : ServiceBase
    {
        public const string Name = "IntegrationIntelStatusSender";
        private readonly IWinServiceStatusSender _winService;

        public IntelStatusSenderService(IWinServiceStatusSender winService)
        {
            _winService = winService;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _winService.Start();
        }

        protected override void OnStop()
        {
            _winService.Stop();
        }
    }
}