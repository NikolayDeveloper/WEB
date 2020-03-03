using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.EntitiesHistory.Payment;

namespace Iserv.Niis.Domain.Entities.Payment
{
	/// <summary>
	/// Выставленные счета(WT_PL_FIXPAYMENT)
	/// </summary>
	public class PaymentInvoice : Entity<int>, IHistorySupport, IHaveConcurrencyToken, ISoftDeletable
	{
		public PaymentInvoice()
		{
			PaymentUses = new HashSet<PaymentUse>();
		}

		public int TariffId { get; set; } //TARIFF_ID
		public DicTariff Tariff { get; set; }
		public decimal Coefficient { get; set; } //FINE_PERCENT
		public decimal Nds { get; set; } //VAT_PERCENT
		public int? RequestId { get; set; }
		public Request.Request Request { get; set; }
		public int? ProtectionDocId { get; set; } //APP_ID
		public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
		public int? ContractId { get; set; }
		public Contract.Contract Contract { get; set; }

		public decimal PenaltyPercent { get; set; } //PENI_PERCENT
		public int? TariffCount { get; set; } //TARIFF_COUNT
		public bool? IsComplete { get; set; } //IS_COMPLETE
											  /// <summary>
											  /// Дата просрочки
											  /// </summary>
		public DateTimeOffset? OverdueDate { get; set; } //DATE_LIMIT
														 /// <summary>
														 /// Дата зачтения?
														 /// </summary>
		public DateTimeOffset? DateFact { get; set; } //DATE_FACT
													  /// <summary>
													  /// Дата списания
													  /// </summary>
		public DateTimeOffset? DateComplete { get; set; } //DATE_COMPLETE
		public int? ApplicantTypeId { get; set; }
		public DicApplicantType ApplicantType { get; set; }
		/// <summary>
		/// Пользователь, создавший оплату
		/// </summary>
		public int? CreateUserId { get; set; } //flCreateUserId
		public ApplicationUser CreateUser { get; set; }
		/// <summary>
		/// Пользователь, списавший оплату
		/// </summary>
		public int? WriteOffUserId { get; set; }
		public ApplicationUser WriteOffUser { get; set; }
		/// <summary>
		/// Пользователь, который зачел оплату
		/// </summary>
		public int? WhoBoundUserId { set; get; }
		public ApplicationUser WhoBoundUser { get; set; }
		public ICollection<PaymentUse> PaymentUses { get; set; }
		public int StatusId { get; set; }
		public DicPaymentStatus Status { get; set; }
		/// <summary>
		/// Дата, время экспорта в 1С
		/// </summary>
		public DateTimeOffset? DateExportedTo1C { get; set; }

		/// <summary>
		/// Дата исправления зачтённой оплаты 
		/// </summary>
		public DateTimeOffset? DateOfChangingChargedPaymentInvoice { get; set; }

		/// <summary>
		/// Причина исправления зачтённой оплаты 
		/// </summary>
		public string ReasonOfChangingChargedPaymentInvoice { get; set; }

		/// <summary>
		/// Сотрудник, выполнивший исправление зачтённой оплаты (ФИО и должность сотрудника, выполнившего исправление зачтённой оплаты )
		/// </summary>
		public string EmployeeAndPositonWhoChangedChargedPaymentInvoice {get;set;}


		/// <summary>
		/// Дата удаления зачтённой оплаты 
		/// </summary>
		public DateTimeOffset? DateOfDeletingChargedPaymentInvoice { get; set; }

		/// <summary>
		/// Причина удаления зачтённой оплаты 
		/// </summary>
		public string ReasonOfDeletingChargedPaymentInvoice { get; set; }

		/// <summary>
		/// Сотрудник, выполнивший уделение зачтённой оплаты (ФИО и должность сотрудника, выполнившего удаления зачтённой оплаты )
		/// </summary>
		public string EmployeeAndPositonWhoDeleteChargedPaymentInvoice { get; set; }
		public bool IsDeleted { get; set; }
		public DateTimeOffset? DeletedDate { get; set; }

		public Type GetHistoryEntity()
        {
            return typeof(PaymentInvoiseHistory);
        }
    }
}