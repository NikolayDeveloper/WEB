using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Iserv.OldNiis.Domain.BulkEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NIIS.DBConverter.Entities.Payments;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisPaymentService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly AppConfiguration _appConfiguration;
        private readonly DictionaryTypesHelper _dictionaryTypesService;

        public OldNiisPaymentService(
            OldNiisContext context,
            AppConfiguration appConfiguration,
            DictionaryTypesHelper dictionaryTypesService)
        {
            _context = context;
            _appConfiguration = appConfiguration;
            _dictionaryTypesService = dictionaryTypesService;
        }

        #region Миграция данных из таблицы "Выставленные счета".
        /// <summary>
        /// Миграция данных из таблицы "Выставленные счета".
        /// </summary>
        /// <param name="packageSize">Размер пакета.</param>
        /// <returns></returns>
        public void GetPaymentInvoices(int packageSize)
        {
            var pldPaymentInvoicesCount = GetPaymentCount(typeof(WtPlFixpayment));
            Console.Write($"\rPaymentInvoices commited {pldPaymentInvoicesCount}");
            using (var connection = new SqlConnection(_appConfiguration.OldNiisConnectionString))
            {
                connection.Open();

                var oldContractTypeIds = _dictionaryTypesService.GetContractTypeIds();
                var oldRequestTypeIds = _dictionaryTypesService.GetRequestTypeIds();

                const string sqlQuery = @"SELECT 
	                                        wpf.U_ID, 
	                                        wpf.date_create,
	                                        wpf.stamp,
	                                        wpf.DATE_COMPLETE,
	                                        wpf.DATE_FACT,
	                                        wpf.DATE_LIMIT,

                                            wpf.APP_ID,
	                                        ddd.DOCTYPE_ID AS DOCTYPE_ID,

                                            wpf.TARIFF_ID,
	                                        wpf.flCreateUserId,
                                            wpf.TARIFF_COUNT,
                                            wpf.IS_COMPLETE,
                                            wpf.VAT_PERCENT,
                                            wpf.FINE_PERCENT,
                                            wpf.PENI_PERCENT,
                                            2 AS StatusId
                                        FROM dbo.WT_PL_FIXPAYMENT wpf
                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = wpf.APP_ID";



                var command = new SqlCommand(sqlQuery, connection);

                using (var sqlDataReader = command.ExecuteReader())
                {
                    var idOrdinal = sqlDataReader.GetOrdinal("U_ID");
                    var dateCreateOrdinal = sqlDataReader.GetOrdinal("date_create");
                    var dateUpdateOrdinal = sqlDataReader.GetOrdinal("stamp");
                    var dateCompleteOrdinal = sqlDataReader.GetOrdinal("DATE_COMPLETE");
                    var dateFactOrdinal = sqlDataReader.GetOrdinal("DATE_FACT");
                    var overdueDateOrdinal = sqlDataReader.GetOrdinal("DATE_LIMIT");
                    var appIdOrdinal = sqlDataReader.GetOrdinal("APP_ID");
                    var documentTypeIdOrdinal = sqlDataReader.GetOrdinal("DOCTYPE_ID");
                    var tariffIdOrdinal = sqlDataReader.GetOrdinal("TARIFF_ID");
                    var createUserIdOrdinal = sqlDataReader.GetOrdinal("flCreateUserId");
                    var tariffCountOrdinal = sqlDataReader.GetOrdinal("TARIFF_COUNT");
                    var isCompleteOrdinal = sqlDataReader.GetOrdinal("IS_COMPLETE");
                    var ndsOrdinal = sqlDataReader.GetOrdinal("VAT_PERCENT");
                    var coefficientOrdinal = sqlDataReader.GetOrdinal("FINE_PERCENT");
                    var penaltyPercentOrdinal = sqlDataReader.GetOrdinal("PENI_PERCENT");
                    var statusIdOrdinal = sqlDataReader.GetOrdinal("StatusId");

                    var paymentInvoiceSqlBulCopy = new PaymentInvoiceSqlBulCopy(_appConfiguration.NiisConnectionString);
                    
                    while (sqlDataReader.Read())
                    {
                        var dataRow = paymentInvoiceSqlBulCopy.DataTable.NewRow();

                        dataRow[PaymentInvoiceSqlBulCopy.IdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.CoefficientColumn] = sqlDataReader[coefficientOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.CreateUserIdColumn] = sqlDataReader[createUserIdOrdinal];
                        if (sqlDataReader[dateCompleteOrdinal] != DBNull.Value)
                            dataRow[PaymentInvoiceSqlBulCopy.DateCompleteColumn] = DateTimeOffset.Parse(sqlDataReader[dateCompleteOrdinal].ToString());
                        dataRow[PaymentInvoiceSqlBulCopy.DateCreateColumn] = sqlDataReader[dateCreateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateCreateOrdinal].ToString());
                        if (sqlDataReader[dateFactOrdinal] != DBNull.Value)
                            dataRow[PaymentInvoiceSqlBulCopy.DateFactColumn] = DateTimeOffset.Parse(sqlDataReader[dateFactOrdinal].ToString());
                        dataRow[PaymentInvoiceSqlBulCopy.DateUpdateColumn] = sqlDataReader[dateUpdateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateUpdateOrdinal].ToString());
                        dataRow[PaymentInvoiceSqlBulCopy.ExternalIdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.IsCompleteColumn] = CustomConverter.StringToNullableBool(sqlDataReader[isCompleteOrdinal].ToString());
                        dataRow[PaymentInvoiceSqlBulCopy.NdsColumn] = sqlDataReader[ndsOrdinal];
                        if (sqlDataReader[overdueDateOrdinal] != DBNull.Value)
                            dataRow[PaymentInvoiceSqlBulCopy.OverdueDateColumn] = DateTimeOffset.Parse(sqlDataReader[overdueDateOrdinal].ToString());
                        dataRow[PaymentInvoiceSqlBulCopy.PenaltyPercentColumn] = sqlDataReader[penaltyPercentOrdinal];

                        dataRow[PaymentInvoiceSqlBulCopy.ContractIdColumn] = DBNull.Value;
                        dataRow[PaymentInvoiceSqlBulCopy.ProtectionDocIdColumn] = DBNull.Value;
                        dataRow[PaymentInvoiceSqlBulCopy.RequestIdColumn] = DBNull.Value;

                        if (sqlDataReader[documentTypeIdOrdinal] != DBNull.Value)
                        {
                            if (oldContractTypeIds.Any(d => d == (int?)sqlDataReader[documentTypeIdOrdinal]))
                                dataRow[PaymentInvoiceSqlBulCopy.ContractIdColumn] = sqlDataReader[appIdOrdinal];
                            else if (oldRequestTypeIds.Any(d => d == (int?)sqlDataReader[documentTypeIdOrdinal]))
                                dataRow[PaymentInvoiceSqlBulCopy.RequestIdColumn] = sqlDataReader[appIdOrdinal];
                        }
                        else
                            dataRow[PaymentInvoiceSqlBulCopy.ProtectionDocIdColumn] = sqlDataReader[appIdOrdinal];

                        dataRow[PaymentInvoiceSqlBulCopy.StatusIdColumn] = sqlDataReader[statusIdOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.TariffCountColumn] = sqlDataReader[tariffCountOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.TariffIdColumn] = sqlDataReader[tariffIdOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.IsDeletedColumn] = false;

                        paymentInvoiceSqlBulCopy.DataTable.Rows.Add(dataRow);

                        if (paymentInvoiceSqlBulCopy.DataTable.Rows.Count < packageSize)
                            continue;

                        paymentInvoiceSqlBulCopy.SqlBulkCopyWriteToServer();
                    }

                    paymentInvoiceSqlBulCopy.SqlBulkCopyWriteToServer();

                    sqlDataReader.Close();
                }
                connection.Close();
            }
        }
        #endregion

        #region Миграция данных из таблицы "Платежи".
        /// <summary>
        /// Миграция данных из таблицы "Платежи".
        /// </summary>
        /// <param name="packageSize">Размер пакета.</param>
        /// <returns></returns>
        public void GetPayments(int packageSize)
        {
            var pldPaymentCount = GetPaymentCount(typeof(WtPlPayment));
            Console.Write($"\rPayments {pldPaymentCount}");
            using (var sqlConnection = new SqlConnection(_appConfiguration.OldNiisConnectionString))
            {
                sqlConnection.Open();

                const string sqlQuery = @"SELECT DISTINCT
	                                        wpf.U_ID, 
	                                        wpf.date_create,
	                                        wpf.CUSTOMER_ID,
	                                        wpf.PAYMENT_AMOUNT,
	                                        wpf.USE_DSC,
	                                        wpf.DSC,
	                                        wpf.flValSum,
	                                        wpf.flExchangeRate,
	                                        wpf.flValType,	
	                                        wpf.IS_AVANS,
	                                        wpf.PAYMENT_NUMB,
	                                        wpf.PAYMENT_DATE,
	                                        wpf.PAYMENT_TYPE
                                        FROM dbo.WT_PL_PAYMENT wpf";

                var command = new SqlCommand(sqlQuery, sqlConnection);

                using (var sqlDataReader = command.ExecuteReader())
                {
                    var idOrdinal = sqlDataReader.GetOrdinal("U_ID");
                    var dateCreateOrdinal = sqlDataReader.GetOrdinal("date_create");
                    var customerIdOrdinal = sqlDataReader.GetOrdinal("CUSTOMER_ID");
                    var amountOrdinal = sqlDataReader.GetOrdinal("PAYMENT_AMOUNT");
                    var assignmentDescriptionOrdinal = sqlDataReader.GetOrdinal("USE_DSC");
                    var purposeDescriptionOrdinal = sqlDataReader.GetOrdinal("DSC");
                    var currencyAmountOrdinal = sqlDataReader.GetOrdinal("flValSum");
                    var currencyRateOrdinal = sqlDataReader.GetOrdinal("flExchangeRate");
                    var currencyTypeOrdinal = sqlDataReader.GetOrdinal("flValType");
                    var isPrePaymentOrdinal = sqlDataReader.GetOrdinal("IS_AVANS");
                    var payment1CNumberOrdinal = sqlDataReader.GetOrdinal("PAYMENT_NUMB");
                    var paymentDateOrdinal = sqlDataReader.GetOrdinal("PAYMENT_DATE");
                    var paymentNumberOrdinal = sqlDataReader.GetOrdinal("PAYMENT_TYPE");

                    var paymentSqlBulCopy = new PaymentSqlBulCopy(_appConfiguration.NiisConnectionString);
                    
                    while (sqlDataReader.Read())
                    {
                        var dataRow = paymentSqlBulCopy.DataTable.NewRow();

                        dataRow[PaymentSqlBulCopy.IdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentSqlBulCopy.ExternalIdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentSqlBulCopy.DateCreateColumn] = sqlDataReader[dateCreateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateCreateOrdinal].ToString()); 
                        dataRow[PaymentSqlBulCopy.DateUpdateColumn] = sqlDataReader[dateCreateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateCreateOrdinal].ToString());
                        dataRow[PaymentSqlBulCopy.CustomerIdColumn] = sqlDataReader[customerIdOrdinal] == DBNull.Value ? 0 : sqlDataReader[customerIdOrdinal];
                        if (sqlDataReader[amountOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.AmountColumn] = sqlDataReader[amountOrdinal];
                        if (sqlDataReader[assignmentDescriptionOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.AssignmentDescriptionColumn] = sqlDataReader[assignmentDescriptionOrdinal];
                        if (sqlDataReader[purposeDescriptionOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.PurposeDescriptionColumn] = sqlDataReader[purposeDescriptionOrdinal];
                        if (sqlDataReader[currencyAmountOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.CurrencyAmountColumn] = sqlDataReader[currencyAmountOrdinal];
                        if (sqlDataReader[currencyRateOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.CurrencyRateColumn] = sqlDataReader[currencyRateOrdinal];
                        if (sqlDataReader[currencyTypeOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.CurrencyTypeColumn] = sqlDataReader[currencyTypeOrdinal];
                        if (sqlDataReader[isPrePaymentOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.IsPrePaymentColumn] = CustomConverter.StringToNullableBool(sqlDataReader[isPrePaymentOrdinal].ToString());
                        if (sqlDataReader[payment1CNumberOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.Payment1CNumberColumn] = sqlDataReader[payment1CNumberOrdinal];
                        dataRow[PaymentSqlBulCopy.PaymentDateColumn] = sqlDataReader[paymentDateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[paymentDateOrdinal].ToString());
                        if (sqlDataReader[paymentNumberOrdinal] != DBNull.Value)
                            dataRow[PaymentSqlBulCopy.PaymentNumberColumn] = sqlDataReader[paymentNumberOrdinal];

                        paymentSqlBulCopy.DataTable.Rows.Add(dataRow);

                        if (paymentSqlBulCopy.DataTable.Rows.Count < packageSize)
                            continue;

                        paymentSqlBulCopy.SqlBulkCopyWriteToServer();
                    }
                    paymentSqlBulCopy.SqlBulkCopyWriteToServer();

                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
        }
        #endregion

        #region Миграция данных из таблицы "Распределенаня оплата".
        /// <summary>
        /// Миграция данных из таблицы "Распределенаня оплата".
        /// </summary>
        /// <param name="packageSize">Размер пакета.</param>
        /// <returns></returns>
        public void GetPaymentUses(int packageSize)
        {
            var pldPaymentUsesCount = GetPaymentCount(typeof(WtPlFixpayment));
            Console.Write($"\rPaymentUses {pldPaymentUsesCount}");
            using (var connection = new SqlConnection(_appConfiguration.OldNiisConnectionString))
            {
                connection.Open();

                const string sqlQuery = @"SELECT DISTINCT
	                                        WPPU.U_ID, 
	                                        WPPU.date_create,
	                                        WPPU.PAYMENT_ID,
	                                        WPPU.FIX_ID,
	                                        WPPU.AMOUNT,
	                                        WPPU.DSC
                                        FROM dbo.WT_PL_PAYMENT_USE WPPU";

                var command = new SqlCommand(sqlQuery, connection);

                using (var sqlDataReader = command.ExecuteReader())
                {
                    var idOrdinal = sqlDataReader.GetOrdinal("U_ID");
                    var dateCreateOrdinal = sqlDataReader.GetOrdinal("date_create");
                    var paymentIdOrdinal = sqlDataReader.GetOrdinal("PAYMENT_ID");
                    var paymentInvoiceIdOrdinal = sqlDataReader.GetOrdinal("FIX_ID");
                    var amountOrdinal = sqlDataReader.GetOrdinal("AMOUNT");
                    var descriptionOrdinal = sqlDataReader.GetOrdinal("DSC");

                    var paymentUsesSqlBulCopy = new PaymentUsesSqlBulCopy(_appConfiguration.NiisConnectionString);
                    
                    while (sqlDataReader.Read())
                    {
                        var dataRow = paymentUsesSqlBulCopy.DataTable.NewRow();

                        dataRow[PaymentUsesSqlBulCopy.IdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentUsesSqlBulCopy.ExternalIdColumn] = sqlDataReader[idOrdinal];
                        dataRow[PaymentUsesSqlBulCopy.DateCreateColumn] = sqlDataReader[dateCreateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateCreateOrdinal].ToString());
                        dataRow[PaymentUsesSqlBulCopy.DateUpdateColumn] = sqlDataReader[dateCreateOrdinal] == DBNull.Value ? DateTimeOffset.Now : DateTimeOffset.Parse(sqlDataReader[dateCreateOrdinal].ToString());
                        if (sqlDataReader[paymentIdOrdinal] != DBNull.Value && (int?)sqlDataReader[paymentIdOrdinal] != 0)
                            dataRow[PaymentUsesSqlBulCopy.PaymentIdColumn] = sqlDataReader[paymentIdOrdinal];
                        if (sqlDataReader[paymentInvoiceIdOrdinal] != DBNull.Value && (int?)sqlDataReader[paymentInvoiceIdOrdinal] != 0)
                            dataRow[PaymentUsesSqlBulCopy.PaymentInvoiceIdColumn] = sqlDataReader[paymentInvoiceIdOrdinal];
                        dataRow[PaymentUsesSqlBulCopy.AmountColumn] = sqlDataReader[amountOrdinal];
                        dataRow[PaymentUsesSqlBulCopy.DescriptionColumn] = sqlDataReader[descriptionOrdinal];
                        dataRow[PaymentInvoiceSqlBulCopy.IsDeletedColumn] = false;

                        paymentUsesSqlBulCopy.DataTable.Rows.Add(dataRow);

                        if (paymentUsesSqlBulCopy.DataTable.Rows.Count < packageSize)
                            continue;

                        paymentUsesSqlBulCopy.SqlBulkCopyWriteToServer();
                    }

                    paymentUsesSqlBulCopy.SqlBulkCopyWriteToServer();

                    sqlDataReader.Close();
                }

                connection.Close();
            }

            //var oldPaymentUses = _context.WtPlPaymentUses
            //    .AsNoTracking()
            //    .Where(p => p.Id > lastId)
            //    .OrderBy(p => p.Id)
            //    .Take(packageSize)
            //    .ToList();

            //return oldPaymentUses.Select(p => new PaymentUse
            //{
            //    Id = p.Id,
            //    ExternalId = p.Id,
            //    DateCreate = p.DateCreate ?? DateTimeOffset.Now,
            //    DateUpdate = p.DateCreate ?? DateTimeOffset.Now,
            //    PaymentId = p.PaymentId == 0 ? null : p.PaymentId,
            //    PaymentInvoiceId = p.FixId == 0 ? null : p.FixId,
            //    //CreateUserId = p.CreateUserId,
            //    Amount = p.Amount,
            //    Description = p.Dsc
            //}).ToList();
        }
        #endregion

        #region Other

        public int GetPaymentCount(Type paymentType)
    {
        var payments = _context.Set(paymentType) as IQueryable<dynamic>;
        return (payments ?? throw new InvalidOperationException()).Count();
    }

        public Payment GetPayment(int id)
        {
            var oldPayment = _context.WtPlPayments
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            if (oldPayment == null)
            {
                return null;
            }

            return new Payment
            {
                Id = oldPayment.Id,
                DateCreate = oldPayment.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = oldPayment.DateCreate ?? DateTimeOffset.Now,
                CustomerId = oldPayment.CustomerId == 0 ? null : oldPayment.CustomerId,
                Amount = oldPayment.Amount,
                AssignmentDescription = oldPayment.UseDsc,
                PurposeDescription = oldPayment.Dsc,
                CurrencyAmount = oldPayment.ValSum,
                CurrencyRate = oldPayment.ExchangeRate,
                CurrencyType = oldPayment.ValType,
                IsPrePayment = CustomConverter.StringToNullableBool(oldPayment.IsAvans),
                Payment1CNumber = oldPayment.Payment1CNumber,
                PaymentDate = oldPayment.PaymentDate,
                PaymentNumber = oldPayment.PaymentType
            };
        }
        
        public List<int> GetPaymentIdsLargerLastBarcode(int? lastBarcode)
        {
            var paymentIds = _context.WtPlPayments
                .AsNoTracking()
                .Where(p => p.Id > lastBarcode)
                .OrderBy(p => p.Id)
                .Select(p => p.Id)
                .ToList();

            return paymentIds;
        }

    #endregion
    }
}
