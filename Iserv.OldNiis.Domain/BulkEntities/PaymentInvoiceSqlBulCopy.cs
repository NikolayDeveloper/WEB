using System;
using System.Data;

namespace Iserv.OldNiis.Domain.BulkEntities
{
    /// <summary>
    /// Выставленные счета(WT_PL_FIXPAYMENT).
    /// </summary>
    public class PaymentInvoiceSqlBulCopy: BaseSqlBulCopy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sqlConnectionString">Строка соединения с бд.</param>
        public PaymentInvoiceSqlBulCopy(string sqlConnectionString) :base(sqlConnectionString, "PaymentInvoices")
        {
            Structure = GetStructure();
            InitializeSqlBulkCopy();
            InitializeDataTable();
        }

        #region Костанты с именами полей таблицы
        public const string IdColumn = "Id";
        public const string ApplicantTypeIdColumn = "ApplicantTypeId";
        public const string CoefficientColumn = "Coefficient";
        public const string ContractIdColumn = "ContractId";
        public const string CreateUserIdColumn = "CreateUserId";
        public const string DateCompleteColumn = "DateComplete";
        public const string DateCreateColumn = "DateCreate";
        public const string DateFactColumn = "DateFact";
        public const string DateUpdateColumn = "DateUpdate";
        public const string ExternalIdColumn = "ExternalId";
        public const string IsCompleteColumn = "IsComplete";
        public const string NdsColumn = "Nds";
        public const string OverdueDateColumn = "OverdueDate";
        public const string PenaltyPercentColumn = "PenaltyPercent";
        public const string ProtectionDocIdColumn = "ProtectionDocId";
        public const string RequestIdColumn = "RequestId";
        public const string StatusIdColumn = "StatusId";
        public const string TariffCountColumn = "TariffCount";
        public const string TariffIdColumn = "TariffId";
        public const string TimestampColumn = "Timestamp";
        public const string WhoBoundUserIdColumn = "WhoBoundUserId";
        public const string WriteOffUserIdColumn = "WriteOffUserId";
        public const string DateExportedTo1CColumn = "DateExportedTo1C";
        public const string DateOfChangingChargedPaymentInvoiceColumn = "DateOfChangingChargedPaymentInvoice";
        public const string DateOfDeletingChargedPaymentInvoiceColumn = "DateOfDeletingChargedPaymentInvoice";
        public const string EmployeeAndPositonWhoChangedChargedPaymentInvoiceColumn = "EmployeeAndPositonWhoChangedChargedPaymentInvoice";
        public const string EmployeeAndPositonWhoDeleteChargedPaymentInvoiceColumn = "EmployeeAndPositonWhoDeleteChargedPaymentInvoice";
        public const string ReasonOfChangingChargedPaymentInvoiceColumn = "ReasonOfChangingChargedPaymentInvoice";
        public const string ReasonOfDeletingChargedPaymentInvoiceColumn = "ReasonOfDeletingChargedPaymentInvoice";
        public const string DeletedDateColumn = "DeletedDate";
        public const string IsDeletedColumn = "IsDeleted";
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
DataType = typeof(int),
ColumnName = ApplicantTypeIdColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(decimal),
ColumnName = CoefficientColumn,
},
new DataColumn
{
DataType = typeof(int),
ColumnName = ContractIdColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(int),
ColumnName = CreateUserIdColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(DateTimeOffset),
ColumnName = DateCompleteColumn,
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
ColumnName = DateFactColumn,
AllowDBNull = true
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
ColumnName = IsCompleteColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(decimal),
ColumnName = NdsColumn,
},
new DataColumn
{
DataType = typeof(DateTimeOffset),
ColumnName = OverdueDateColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(decimal),
ColumnName = PenaltyPercentColumn,
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
DataType = typeof(int),
ColumnName = StatusIdColumn,
},
new DataColumn
{
DataType = typeof(int),
ColumnName = TariffCountColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(int),
ColumnName = TariffIdColumn,
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
ColumnName = WhoBoundUserIdColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(int),
ColumnName = WriteOffUserIdColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(DateTimeOffset),
ColumnName = DateExportedTo1CColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(DateTimeOffset),
ColumnName = DateOfChangingChargedPaymentInvoiceColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(DateTimeOffset),
ColumnName = DateOfDeletingChargedPaymentInvoiceColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(string),
ColumnName = EmployeeAndPositonWhoChangedChargedPaymentInvoiceColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(string),
ColumnName = EmployeeAndPositonWhoDeleteChargedPaymentInvoiceColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(string),
ColumnName = ReasonOfChangingChargedPaymentInvoiceColumn,
AllowDBNull = true
},
new DataColumn
{
DataType = typeof(string),
ColumnName = ReasonOfDeletingChargedPaymentInvoiceColumn,
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
};
        }
        #endregion
    }
}
