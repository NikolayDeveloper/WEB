using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Контрагенты
    /// Примечание: Структура хранения информации о клиентах
    /// Модуль: Контрагенты 
    /// </summary>
    //[Table("WT_CUSTOMER", "dbo")]
    public class WtCustomer
    {
        /// <summary>
		/// Наименование: ID
		/// Тип данных БД: int NOT NULL
		/// </summary>
		////[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Дата занесения в базу данных
        /// Тип данных БД: datetime NULL
        /// </summary>
        ////[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Метка обновления
        /// Тип данных БД: datetime NULL
        /// </summary>
        ////[ColumnName("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Наименование: Тип
        /// Тип данных БД: int NOT NULL
        /// </summary>
        ////[ColumnName("TYPE_ID")]
        public int TypeId { get; set; }

        /// <summary>
        /// Наименование: ИИН/БИН
        /// Тип данных БД: nvarchar(12) NULL
        /// </summary>
        ////[ColumnName("flXIN")]
        public string Xin { get; set; }

        /// <summary>
        /// Наименование: Наименование (EN)
        /// Тип данных БД: nvarchar(500) NULL
        /// </summary>
        ////[ColumnName("CUS_NAME_ML_EN")]
        public string CusNameMlEn { get; set; }

        /// <summary>
        /// Наименование: Наименование (RU)
        /// Тип данных БД: nvarchar(500) NULL
        /// </summary>
        ////[ColumnName("CUS_NAME_ML_RU")]
        public string CusNameMlRu { get; set; }

        /// <summary>
        /// Наименование: Наименование (KZ)
        /// Тип данных БД: nvarchar(500) NULL
        /// </summary>
        ////[ColumnName("CUS_NAME_ML_KZ")]
        public string CusNameMlKz { get; set; }

        /// <summary>
        /// Наименование: РНН
        /// Тип данных БД: nvarchar(12) NULL
        /// </summary>
        ////[ColumnName("RTN")]
        public string Rtn { get; set; }

        /// <summary>
        /// Наименование: Телефон
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("PHONE")]
        public string Phone { get; set; }

        /// <summary>
        /// Наименование: Факс
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("FAX")]
        public string Fax { get; set; }

        /// <summary>
        /// Наименование: e-mail
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("EMAIL")]
        public string Email { get; set; }

        /// <summary>
        /// Наименование: Номер регистрации юридического лица
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("JUR_REG_NUMBER")]
        public string JurRegNumber { get; set; }

        /// <summary>
        /// Наименование: Контактное лицо
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("CONTACT_FACE")]
        public string ContactFace { get; set; }

        /// <summary>
        /// Наименование: ПП: Регистрационный код
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("ATT_CODE")]
        public string AttCode { get; set; }

        /// <summary>
        /// Наименование: ПП: Гос. Регистрация
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("ATT_STAT_REG")]
        public string AttStatReg { get; set; }

        /// <summary>
        /// Наименование: ПП: Дата Гос. Регистрации
        /// Тип данных БД: date NULL
        /// </summary>
        ////[ColumnName("ATT_STAT_REG_DATE")]
        public DateTime? AttStatRegDate { get; set; }

        /// <summary>
        /// Наименование: ПП: Сфера Деятельности
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("ATT_SPHERE_WORK")]
        public string AttSphereWork { get; set; }

        /// <summary>
        /// Наименование: ПП: Сфера Знаний
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("ATT_SPHERE_KNOW")]
        public string AttSphereKnow { get; set; }

        /// <summary>
        /// Наименование: ПП: Место Работы
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        ////[ColumnName("ATT_WORK_PLACE")]
        public string AttWorkPlace { get; set; }

        /// <summary>
        /// Наименование: ПП: Языки
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_LANG")]
        public string AttLang { get; set; }

        /// <summary>
        /// Наименование: ATT_DATE_CARD
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("ATT_DATE_CARD")]
        public DateTime? AttDateCard { get; set; }

        /// <summary>
        /// Наименование: ATT_DATE_DISCARD
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("ATT_DATE_DISCARD")]
        public DateTime? AttDateDiscard { get; set; }

        /// <summary>
        /// Наименование: ПП: Начало завершения деятельности
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("ATT_DATE_BEGIN_STOP")]
        public DateTime? AttDateBeginStop { get; set; }

        /// <summary>
        /// Наименование: ATT_DATE_END_STOP
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("ATT_DATE_END_STOP")]
        public DateTime? AttDateEndStop { get; set; }

        /// <summary>
        /// Наименование: ATT_SOME_DATE
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_SOME_DATE")]
        public string AttSomeDate { get; set; }

        /// <summary>
        /// Наименование: ПП: Платежное Поручение
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_PLATPOR")]
        public string AttPlatpor { get; set; }

        /// <summary>
        /// Наименование: ПП: Дата публикации
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("ATT_PUBLIC")]
        public DateTime? AttPublic { get; set; }

        /// <summary>
        /// Наименование: ПП: Дата публикации изменений
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_PUBLIC_REDEFINE")]
        public string AttPublicRedefine { get; set; }

        /// <summary>
        /// Наименование: ПП: Изменения
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_REDEFINE")]
        public string AttRedefine { get; set; }

        /// <summary>
        /// Наименование: ПП: Образование
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("ATT_EDUCATION")]
        public string AttEducation { get; set; }

        /// <summary>
        /// Наименование: Код страны
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("COUNTRY_ID")]
        public int? CountryId { get; set; }

        /// <summary>
        /// Наименование: Адрес
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("ADDRESS_ID")]
        public int? AddressId { get; set; }

        /// <summary>
        /// Наименование: Логин
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("LOGIN")]
        public string Login { get; set; }

        /// <summary>
        /// Наименование: Пароль
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PASSWORD_")]
        public string Password { get; set; }

        /// <summary>
        /// Наименование: Подпись
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SUBSCRIPT")]
        public string Subscript { get; set; }

        /// <summary>
        /// Наименование: flApplicantsInfo
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flApplicantsInfo")]
        public string FlApplicantsInfo { get; set; }

        /// <summary>
        /// Наименование: flCertificateSeries
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flCertificateSeries")]
        public string FlCertificateSeries { get; set; }

        /// <summary>
        /// Наименование: flCertificateNumber
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flCertificateNumber")]
        public string FlCertificateNumber { get; set; }

        /// <summary>
        /// Наименование: flRegDate
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("flRegDate")]
        public DateTime? FlRegDate { get; set; }

        /// <summary>
        /// Наименование: flOpf
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flOpf")]
        public string FlOpf { get; set; }

        /// <summary>
        /// Наименование: flPowerAttorneyFullNum
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flPowerAttorneyFullNum")]
        public string FlPowerAttorneyFullNum { get; set; }

        /// <summary>
        /// Наименование: flPowerAttorneyDateIssue
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("flPowerAttorneyDateIssue")]
        public DateTime? FlPowerAttorneyDateIssue { get; set; }

        /// <summary>
        /// Наименование: flNotaryName
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flNotaryName")]
        public string FlNotaryName { get; set; }

        /// <summary>
        /// Наименование: flShortDocContent
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flShortDocContent")]
        public string FlShortDocContent { get; set; }

        /// <summary>
        /// Наименование: Наименование (EN) (сокращенное)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("CUS_NAME_ML_EN_long")]
        public string CusNameMlEnLong { get; set; }

        /// <summary>
        /// Наименование: Наименование (RU)  (сокращенное)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("CUS_NAME_ML_RU_long")]
        public string CusNameMlRuLong { get; set; }

        /// <summary>
        /// Наименование: Наименование (KZ)  (сокращенное)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("CUS_NAME_ML_KZ_long")]
        public string CusNameMlKzLong { get; set; }

        /// <summary>
        /// Наименование: Субъект малого и среднего бизнеса
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("flIsSMB")]
        public string FlIsSmb { get; set; }
    }
}