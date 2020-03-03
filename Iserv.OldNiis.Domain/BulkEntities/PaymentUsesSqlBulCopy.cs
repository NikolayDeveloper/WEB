using System;
using System.Data;

namespace Iserv.OldNiis.Domain.BulkEntities
{
    /// <summary>
    /// Распределенаня оплата (WT_PL_PAYMENT_USE).
    /// </summary>
    public class PaymentUsesSqlBulCopy : BaseSqlBulCopy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sqlConnectionString">Строка соединения с бд.</param>
        public PaymentUsesSqlBulCopy(string sqlConnectionString) : base(sqlConnectionString, "PaymentUses")
        {
            Structure = GetStructure();
            InitializeSqlBulkCopy();
            InitializeDataTable();
        }

        #region Костанты с именами полей таблицы
        public const string IdColumn = "Id";
        public const string AmountColumn = "Amount";
        public const string DateCreateColumn = "DateCreate";
        public const string DateUpdateColumn = "DateUpdate";
        public const string DescriptionColumn = "Description";
        public const string ExternalIdColumn = "ExternalId";
        public const string PaymentIdColumn = "PaymentId";
        public const string PaymentInvoiceIdColumn = "PaymentInvoiceId";
        public const string TimestampColumn = "Timestamp";
        public const string BlockedAmountColumn = "BlockedAmount";
        public const string BlockedAmountEmployeeNameColumn = "BlockedAmountEmployeeName";
        public const string BlockedAmountReasonColumn = "BlockedAmountReason";
        public const string ContractIdColumn = "ContractId";
        public const string DateOfPaymentColumn = "DateOfPayment";
        public const string DeletionClearedPaymentDateColumn = "DeletionClearedPaymentDate";
        public const string DeletionClearedPaymentEmployeeNameColumn = "DeletionClearedPaymentEmployeeName";
        public const string DeletionClearedPaymentReasonColumn = "DeletionClearedPaymentReason";
        public const string DicProtectionDocSubTypeIdColumn = "DicProtectionDocSubTypeId";
        public const string DicProtectionDocTypeIdColumn = "DicProtectionDocTypeId";
        public const string DicTariffIdColumn = "DicTariffId";
        public const string EmployeeCheckoutPaymentNameColumn = "EmployeeCheckoutPaymentName";
        public const string EmployeeWriteOffPaymentNameColumn = "EmployeeWriteOffPaymentName";
        public const string IssuingPaymentDateColumn = "IssuingPaymentDate";
        public const string PayerColumn = "Payer";
        public const string PayerBinOrInnColumn = "PayerBinOrInn";
        public const string PaymentUseDateColumn = "PaymentUseDate";
        public const string ProtectionDocIdColumn = "ProtectionDocId";
        public const string RequestIdColumn = "RequestId";
        public const string ReturnAmountEmployeeNameColumn = "ReturnAmountEmployeeName";
        public const string ReturnAmountReasonColumn = "ReturnAmountReason";
        public const string ReturnedAmountColumn = "ReturnedAmount";
        public const string ReturnedAmountDateColumn = "ReturnedAmountDate";
        public const string DeletedDateColumn = "DeletedDate";
        public const string IsDeletedColumn = "IsDeleted";
        public const string EditClearedPaymentDateColumn = "EditClearedPaymentDate";
        public const string EditClearedPaymentEmployeeNameColumn = "EditClearedPaymentEmployeeName";
        public const string EditClearedPaymentReasonColumn = "EditClearedPaymentReason";
        #endregion

        #region Инициализация схемы колонок.
        /// <summary>
        /// Инициализация схемы колонок.
        /// </summary>
        private DataColumn[] GetStructure()
        {
            return new []
            {
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = IdColumn,
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = AmountColumn,
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = DateCreateColumn,
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = DateUpdateColumn,
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = DescriptionColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = ExternalIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = PaymentIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = PaymentInvoiceIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(byte[]),
                    ColumnName = TimestampColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = BlockedAmountColumn,
                    DefaultValue = 0,
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = BlockedAmountEmployeeNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = BlockedAmountReasonColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = ContractIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = DateOfPaymentColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = DeletionClearedPaymentDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = DeletionClearedPaymentEmployeeNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = DeletionClearedPaymentReasonColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = DicProtectionDocSubTypeIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = DicProtectionDocTypeIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = DicTariffIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EmployeeCheckoutPaymentNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EmployeeWriteOffPaymentNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = IssuingPaymentDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = PayerColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = PayerBinOrInnColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = PaymentUseDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = ProtectionDocIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = RequestIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = ReturnAmountEmployeeNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = ReturnAmountReasonColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = ReturnedAmountColumn,
                    DefaultValue = 0,
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = ReturnedAmountDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = DeletedDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(bool),
                    ColumnName = IsDeletedColumn,
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = EditClearedPaymentDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EditClearedPaymentEmployeeNameColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EditClearedPaymentReasonColumn,
                    AllowDBNull = true
                },
            };
        }
        #endregion
    }
}