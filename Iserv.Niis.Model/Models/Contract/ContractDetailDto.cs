using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Subject;
using Iserv.Niis.Model.Models.Material;

namespace Iserv.Niis.Model.Models.Contract
{
    /// <summary>
    /// Карточка заявки на договор/договор
    /// </summary>
    public class ContractDetailDto
    {
        #region Common

        public int Id { get; set; }
        public int? Barcode { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int? StatusId { get; set; }
        public string ValidDate { get; set; } //STZ17
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public string TypeCode { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public string ProtectionDocTypeCode { get; set; }
        public int? CurrentWorkflowId { get; set; }
        public string RegistrationPlace { get; set; }
        public WorkflowDto CurrentWorkflow { get; set; }
        public PaymentInvoiceDto[] InvoiceDtos { get; set; }
        public SubjectDto[] Subjects { get; set; }
        public ContractProtectionDocRelationDto[] ProtectionDocRelations { get; set; }
        public ContractRequestRelationDto[] RequestRelations { get; set; }
        public string Changes { get; set; } // Добавила, в десктопе нет поля
        public int? FullExpertiseExecutorId { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public bool? WasScanned { get; set; }
        public string IncomingNumber { get; set; }
        public DateTimeOffset? IncomingDate { get; set; }
        public DateTimeOffset? ApplicationDateCreate { get; set; }
        #endregion

        #region RequestForContract

        public string ApplicationNum { get; set; }  //это идентичное поле ContractNum
        public string ContractNum { get; set; } //REQ_NUMBER_21
        public DateTimeOffset? RegDate { get; set; } //REQ_DATE_22
        public int? ReceiveTypeId { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; } // Добавила, в десктопе нет поля
        public string OutgoingNumber { get; set; } // Добавила, в старом вебе тоже есть поле в бд, на UI нет
        public MaterialOwnerDto[] Owners { get; set; }
        public MaterialOwnerDto[] ProtectionDocsOwners { get; set; }
        #endregion

        #region Contract

        public string GosNumber { get; set; } //GOS_NUMBER_11
        public DateTimeOffset? GosDate { get; set; } //GOS_DATE_11
        public string NumberBulletin { get; set; } //NBY
        public DateTimeOffset? BulletinDate { get; set; } //DBY
        public DateTimeOffset? TerminateDate { get; set; } // Добавила, в десктопе нет поля

        #endregion

        #region Referenced Values
        public int? DepartmentId { get; set; }
        public string DepartmentNameRu { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionNameRu { get; set; }
        public int? AddresseeId { get; set; }
        public string AddresseeXin { get; set; }
        public string AddresseeNameRu { get; set; }
        public string AddresseeAddress { get; set; }
        public string Apartment { get; set; }

        #endregion
    }
}