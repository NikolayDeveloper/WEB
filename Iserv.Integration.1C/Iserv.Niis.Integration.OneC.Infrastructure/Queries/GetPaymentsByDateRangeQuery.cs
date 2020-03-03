using System;
using System.Collections.Generic;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Integration.OneC.Infrastructure.Interfaces;
using Iserv.Niis.Integration.OneC.Model;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Queries
{
    /// <summary>
    /// Получает платежи в диапазоне дат "с" .. "по".
    /// </summary>
    public class GetPaymentsByDateRangeQuery : BaseOneCQuery
    {
        private const string AdvanceCode = "3510";

        /// <summary>
        /// Выполняет запрос.
        /// </summary>
        /// <param name="fromDate">Дата "с".</param>
        /// <param name="toDate">Дата "по".</param>
        /// <returns></returns>
        public List<PaymentFrom1CDto> Execute(DateTime fromDate, DateTime toDate)
        {
            using (var oneCConnection = CreateOneCConnection())
            {
                var payments = new List<PaymentFrom1CDto>();

                dynamic documents = null;

                try
                {
                    var comConnection = oneCConnection.Open();

                    documents = comConnection.Документы.ПлатежноеПоручениеВходящее.Выбрать(
                        new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0),
                        new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59));

                    while (documents.Следующий())
                    {
                        var code = documents.РасшифровкаПлатежа.Get(0).СчетУчетаРасчетовПоАвансам.Код;

                        if (code != AdvanceCode)
                            continue;

                        var payment1CDto = new PaymentFrom1CDto
                        {
                            Payment1CNumber = (documents.НомерВходящегоДокумента + string.Empty).Trim(),
                            PurposeDescription = (documents.Комментарий + string.Empty).Trim(),
                            Payer = (documents.Контрагент.НаименованиеПолное + string.Empty).Trim(),
                            Amount = Convert.ToDecimal(documents.СуммаДокумента),
                            PaymentCNumberBVU = (documents.Номер + string.Empty).Trim(),
                            PaymentDate = new DateTimeOffset(documents.ДатаВыписки),
                            PayerBinOrInn = (documents.Контрагент.ИдентификационныйКодЛичности + string.Empty).Trim(),
                            PayerRNN = (documents.Контрагент.РНН + string.Empty).Trim(),
                            CurrencyType = (documents.ВалютаДокумента.БуквенныйКод + string.Empty).Trim(),
                            CustomerType = comConnection.XMLString(documents.Контрагент.ЮрФизЛицо),
                            IsAdvancePayment = true
                        };

                        payments.Add(payment1CDto);
                    }

                    return payments;
                }
                catch (Exception exception)
                {
                    throw new ComException(ComExceptionType.UnknownComError, "Unknown com-related error occured.",
                        exception);
                }
                finally
                {
                    ReleaseComObject(ref documents);
                    oneCConnection.Close();
                }
            }
        }
    }
}
