namespace Iserv.Niis.Common.Codes
{
    /// <summary>
    /// Коды Отчетов
    /// </summary>
    public class ReportCodes
    {
        /// <summary>
        /// Сведения по поступившим в РГП «НИИС» заявкам на выдачу охранных документов на объекты промышленной собственности за год
        /// </summary>
        public const string ReceivedRequestReport = "ReceivedRequestReport";

        /// <summary>
        /// Сведения по выданным охранным документам на объекты промышленной собственности РГП «НИИС» за указанный период
        /// </summary>
        public const string IssuedProtectionDocumentsReport = "IssuedProtectionDocumentsReport";

		/// <summary>
		/// Отчёт о зачтённых оплатах
		/// </summary>
		public const string ChargedPaymentInvoicesReport = "ChargedPaymentInvoicesReport";

		/// <summary>
		/// 2.	Отчёт о выполненной работе
		/// </summary>
		public const string WorkIsDoneReport = "WorkIsDoneReport";
	}
}
