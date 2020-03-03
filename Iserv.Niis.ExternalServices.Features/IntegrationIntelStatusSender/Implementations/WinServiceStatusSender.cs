using System;
using System.Threading;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations
{
    public class WinServiceStatusSender : IWinServiceStatusSender
    {
        private static Thread _thread;
        private readonly Configuration _configuration;
        private readonly ISession _session;

        public WinServiceStatusSender(
            Configuration configuration,
            ISession session)
        {
            _configuration = configuration;
            _session = session;
        }

        public void Start()
        {
            Log.Debug("Service started");
            StopThread();
            _thread = new Thread(ThreadMethod)
            {
                Name = "StatusSender",
                IsBackground = true
            };
            _thread.Start();
        }

        public void Stop()
        {
            Log.Debug("Service stopped");
            StopThread();
        }

        #region PrivateMethod

        private void ThreadMethod()
        {
            while (true)
            {
                Cycle();
                Thread.Sleep(TimeSpan.FromMinutes(_configuration.CheckPeriodInMinutes));
            }
        }

        private void Cycle()
        {
            try
            {
                _session.PerformSendDocument();
                _session.PerformSendStatuses();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        private void StopThread()
        {
            if (_thread != null && _thread.ThreadState == ThreadState.Running)
            {
                _thread.Abort();
                _thread = null;
            }
        }

        #endregion
    }
}