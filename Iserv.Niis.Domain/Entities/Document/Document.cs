using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Entities.Request;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Документ
    /// </summary>
    public class Document : Entity<int>, IHaveBarcode, IHaveConcurrencyToken, ISoftDeletable
    {
        public Document()
        {
            Requests = new HashSet<RequestDocument>();
            Contracts = new HashSet<ContractDocument>();
            ProtectionDocs = new HashSet<ProtectionDocDocument>();
            
            AdditionalAttachments = new HashSet<Attachment>();
            Workflows = new HashSet<DocumentWorkflow>();
            NotificationStatuses = new HashSet<DocumentNotificationStatus>();
            Comments = new HashSet<DocumentComment>();
            DocumentLinks = new HashSet<DocumentLink>();
        }
        public Document(DocumentType documentType) : this()
        {
            DocumentType = documentType;
        }
        public DateTimeOffset DateUpdate { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        //public int? CurrentWorkflowId { get; set; }
        //public DocumentWorkflow CurrentWorkflow { get; set; }

        [NotMapped]
        public ICollection<DocumentWorkflow> CurrentWorkflows { get { return Workflows.Where(d => d.IsCurent).ToList(); } }

        public DocumentContent Content { get; set; }
        public DicDocumentStatus Status { get; set; }
        public int? StatusId { get; set; }


        #region Incoming (Outgoing)    

        public int TypeId { get; set; }
        public DicDocumentType Type { get; set; }
        public string DocumentNum { get; set; }
        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }
        public int? DivisionId { get; set; }
        public DicDivision Division { get; set; }

        /// <summary>
        /// Отметка контроля
        /// </summary>
        public bool? ControlMark { get; set; }


        /// <summary>
        /// Дата контроля
        /// </summary>
        public DateTimeOffset? ControlDate { get; set; }

        /// <summary>
        /// Резолюция по продлению даты контроля\снятию с контроля
        /// </summary>
        public string ResolutionExtensionControlDate { get; set; }

        /// <summary>
        /// Снят с контроля
        /// </summary>
        public bool? OutOfControl { get; set; }

        /// <summary>
        /// Дата снятия с контроля
        /// </summary>
        public DateTimeOffset? DateOutOfControl { get; set; }

        /// <summary>
        /// Признак наличия платёжного документа
        /// </summary>
        public bool? IsHasPaymentDocument { get; set; }

        #endregion

        #region Outgoing

        public string IncomingNumber { get; set; }
        public string IncomingNumberFilial { get; set; }
        public int? AddresseeId { get; set; }
        public DicCustomer Addressee { get; set; }
        public string AddresseeAddress { get; set; }
        public string OutgoingNumber { get; set; }
        public DateTimeOffset? SendingDate { get; set; }
        public bool WasScanned { get; set; }

        public int? MainAttachmentId { get; set; }
        public Attachment MainAttachment { get; set; }

        public int? SendTypeId { get; set; }
        public DicReceiveType SendType { get; set; }

        /// <summary>
        /// Ссылка на воходящий в исходящем(Ответ на входящий) для завершения входящего при присвоении рег. номера.
        /// </summary>
        public int? IncomingAnswerId { get; set; }
        public Document IncomingAnswer { get; set; }

        /// <summary>
        /// № счёта на оплату
        /// </summary>
        public string NumberForPayment { get; set; }

        /// <summary>
        /// Дата счёта
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }

        /// <summary>
        /// № входящего документа
        /// Номер входящего документа с которым связан исходящий документ
        /// TODO пока строка, должно замениться на ссылку
        /// </summary>
        public string IncomingDocumentNumber { get; set; }

        /// <summary>
        /// Трэк номер
        /// </summary>
        public string TrackNumber { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public int? PaymentInvoiceId { get; set; }
        public PaymentInvoice PaymentInvoice { get; set; }

        #endregion

        public ICollection<Attachment> AdditionalAttachments { get; set; }
        public ICollection<DocumentWorkflow> Workflows { get; set; }
        public ICollection<RequestDocument> Requests { get; set; }


        public ICollection<ContractDocument> Contracts { get; set; }
        public ICollection<ProtectionDocDocument> ProtectionDocs { get; set; }
        public ICollection<DocumentNotificationStatus> NotificationStatuses { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public ICollection<DocumentComment> Comments { get; set; }

        /// <summary>
        /// Ссылшки на связанные документы
        /// </summary>
        public ICollection<DocumentLink> DocumentLinks { get; set; }

        /// <summary>
        /// Ссылка на документы которыке связанны со мной
        /// ТОЛЬКО ЧТЕНИЕ!!!!!
        /// </summary>
        public ICollection<DocumentLink> DocumentParentLinks { get; set; }

        public int Barcode { get; set; }
        public int? ReceiveTypeId { get; set; }
        public DicReceiveType ReceiveType { get; set; }

        public DocumentType DocumentType { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsFinished { get; set; }
        public int? BulletinId { get; set; }
        public Bulletin.Bulletin Bulletin { get; set; }
        public int? ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public int? AttachedPaymentsCount { get; set; }
    }
}