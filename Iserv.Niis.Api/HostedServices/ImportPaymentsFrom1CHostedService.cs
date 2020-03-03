using FluentScheduler;
using Iserv.Niis.Business.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Iserv.Niis.Api.HostedServices
{
    public class ImportPaymentsFrom1CHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IImportPaymentsFrom1CService _importService;

        public ImportPaymentsFrom1CHostedService(IConfiguration configuration, IImportPaymentsFrom1CService importService)
        {
            _configuration = configuration;
            _importService = importService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            var options = _configuration.GetSection("IntegrationWith1COptions");
            var workingDaysBeforeImport = options.GetValue<int>("WorkingDaysBeforeImport");
            var importTime = options.GetValue<DateTimeOffset>("ImportTime");

            var registry = new Registry();
            // Uncomment to run immediately after application starts
            /* 
            registry.Schedule(() => { DoImportPayments(new DateTimeOffset(new DateTime(2018, 11, 17)), 0); })
                .ToRunOnceIn(1).Seconds();
            */
            registry.Schedule(() => { DoImportPayments(DateTimeOffset.Now, workingDaysBeforeImport); })
                .ToRunEvery(1).Days().At(importTime.Date.Hour, importTime.Date.Minute);

            JobManager.Initialize(registry);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            JobManager.StopAndBlock();

            return Task.CompletedTask;
        }

        private void DoImportPayments(DateTimeOffset importDate, int workingDaysBeforeImport)
        {
            var dateOf1CPayments = _importService.CalculateDateOf1CPaymentsToImport(importDate, workingDaysBeforeImport);
            Task.Run(() => _importService.ImportPaymentsAsync(dateOf1CPayments, dateOf1CPayments)).Wait();
        }
    }
}