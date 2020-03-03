using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.EntitiesHistory.Patent;

namespace Iserv.Niis.Domain.Entities.Contract
{
    /// <summary>
    /// Договор
    /// </summary>
    public class Contract : Entity<int>, IHaveBarcode, IHistorySupport, IHaveConcurrencyToken
    {
        public Contract()
        {
            Documents = new HashSet<ContractDocument>();
            ProtectionDocs = new HashSet<ContractProtectionDocRelation>();
            RequestsForProtectionDoc = new HashSet<ContractRequestRelation>();
            Workflows = new HashSet<ContractWorkflow>();
            ContractCustomers = new HashSet<ContractCustomer>();
            PaymentInvoices = new HashSet<PaymentInvoice>();
            NotificationStatuses = new List<ContractNotificationStatus>();
        }

        #region Common

        public int Barcode { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int? StatusId { get; set; }
        public DicContractStatus Status { get; set; }
        public int? ApplicantTypeId { get; set; }
        public DicApplicantType ApplicantType { get; set; }
        public string ValidDate { get; set; } //STZ17
        public DateTimeOffset? ExtensionDate { get; set; } //STZ176
        public int? CategoryId { get; set; }
        public DicContractCategory Category { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public int? TypeId { get; set; }
        public DicContractType Type { get; set; }


        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
        public string PaperworkStateRegister { get; set; } //DVPP
        public int? CurrentWorkflowId { get; set; }
        public ContractWorkflow CurrentWorkflow { get; set; }
        public string RegistrationPlace { get; set; }
        public ICollection<ContractDocument> Documents { get; set; }
        public ICollection<ContractProtectionDocRelation> ProtectionDocs { get; set; }
        public ICollection<ContractRequestRelation> RequestsForProtectionDoc { get; set; }
        public ICollection<ContractWorkflow> Workflows { get; set; }
        public ICollection<ContractCustomer> ContractCustomers { get; set; }
        public ICollection<ContractNotificationStatus> NotificationStatuses { get; set; }
        public string Changes { get; set; } // Добавила, в десктопе нет поля
        public ICollection<PaymentInvoice> PaymentInvoices { get; set; }
        public int? FullExpertiseExecutorId { get; set; }
        public ApplicationUser FullExpertiseExecutor { get; set; }
        public bool IsRead { get; set; }
        public int? AddresseeId { get; set; }
        public DicCustomer Addressee { get; set; }
        public string AddresseeAddress { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }
        public int? DivisionId { get; set; }
        public DicDivision Division { get; set; }
        public int? MainAttachmentId { get; set; }
        public Attachment MainAttachment { get; set; }
        public string IncomingNumber { get; set; }
        public DateTimeOffset? IncomingDate { get; set; }
        public DateTimeOffset? ApplicationDateCreate { get; set; }
        #endregion

        #region RequestForContract

        public string ApplicationNum { get; set; }  //это идентичное поле ContractNum
        public string ContractNum { get; set; } //REQ_NUMBER_21
        public DateTimeOffset? RegDate { get; set; } //REQ_DATE_22
        public int? ReceiveTypeId { get; set; }
        public DicReceiveType ReceiveType { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; } // Добавила, в десктопе нет поля
        public string OutgoingNumber { get; set; } // Добавила, в старом вебе тоже есть поле в бд, на UI нет

        /// <summary>
        /// Отмета успешной отправки рег. номера в ЛК
        /// </summary>
        public bool? IsSyncContractNum { get; set; }
        #endregion

        #region Contract

        public string GosNumber { get; set; } //GOS_NUMBER_11
        public DateTimeOffset? GosDate { get; set; } //GOS_DATE_11
        public string NumberBulletin { get; set; } //NBY
        public DateTimeOffset? BulletinDate { get; set; } //DBY
        public DateTimeOffset? TerminateDate { get; set; } // Добавила, в десктопе нет поля

        #endregion

        [NotMapped]
        public string CustomerContact { get; set; } //PatentCustomerContact

        [NotMapped]
        public string CustomerConfidant { get; set; } //PatentCustomerConfidant

        [NotMapped]
        public string FirstPartyWithAddress { get; set; } //PatentStorona1WithAddress

        [NotMapped]
        public string SecondPartyWithAddress { get; set; } //PatentStorona2WithAddress

        public Type GetHistoryEntity()
        {
            return typeof(PatentHistory);
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }

        public void MarkAsUnRead()
        {
            IsRead = false;
        }

        public override string ToString()
        {
            var result = string.Empty;
            result += DateCreate.ToString("dd.MM.yyyy");
            var name = string.Empty;
            if (!string.IsNullOrEmpty(NameRu))
                name = NameRu;
            else if (!string.IsNullOrEmpty(NameKz))
                name = NameKz;
            else if (!string.IsNullOrEmpty(NameEn))
                name = NameEn;
            if (!string.IsNullOrEmpty(ContractNum))
                result = (string.IsNullOrEmpty(result) ? string.Empty : result + " - ") + ContractNum;
            if (!string.IsNullOrEmpty(name))
                result = (string.IsNullOrEmpty(result) ? string.Empty : result + " - ") + name;
            if (string.IsNullOrEmpty(result))
                return Id + string.Empty;
            return result;
        }
    }
}