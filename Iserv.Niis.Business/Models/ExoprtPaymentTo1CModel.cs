using Iserv.Niis.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Business.Models
{
	public class ExoprtPaymentTo1CModel
	{
		public string PaymentUseId { get; set; }
		/// <summary>
		/// Номер платежа (Значение из 1С)
		/// </summary>
		public string Payment1CNumber { get; set; }

		/// <summary>
		/// Дата платежа (Дата поступления платежа. Значение из 1С.)
		/// </summary>
		public DateTimeOffset? PaymentDate { get; set; }


		public decimal PaymentAmount{ get; set; }

		/// <summary>
		/// Назначение (Краткое описание назначения платежа. Значение из 1С)
		/// </summary>
		public string PurposeDescription { get; set; }

		/// <summary>
		/// Плательщик (Наименование контрагента. Значение из 1С)
		/// </summary>
		public string Payer { get; set; }

		/// <summary>
		/// ID платежа , присвоенный в АИС НИИС
		/// </summary>
		public int PaymentID { get; set; }

		/// <summary>
		/// Номер документа 1С (Значение из 1С. Номер платежа, присвоенный БВУ. банковская выписка ??)
		/// </summary>
		public string PaymentCNumberBVU { get; set; }

		/// <summary>
		/// ИИН\БИН плательщика (ИИН\БИН плательщика совершившего платёж)
		/// </summary>
		[MaxLength(12)]
		public string PayerBinOrInn { get; set; }

		/// <summary>
		/// РНН контрагента совершившего платёж
		/// </summary>
		[MaxLength(12)]
		public string PayerRNN { get; set; }
		/// <summary>
		/// Отображается код услуги (Должны использоваться те же кода, которые используются в старой Системе)
		/// </summary>
		public string TariffCode { get; set; }
		
		/// <summary>
		/// Отображается наименование услуги 
		/// </summary>
		public string TariffName { get; set; }

		/// <summary>
		/// Номер Заявки
		/// </summary>
		public string RequestNum { get; set; }

		/// <summary>
		/// Связанные документы
		/// </summary>
		public string LinkedProtectionDocNums { get; set; }

		/// <summary>
		/// Номер ОД
		/// </summary>
		public string ProtectionDocTypeNumber { get; set; }

		/// <summary>
		/// Вид ОПС. Отображается наименование вида ОПС по которому оплачена услуга.
		/// </summary>
		public string ProtectionDocType { get; set; }

		/// <summary>
		/// Подвид заявки, Код Группы заявки
		/// </summary>
		public string RequestType { get; set; }

		/// <summary>
		/// Номер Договора
		/// </summary>
		public string ContractNum { get; set; }

		/// <summary>
		/// Вид заявления на регистрацию договора
		/// </summary>
		public string ContractType { get; set; }

		/// <summary>
		/// Фактическая сумма оплаты
		/// </summary>
		public decimal TotalAmountNds { get; set; }

		/// <summary>
		/// Статус оплаты
		/// </summary>
		public string StatusName { get; set; }
		
		/// <summary>
		/// Дата зачтения оплаты
		/// </summary>
		public DateTimeOffset? CreditDate { get; set; }

		/// <summary>
		/// Исполнитель (кто зачёл)
		/// </summary>
		public string CreditUser { get; set; }

		/// <summary>
		/// Дата списания
		/// </summary>
		public DateTimeOffset? WriteOffDate { get; set; }

		/// <summary>
		/// Исполнитель (кто списал)
		/// </summary>
		public string WriteOffUser { get; set; }

		/// <summary>
		/// Код подразделения
		/// </summary>
		public string DepartmentName { get; set; }

		
		/// <summary>
		/// Дата и время удаления, изменения списанной оплаты 
		/// </summary>
		public DateTimeOffset? DeleteChangeDate { get; set; }

		/// <summary>
		/// ФИО и Должность сотрудника, удалившего, изменившего запись о списанной оплате 
		/// </summary>
		public string EmployeeAndPositonWhoChangedRecord { get; set; }

		/// <summary>
		/// Причина удаления, изменения записи о списанной оплате 
		/// </summary>
		public string DeleteChangeReason { get; set; }

		/// <summary>
		/// Изменен
		/// </summary>
		public PaymentInvoiveChangeFlag ChangeFlag { get; set; }


	}
}
