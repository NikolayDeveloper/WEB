using System;
using System.Data;

namespace Iserv.OldNiis.Domain.BulkEntities
{
    /// <summary>
    /// Платежи (WT_PL_PAYMENT)
    /// </summary>
    public class PaymentSqlBulCopy : BaseSqlBulCopy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sqlConnectionString">Строка соединения с бд.</param>
        public PaymentSqlBulCopy(string sqlConnectionString) : base(sqlConnectionString, "Payments")
        {
            Structure = GetStructure();
            InitializeSqlBulkCopy();
            InitializeDataTable();
        }

        #region Костанты с именами полей таблицы
        public const string IdColumn = "Id";
        public const string AmountColumn = "Amount";
        public const string AssignmentDescriptionColumn = "AssignmentDescription";
        public const string CurrencyAmountColumn = "CurrencyAmount";
        public const string CurrencyRateColumn = "CurrencyRate";
        public const string CurrencyTypeColumn = "CurrencyType";
        public const string CustomerIdColumn = "CustomerId";
        public const string DateCreateColumn = "DateCreate";
        public const string DateUpdateColumn = "DateUpdate";
        public const string ExternalIdColumn = "ExternalId";
        public const string IsPrePaymentColumn = "IsPrePayment";
        public const string Payment1CNumberColumn = "Payment1CNumber";
        public const string PaymentDateColumn = "PaymentDate";
        public const string PaymentNumberColumn = "PaymentNumber";
        public const string PurposeDescriptionColumn = "PurposeDescription";
        public const string TimestampColumn = "Timestamp";
        public const string PaymentStatusIdColumn = "PaymentStatusId";
        public const string CashBalanceColumn = "CashBalance";
        public const string IsForeignCurrencyColumn = "IsForeignCurrency";
        public const string PaymentUseAmmountSummColumn = "PaymentUseAmmountSumm";
        public const string EmployeeNameReturnedPaymentColumn = "EmployeeNameReturnedPayment";
        public const string IsAdvancePaymentColumn = "IsAdvancePayment";
        public const string PayerColumn = "Payer";
        public const string PayerBinOrInnColumn = "PayerBinOrInn";
        public const string PaymentCNumberBVUColumn = "PaymentCNumberBVU";
        public const string ReturnedDateColumn = "ReturnedDate";
        public const string PayerRNNColumn = "PayerRNN";
        public const string ImportedDateColumn = "ImportedDate";
        public const string UserImportedIdColumn = "UserImportedId";
        public const string UserNameImportedColumn = "UserNameImported";
        public const string UserPositionImportedColumn = "UserPositionImported";
        public const string ReturnedAmountColumn = "ReturnedAmount";
        public const string BlockedAmountColumn = "BlockedAmount";
        public const string BlockedDateColumn = "BlockedDate";
        public const string EmployeeNameBlockedPaymentColumn = "EmployeeNameBlockedPayment";
        public const string BlockedReasonColumn = "BlockedReason";
        public const string ReturnedReasonColumn = "ReturnedReason";
        #endregion

        #region Инициализация схемы колонок.

        /// <summary>
        /// Инициализация схемы колонок.
        /// </summary>
        private DataColumn[] GetStructure()
        {
            return new DataColumn[]
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
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = AssignmentDescriptionColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = CurrencyAmountColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = CurrencyRateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = CurrencyTypeColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = CustomerIdColumn,
                    AllowDBNull = true
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
                    DataType = typeof(int),
                    ColumnName = ExternalIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(bool),
                    ColumnName = IsPrePaymentColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = Payment1CNumberColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = PaymentDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = PaymentNumberColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = PurposeDescriptionColumn,
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
                    DataType = typeof(int),
                    ColumnName = PaymentStatusIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = CashBalanceColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(bool),
                    ColumnName = IsForeignCurrencyColumn,
                    DefaultValue = false
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = PaymentUseAmmountSummColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EmployeeNameReturnedPaymentColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(bool),
                    ColumnName = IsAdvancePaymentColumn,
                    DefaultValue = false
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
                    DataType = typeof(string),
                    ColumnName = PaymentCNumberBVUColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = ReturnedDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = PayerRNNColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = ImportedDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = UserImportedIdColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = UserNameImportedColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = UserPositionImportedColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = ReturnedAmountColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(decimal),
                    ColumnName = BlockedAmountColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(DateTimeOffset),
                    ColumnName = BlockedDateColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = EmployeeNameBlockedPaymentColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = BlockedReasonColumn,
                    AllowDBNull = true
                },
                new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = ReturnedReasonColumn,
                    AllowDBNull = true
                }
            };
        }

        #endregion
    }
}
