using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Iserv.Niis.Domain.Entities;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class DocumentDto
    {
        /// <summary> Категория документа </summary>
        public DocumentCategory DocumentCategory { get; set; }

        /// <summary> ИД </summary>
        public int Id { get; set; }

        /// <summary> Штрихкод </summary>
        [Display(Name = "Штрихкод")]
        public int Barcode { get; set; }

        /// <summary> Название вида ОПС\Договора </summary>
        [Display(Name = @"Название вида ОПС\Договора")]
        public string DocTypeName { get; set; }

        /// <summary> ИД </summary>
        public int? ProtectionDocTypeId { get; set; }

        /// <summary> Подвид заявки </summary>
        [Display(Name = "Подвид заявки")]
        public string RequestSubTypeName { get; set; }

        /// <summary> Тип заявки </summary>
        [Display(Name = "Тип заявки")]
        public string RequestTypeName { get; set; }

        /// <summary> Входящий номер Заявки\Договора </summary>
        [Display(Name = @"Входящий номер Заявки\Договора")]
        public string IncomingNumber { get; set; }

        /// <summary> Дата создания Заявки\ОД\Договора </summary>
        [Display(Name = @"Дата создания Заявки\ОД\Договора")]
        public DateTimeOffset DateCreate { get; set; }

        /// <summary> Регистрационный номер Заявки\Договора </summary>
        [Display(Name = @"Регистрационный номер Заявки\Договора")]
        public string RegNumber { get; set; }

        /// <summary> Тип доставки </summary>
        [Display(Name = "Тип доставки")]
        public string ReceiveTypeName { get; set; }

        /// <summary> Наименование на русском </summary>
        [Display(Name = "Наименование на русском")]
        public string NameRu { get; set; }

        /// <summary> Наименование на казахском </summary>
        [Display(Name = "Наименование на казахском")]
        public string NameKz { get; set; }

        /// <summary> Наименование на английском </summary>
        [Display(Name = "Наименование на английском")]
        public string NameEn { get; set; }

        /// <summary> Дата подачи Заявки\Заявления на регистрацию Договора </summary>
        [Display(Name = @"Дата подачи Заявки\Заявления на регистрацию Договора")]
        public DateTimeOffset? RegDate { get; set; }

        /// <summary> Номер охранного документа </summary>
        [Display(Name = "Номер охранного документа")]
        public string ProtectionDocNumber { get; set; }

        /// <summary> Год поддержания ОД </summary>
        [Display(Name = "Год поддержания ОД")]
        public int? ProtectionDocMaintainYear { get; set; }

        /// <summary> Срок действия ОД </summary>
        [Display(Name = "Срок действия ОД")]
        public DateTimeOffset? ProtectionDocValidDate { get; set; }

        /// <summary> Срок продления действия ОД </summary>
        [Display(Name = "Срок продления действия ОД")]
        public DateTimeOffset? ProtectionDocExtensionDate { get; set; }

        /// <summary> Статус заявки </summary>
        [Display(Name = "Статус заявки")]
        public string RequestStatusName { get; set; }

        /// <summary> Статус ОД </summary>
        [Display(Name = "Статус ОД")]
        public string ProtectionDocStatusName { get; set; }

        /// <summary> Тип селекционного достижения </summary>
        [Display(Name = "Тип селекционного достижения")]
        public string SelectionAchieveTypeName { get; set; }

        /// <summary> Селекционный номер </summary>
        [Display(Name = "Селекционный номер")]
        public string BreedingNumber { get; set; }

        /// <summary> Дата регистрации ОД в Госреестре </summary>
        [Display(Name = "Дата регистрации ОД в Госреестре")]
        public DateTimeOffset? ProtectionDocDate { get; set; }

        /// <summary> Дата отправки патента патентообладателю </summary>
        [Display(Name = "Дата отправки патента патентообладателю")]
        public DateTimeOffset? ProtectionDocOutgoingDate { get; set; }

        /// <summary> Дискламация (рус) </summary>
        [Display(Name = "Дискламация (рус)")]
        public string DisclaimerRu { get; set; }

        /// <summary> Дискламация (каз) </summary>
        [Display(Name = "Дискламация (каз)")]
        public string DisclaimerKz { get; set; }

        /// <summary> МПК </summary>
        [Display(Name = "МПК")]
        public string IcisCodes { get; set; }

        /// <summary> МКИТЗ </summary>
        [Display(Name = "МКИТЗ")]
        public string IcfemCodes { get; set; }

        /// <summary> МКПО </summary>
        [Display(Name = "МКПО")]
        public string IpcCodes { get; set; }

        /// <summary> МКТУ </summary>
        [Display(Name = "МКТУ")]
        public string IcgsCodes { get; set; }

        /// <summary> Заявители </summary>
        [Display(Name = "Заявители")]
        public string DeclarantNames { get; set; }

        /// <summary> Патентоообладатели </summary>
        [Display(Name = "Патентоообладатели")]
        public string PatentOwnerNames { get; set; }

        /// <summary> Авторы </summary>
        [Display(Name = "Авторы")]
        public string AuthorNames { get; set; }

        /// <summary> Патентноповеренные </summary>
        [Display(Name = "Патентноповеренные")]
        public string PatentAttorneyNames { get; set; }

        /// <summary> Адресаты для переписки </summary>
        [Display(Name = "Адресаты для переписки")]
        public string CorrespondenceNames { get; set; }

        /// <summary> Доверенные лица </summary>
        [Display(Name = "Доверенные лица")]
        public string ConfidantNames { get; set; }

        /// <summary> Наличие отказов от упоминания при публикации </summary>
        [Display(Name = "Наличие отказов от упоминания при публикации")]
        public bool AuthorsAreNotMentions { get; set; }

        /// <summary> № авторского свидетельства </summary>
        [Display(Name = "№ авторского свидетельства")]
        public string AuthorsCertificateNumbers { get; set; }

        /// <summary> Номер бюллетеня </summary>
        [Display(Name = "Номер бюллетеня")]
        public string NumberBulletin { get; set; }

        /// <summary> Изображение </summary>
        public string Image { get; set; }

        [IgnoreDataMember]
        public static Expression<Func<SearchRequestViewEntity, DocumentDto>> FromRequest = d => new DocumentDto
        {
            DocumentCategory = DocumentCategory.Request,
            Id = d.Id,
            Barcode = d.Barcode,
            ProtectionDocTypeId = d.ProtectionDocTypeId,
            DocTypeName = d.DocTypeName,
            RequestSubTypeName = d.RequestSubTypeName,
            RequestTypeName = d.RequestTypeName,
            IncomingNumber = d.IncomingNumber,
            DateCreate = d.DateCreate,
            RegNumber = d.RegNumber,
            ReceiveTypeName = d.ReceiveTypeName,
            NameRu = d.NameRu,
            NameKz = d.NameKz,
            NameEn = d.NameEn,
            RegDate = d.RegDate,
            ProtectionDocNumber = d.ProtectionDocNumber,
            ProtectionDocMaintainYear = d.ProtectionDocMaintainYear,
            ProtectionDocValidDate = d.ProtectionDocValidDate,
            ProtectionDocExtensionDate = d.ProtectionDocExtensionDate,
            RequestStatusName = d.RequestStatusName,
            ProtectionDocStatusName = d.ProtectionDocStatusName,
            SelectionAchieveTypeName = d.SelectionAchieveTypeName,
            BreedingNumber = d.BreedingNumber,
            ProtectionDocDate = d.ProtectionDocDate,
            ProtectionDocOutgoingDate = d.ProtectionDocOutgoingDate,
            DisclaimerRu = d.DisclaimerRu,
            DisclaimerKz = d.DisclaimerKz,
            IcisCodes = d.IcisCodes,
            IcfemCodes = d.IcfemCodes,
            IpcCodes = d.IpcCodes,
            IcgsCodes = d.IcgsCodes,
            DeclarantNames = d.DeclarantNames,
            PatentOwnerNames = d.PatentOwnerNames,
            AuthorNames = d.AuthorNames,
            PatentAttorneyNames = d.PatentAttorneyNames,
            CorrespondenceNames = d.CorrespondenceNames,
            ConfidantNames = d.ConfidantNames,
            AuthorsAreNotMentions = d.AuthorsAreNotMentions,
            AuthorsCertificateNumbers = d.AuthorsCertificateNumbers,
            NumberBulletin = d.NumberBulletin,
            Image = d.Image != null ? Convert.ToBase64String(d.Image) : null
        };

        [IgnoreDataMember]
        public static Expression<Func<SearchProtectionDocViewEntity, DocumentDto>> FromProtectionDoc = d => new DocumentDto
        {
            DocumentCategory = DocumentCategory.ProtectionDoc,
            Id = d.Id,
            Barcode = d.Barcode,
            ProtectionDocTypeId = d.ProtectionDocTypeId,
            DocTypeName = d.DocTypeName,
            RequestSubTypeName = d.RequestSubTypeName,
            RequestTypeName = d.RequestTypeName,
            IncomingNumber = d.IncomingNumber,
            DateCreate = d.DateCreate,
            RegNumber = d.RegNumber,
            ReceiveTypeName = d.ReceiveTypeName,
            NameRu = d.NameRu,
            NameKz = d.NameKz,
            NameEn = d.NameEn,
            RegDate = d.RegDate,
            ProtectionDocNumber = d.ProtectionDocNumber,
            ProtectionDocMaintainYear = d.ProtectionDocMaintainYear,
            ProtectionDocValidDate = d.ProtectionDocValidDate,
            ProtectionDocExtensionDate = d.ProtectionDocExtensionDate,
            RequestStatusName = d.RequestStatusName,
            ProtectionDocStatusName = d.ProtectionDocStatusName,
            SelectionAchieveTypeName = d.SelectionAchieveTypeName,
            BreedingNumber = d.BreedingNumber,
            ProtectionDocDate = d.ProtectionDocDate,
            ProtectionDocOutgoingDate = d.ProtectionDocOutgoingDate,
            DisclaimerRu = d.DisclaimerRu,
            DisclaimerKz = d.DisclaimerKz,
            IcisCodes = d.IcisCodes,
            IcfemCodes = d.IcfemCodes,
            IpcCodes = d.IpcCodes,
            IcgsCodes = d.IcgsCodes,
            DeclarantNames = d.DeclarantNames,
            PatentOwnerNames = d.PatentOwnerNames,
            AuthorNames = d.AuthorNames,
            PatentAttorneyNames = d.PatentAttorneyNames,
            CorrespondenceNames = d.CorrespondenceNames,
            ConfidantNames = d.ConfidantNames,
            AuthorsAreNotMentions = d.AuthorsAreNotMentions,
            AuthorsCertificateNumbers = d.AuthorsCertificateNumbers,
            NumberBulletin = d.NumberBulletin,
            Image = d.Image != null ? Convert.ToBase64String(d.Image) : null
        };

        [IgnoreDataMember]
        public static Expression<Func<SearchContractViewEntity, DocumentDto>> FromContract = d => new DocumentDto
        {
            DocumentCategory = DocumentCategory.Contract,
            Id = d.Id,
            Barcode = d.Barcode,
            ProtectionDocTypeId = d.ProtectionDocTypeId,
            DocTypeName = d.DocTypeName,
            RequestSubTypeName = d.RequestSubTypeName,
            RequestTypeName = d.RequestTypeName,
            IncomingNumber = d.IncomingNumber,
            DateCreate = d.DateCreate,
            RegNumber = d.RegNumber,
            ReceiveTypeName = d.ReceiveTypeName,
            NameRu = d.NameRu,
            NameKz = d.NameKz,
            NameEn = d.NameEn,
            RegDate = d.RegDate,
            ProtectionDocNumber = d.ProtectionDocNumber,
            ProtectionDocMaintainYear = d.ProtectionDocMaintainYear,
            ProtectionDocValidDate = d.ProtectionDocValidDate,
            ProtectionDocExtensionDate = d.ProtectionDocExtensionDate,
            RequestStatusName = d.RequestStatusName,
            ProtectionDocStatusName = d.ProtectionDocStatusName,
            SelectionAchieveTypeName = d.SelectionAchieveTypeName,
            BreedingNumber = d.BreedingNumber,
            ProtectionDocDate = d.ProtectionDocDate,
            ProtectionDocOutgoingDate = d.ProtectionDocOutgoingDate,
            DisclaimerRu = d.DisclaimerRu,
            DisclaimerKz = d.DisclaimerKz,
            IcisCodes = d.IcisCodes,
            IcfemCodes = d.IcfemCodes,
            IpcCodes = d.IpcCodes,
            IcgsCodes = d.IcgsCodes,
            DeclarantNames = d.DeclarantNames,
            PatentOwnerNames = d.PatentOwnerNames,
            AuthorNames = d.AuthorNames,
            PatentAttorneyNames = d.PatentAttorneyNames,
            CorrespondenceNames = d.CorrespondenceNames,
            ConfidantNames = d.ConfidantNames,
            AuthorsAreNotMentions = d.AuthorsAreNotMentions,
            AuthorsCertificateNumbers = d.AuthorsCertificateNumbers,
            NumberBulletin = d.NumberBulletin,
            Image = d.Image != null ? Convert.ToBase64String(d.Image) : null
        };
    }
}