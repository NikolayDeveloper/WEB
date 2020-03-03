using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Models;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Integration.OneC.Model;

namespace Iserv.Niis.Business.Implementations
{
    public class ImportPaymentsFrom1CService : IImportPaymentsFrom1CService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IIntegrationOneCApiClient _integrationOneCApiClient;
        private const string KZT = "KZT";
        private const string AdvanceCode = "3510";

        public ImportPaymentsFrom1CService(
            IServiceScopeFactory serviceScopeFactory,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IIntegrationOneCApiClient integrationOneCApiClient)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _integrationOneCApiClient = integrationOneCApiClient;
        }

        public DateTimeOffset CalculateDateOf1CPaymentsToImport(DateTimeOffset importDate, int workingDaysBeforeImport)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var calendarProvider = serviceScope.ServiceProvider.GetRequiredService<ICalendarProvider>();

                var workingDaysCounter = 0;
                var dateOf1CPayments = importDate;

                while (workingDaysCounter != workingDaysBeforeImport + 1)
                {
                    dateOf1CPayments = dateOf1CPayments.AddDays(-1);

                    if (!calendarProvider.IsHoliday(dateOf1CPayments))
                    {
                        workingDaysCounter++;
                    }
                }

                return dateOf1CPayments;
            }
        }

        public async Task<int> ImportPaymentsAsync(
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            int? userId = null,
            string userName = null,
            string userPosition = null)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<NiisWebContext>();
                var logService = serviceScope.ServiceProvider.GetRequiredService<ILogRecordService>();
                var notDistributedStatus = context.DicPaymentStatuses.First(s => s.Code == DicPaymentStatusCodes.NotDistributed);

                LogRecord log;
                StringBuilder logMessage;

                var importedCounter = 0;

                var payments1c = await LoadPaymentsAsync(fromDate, toDate);

                foreach (var p1c in payments1c)
                {
                    var paymentExist = context.Payments.Any(p =>
                        p.Payment1CNumber == p1c.Payment1CNumber
                        && p.PayerBinOrInn == p1c.PayerBinOrInn
                        && p.Payer == p1c.Payer
                        && p.PayerRNN == p1c.PayerRNN
                        && p.Amount == p1c.Amount
                        && p.PaymentDate == p1c.PaymentDate);

                    if (paymentExist)
                    {
                        logMessage = new StringBuilder()
                            .Append($"Сбой сохранения платежа {DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss")}, ")
                            .Append("Дубликат, ")
                            .Append($"{p1c.Payment1CNumber}, ")
                            .Append($"{p1c.PayerBinOrInn}, ")
                            .Append($"{p1c.PayerRNN}, ")
                            .Append($"{p1c.Amount}, ")
                            .Append($"{p1c.PaymentDate?.ToString("dd.MM.yyyy")}, ")
                            .Append($"{p1c.PaymentCNumberBVU}. ")
                            .Append($"{p1c.PurposeDescription}");

                        log = new LogRecord
                        {
                            LogType = LogType.ImportFrom1C,
                            LogErrorType = LogErrorType.Warning,
                            Message = logMessage.ToString()
                        };

                        logService.Log(log);

                        continue;
                    }

                    var customer = GetCustomerBy(context, p1c.PayerBinOrInn, p1c.PayerRNN);
                    if (customer == null)
                    {
                        customer = new DicCustomer
                        {
                            NameRuLong = p1c.Payer,
                            Xin = p1c.PayerBinOrInn,
                            Rnn = p1c.PayerRNN,
                            TypeId = GetCustomerTypeId(p1c.CustomerType)
                        };

                        context.DicCustomers.Add(customer);
                    }

                    var payment = new Payment
                    {
                        Payment1CNumber = p1c.Payment1CNumber,
                        PurposeDescription = p1c.PurposeDescription,
                        Payer = p1c.Payer,
                        Amount = p1c.Amount,
                        PaymentCNumberBVU = p1c.PaymentCNumberBVU,
                        PaymentDate = p1c.PaymentDate,
                        PayerBinOrInn = p1c.PayerBinOrInn,
                        PayerRNN = p1c.PayerRNN,
                        IsForeignCurrency = p1c.CurrencyType != KZT,
                        CurrencyType = p1c.CurrencyType,
                        CustomerId = customer.Id,
                        DateCreate = DateTimeOffset.Now,
                        ImportedDate = DateTimeOffset.Now,
                        UserImportedId = userId,
                        UserNameImported = userName,
                        UserPositionImported = userPosition,
                        PaymentStatusId = notDistributedStatus.Id,
                        IsAdvancePayment = p1c.IsAdvancePayment
                    };

                    context.Payments.Add(payment);
                    context.SaveChanges();

                    logMessage = new StringBuilder()
                        .Append($"Платеж успешно сохранен {DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss")}, ")
                        .Append($"{p1c.Payment1CNumber}, ")
                        .Append($"{p1c.PaymentCNumberBVU}. ")
                        .Append($"{p1c.PurposeDescription}, ")
                        .Append($"{p1c.PayerBinOrInn}, ")
                        .Append($"{p1c.PayerRNN}, ")
                        .Append($"{p1c.Amount}, ")
                        .Append($"{p1c.PaymentDate?.ToString("dd.MM.yyyy")}");

                    log = new LogRecord
                    {
                        LogType = LogType.ImportFrom1C,
                        LogErrorType = LogErrorType.Information,
                        Message = logMessage.ToString()
                    };

                    logService.Log(log);

                    importedCounter++;
                }

                return importedCounter;
            }
        }

        /// <summary>
        /// Получает платежи в диапазоне дат "с" .. "по".
        /// </summary>
        /// <param name="fromDate">Дата "с".</param>
        /// <param name="toDate">Дата "по".</param>
        /// <returns></returns>
        private async Task<List<PaymentFrom1CDto>> LoadPaymentsAsync(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var logRecordService = serviceScope.ServiceProvider.GetRequiredService<ILogRecordService>();

                var paymentsByDateRange = await _integrationOneCApiClient.GetPaymentsByDateRange(fromDate, toDate);

                if (paymentsByDateRange.IsSuccess)
                    return paymentsByDateRange.Data;

                if (paymentsByDateRange.Exception.Type != typeof(ComException).ToString())
                {
                    throw new Exception(paymentsByDateRange.Exception.Message);
                }

                var comException = (ComExceptionType)paymentsByDateRange.Exception.HResult;

                switch (comException)
                {
                    case ComExceptionType.CannotCreateComConnectorInstance:
                        LogExceprionCannotCreateComConnectorInstance(logRecordService);
                        break;
                    case ComExceptionType.CannotConnectTo1CDatabase:
                        LogExceprionCannotConnectTo1CDatabase(logRecordService);
                        break;
                    case ComExceptionType.UnknownComError:
                        LogExceprionUnknownComError(logRecordService);
                        break;
                }

                return new List<PaymentFrom1CDto>();
            }
        }

        #region Логируем ошибки
        #region Логируем ошибку "CannotCreateComConnectorInstance".
        /// <summary>
        /// Логируем ошибку "CannotCreateComConnectorInstance".
        /// </summary>
        /// <param name="logRecordService">Сервис логирования.</param>
        private void LogExceprionCannotCreateComConnectorInstance(ILogRecordService logRecordService)
        {
            var logMessage = new StringBuilder()
                .Append($"Ошибка подключения к БД 1С {DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss")}, ")
                .Append("Не удалось создать экземпляр com connector");

            var logRecord = new LogRecord
            {
                LogType = LogType.ImportFrom1C,
                LogErrorType = LogErrorType.Error,
                Message = logMessage.ToString()
            };

            logRecordService.Log(logRecord);

            throw new ComException(ComExceptionType.CannotCreateComConnectorInstance, "Cannot create com connector instance.");
        }
        #endregion

        #region Логируем ошибку "CannotConnectTo1CDatabase".
        /// <summary>
        /// Логируем ошибку "CannotConnectTo1CDatabase".
        /// </summary>
        /// <param name="logRecordService">Сервис логирования.</param>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        private void LogExceprionCannotConnectTo1CDatabase(ILogRecordService logRecordService)
        {
            var logMessage = new StringBuilder()
                .Append($"Ошибка подключения к БД 1С {DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss")}, ")
                .Append("Не удалось подключиться к БД 1С.");

            var logRecord = new LogRecord
            {
                LogType = LogType.ImportFrom1C,
                LogErrorType = LogErrorType.Error,
                Message = logMessage.ToString()
            };

            logRecordService.Log(logRecord);

            throw new ComException(ComExceptionType.CannotConnectTo1CDatabase, "Cannot connect to 1C database.");
        }
        #endregion

        #region Логируем ошибку "UnknownComError".
        /// <summary>
        /// Логируем ошибку "UnknownComError".
        /// </summary>
        /// <param name="logRecordService">Сервис логирования.</param>
        private void LogExceprionUnknownComError(ILogRecordService logRecordService)
        {
            var logMessage = new StringBuilder()
                .Append($"Ошибка подключения к БД 1С {DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss")}, ")
                .Append("Неизвестная ошибка, связанная с com соединением");

            var logRecord = new LogRecord
            {
                LogType = LogType.ImportFrom1C,
                LogErrorType = LogErrorType.Error,
                Message = logMessage.ToString()
            };

            logRecordService.Log(logRecord);

            throw new ComException(ComExceptionType.UnknownComError, "Unknown com-related error occured.");
        }
        #endregion
        #endregion

        private int GetCustomerTypeId(string cusType)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var dictionaryHelper = serviceScope.ServiceProvider.GetRequiredService<DictionaryHelper>();

                if (cusType.Equals("ЮрЛицо", StringComparison.OrdinalIgnoreCase))
                {
                    return dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), DicCustomerTypeCodes.Juridical);
                }

                if (cusType.Equals("ФизЛицо", StringComparison.OrdinalIgnoreCase))
                {
                    return dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), DicCustomerTypeCodes.Physical);
                }

                return dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), DicCustomerTypeCodes.Undefined);
            }
        }

        private DicCustomer GetCustomerBy(NiisWebContext context, string xin, string rnn)
        {
            DicCustomer customer = null;

            if (string.IsNullOrWhiteSpace(xin))
            {
                customer = context.DicCustomers.FirstOrDefault(c => c.Xin == xin);
            }

            if (customer != null)
            {
                return customer;
            }

            if (!string.IsNullOrWhiteSpace(rnn))
            {
                customer = context.DicCustomers.FirstOrDefault(c => c.Rnn == rnn);
            }

            return customer;
        }

        private string GetConnectionString()
        {
            var options = _configuration.GetSection("IntegrationWith1COptions");
            var usr = options.GetSection("Usr").Value;
            var pwd = options.GetSection("Pwd").Value;

            //if (_hostingEnvironment.IsDevelopment())
            //{
            var file = options.GetSection("File").Value;

            return $"File={file};Usr={usr};Pwd={pwd}";
            //}
            //else
            //{
            //    var srvr = options.GetSection("Srvr").Value;
            //    var _ref = options.GetSection("Ref").Value;

            //    return $"Srvr={srvr};Ref={_ref};Usr={usr};Pwd={pwd}";
            //}
        }

        private void ReleaseComObjects(List<dynamic> comObjects)
        {
            foreach (var comObject in comObjects)
            {
                if (comObject != null)
                {
                    Marshal.ReleaseComObject(comObject);
                }
            }
        }
    }
}