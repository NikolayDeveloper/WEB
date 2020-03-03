using System;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.PaymentUses;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Domain.OldNiisEntities;
using Iserv.Niis.Services.Dapper;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Helpers;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Services.Implementations
{
    public class ImportPaymentsHelper : BaseImportHelper, IImportPaymentsHelper
    {
        #region Constructor
        
        public ImportPaymentsHelper(
            IConfiguration configuration, 
            DictionaryHelper dictionaryHelper) 
            : base(configuration, dictionaryHelper)
        {
        }
        
        #endregion
        
        public async Task ImportFromDb(string number, int requestId)
        {
            await FillPayments(number);

            await FillPaymentInvoices(number, requestId);
            await FillPaymentUse(number);
        }
        
        #region PaymentInvoices

        private async Task FillPaymentInvoices(string number, int requestId)
        {
            var paymentInvoicesSqlQuery = string.Format(ImportSqlQueryHelper.PaymentInvoicesSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var oldPaymentInvoices = await SqlDapperConnection.QueryAsync<WtPlFixpayment>(paymentInvoicesSqlQuery, TargetConnectionString);
            foreach(var oldPaymentInvoice in oldPaymentInvoices)
            {
                var paymentInvoice = CreatePaymentInvoice(oldPaymentInvoice, requestId);
                if (paymentInvoice == null) continue;
                await Executor.GetCommand<CreatePaymentInvoiceCommand>().Process(d => d.ExecuteAsync(paymentInvoice));
                
                paymentInvoice.DateCreate = new DateTimeOffset(oldPaymentInvoice.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(d => d.Execute(paymentInvoice));
            }
        }

        protected PaymentInvoice CreatePaymentInvoice(WtPlFixpayment oldPaymentInvoice, int requestId)
        {
            try
            {
                var tariffId = GetObjectId<DicTariff>(oldPaymentInvoice.TariffId);

                if (tariffId == null || tariffId == 0) return null;

                var paymentInvoice = new PaymentInvoice
                {
                    Coefficient = oldPaymentInvoice.FinePercent,
                    CreateUserId = GetUserId(oldPaymentInvoice.FlCreateUserId),
                    DateComplete = GetNullableDate(oldPaymentInvoice.DateComplete),
                    DateCreate = new DateTimeOffset(oldPaymentInvoice.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldPaymentInvoice.Stamp.GetValueOrDefault(DateTime.Now)),
                    DateFact = GetNullableDate(oldPaymentInvoice.DateFact),
                    ExternalId = oldPaymentInvoice.Id,
                    IsComplete = GenerateHelper.StringToNullableBool(oldPaymentInvoice.IsComplete),
                    Nds = oldPaymentInvoice.VatPercent,
                    OverdueDate = GetNullableDate(oldPaymentInvoice.DateLimit),
                    PenaltyPercent = oldPaymentInvoice.PeniPercent,
                    RequestId = requestId,
                    StatusId = GetStatusId(oldPaymentInvoice),
                    TariffCount = oldPaymentInvoice.TariffCount,
                    TariffId = tariffId.Value,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    IsDeleted = false
                };

                return paymentInvoice;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Получить статус платежа
        /// </summary>
        /// <param name="oldPaymentInvoice"></param>
        /// <returns></returns>
        private int GetStatusId(WtPlFixpayment oldPaymentInvoice)
        {
            var dateComplete = oldPaymentInvoice.DateComplete;
            var fixPayRemainder = oldPaymentInvoice.FixPayRemainder;

            var status = 1;

            if (dateComplete.HasValue)
                status = 3;

            if (fixPayRemainder < 100)
                status = 2;

            return status;
        }

        #endregion

        #region Payments

        private async Task FillPayments(string number)
        {
            var paymentsSqlQuery = string.Format(ImportSqlQueryHelper.PaymentsSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var oldPayments = await SqlDapperConnection.QueryAsync<WtPlPayment>(paymentsSqlQuery, TargetConnectionString);
            foreach (var oldPayment in oldPayments)
            {
                var payment = await CreatePaymen(oldPayment);
                if (payment == null) continue;

                var result = Executor.GetCommand<CheckExistPaymentCommand>().Process(d => d.Execute(payment));
                if (result) continue;

                await Executor.GetCommand<CreatePaymentCommand>().Process(d => d.ExecuteAsync(payment));

                payment.DateCreate = new DateTimeOffset(oldPayment.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdatePaymentCommand>().Process(d => d.Execute(payment));
            }
        }

        private async Task<Payment> CreatePaymen(WtPlPayment oldPayment)
        {
            var customerId = GetObjectId<DicCustomer>(oldPayment.CustomerId);

            if ((customerId == null || customerId == 0) && oldPayment.CustomerId.HasValue)
                customerId = await GetCustomer(oldPayment.CustomerId.Value);

            if (customerId == null || customerId == 0)
                return null;

            try
            {
                var payment = new Payment
                {
                    Amount = oldPayment.PaymentAmount,
                    AssignmentDescription = oldPayment.UseDsc,
                    CurrencyAmount = oldPayment.FlValSum,
                    CurrencyRate = oldPayment.FlExchangeRate,
                    CurrencyType = oldPayment.FlValType,
                    CustomerId = customerId,
                    DateCreate = new DateTimeOffset(oldPayment.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldPayment.DateCreate.GetValueOrDefault(DateTime.Now)),
                    ExternalId = oldPayment.Id,
                    IsPrePayment = GenerateHelper.StringToNullableBool(oldPayment.IsAvans),
                    Payment1CNumber = oldPayment.PaymentNumb,
                    PaymentDate = GetNullableDate(oldPayment.PaymentDate),
                    PaymentNumber = oldPayment.PaymentType,
                    PurposeDescription = oldPayment.Dsc,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    IsForeignCurrency = false,
                    IsAdvancePayment = false
                };

                return payment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region PaymentUses

        private async Task FillPaymentUse(string number)
        {
            var paymentUsesSqlQuery = string.Format(ImportSqlQueryHelper.PaymentUses, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var oldPaymentUses = await SqlDapperConnection.QueryAsync<WtPlPaymentUse>(paymentUsesSqlQuery, TargetConnectionString);
            foreach (var oldPaymentUse in oldPaymentUses)
            {
                var paymentUse = CreatePaymenUse(oldPaymentUse);
                if (paymentUse == null) continue;
                Executor.GetCommand<CreatePaymentUseCommand>().Process(d => d.ExecuteSystem(paymentUse));

                paymentUse.DateCreate = new DateTimeOffset(oldPaymentUse.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdatePaymentUseCommand>().Process(d => d.Execute(paymentUse));
            }
        }

        private PaymentUse CreatePaymenUse(WtPlPaymentUse oldPayment)
        {
            try
            {
                var paymentUse = new PaymentUse
                {
                    Amount = oldPayment.Amount,
                    DateCreate = new DateTimeOffset(oldPayment.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldPayment.DateCreate.GetValueOrDefault(DateTime.Now)),
                    Description = oldPayment.Dsc,
                    ExternalId = oldPayment.Id,
                    PaymentId = GetObjectId<Payment>(oldPayment.PaymentId),
                    PaymentInvoiceId = GetObjectId<PaymentInvoice>(oldPayment.FixId),
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    BlockedAmount = 0,
                    ReturnedAmount = 0,
                    IsDeleted = false
                };

                return paymentUse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}