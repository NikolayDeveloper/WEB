using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.EntitiesHistory.AccountingData;

namespace Iserv.Niis.Domain.Entities.AccountingData
{
    /// <summary>
    /// Информация о патентно поверенном
    /// </summary>
    public class CustomerAttorneyInfo : Entity<int>, IHistorySupport
    {
        /// <summary>
        /// Наименование: ИИН
        /// Тип данных БД: char(12) NOT NULL
        /// </summary>
        public string Iin { get; set; }

        /// <summary>
        /// Наименование: Фамилия
        /// Тип данных БД: nvarchar(50) NULL
        /// </summary>
        public string NameLast { get; set; }

        /// <summary>
        /// Наименование: Имя
        /// Тип данных БД: nvarchar(50) NULL
        /// </summary>
        public string NameFirst { get; set; }

        /// <summary>
        /// Наименование: Отчество
        /// Тип данных БД: nvarchar(50) NULL
        /// </summary>
        public string NameMiddle { get; set; }

        /// <summary>
        /// Наименование: № удостоверения патентного поверенного
        /// Тип данных БД: nvarchar(50) NOT NULL
        /// </summary>
        public string CertNum { get; set; }

        /// <summary>
        /// Наименование: Дата выдачи удостоверения патентного поверенного
        /// Тип данных БД: date NOT NULL
        /// </summary>
        public DateTime CertDate { get; set; }

        /// <summary>
        /// Наименование: Статус
        /// Тип данных БД: int NOT NULL
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Наименование: Дата переаттеста ции и свид. патентного поверенного
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string RevalidNote { get; set; }

        /// <summary>
        /// Наименование: Объекты промышленной собственности
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Ops { get; set; }

        /// <summary>
        /// Наименование: Область знании
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string KnowledgeArea { get; set; }

        /// <summary>
        /// Наименование: Языки
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Наименование: Место работы и должность
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Job { get; set; }

        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }

        /// <summary>
        /// Наименование: Город
        /// Тип данных БД: int NULL
        /// </summary>
        public int? LocationId { get; set; }
        public DicAddress Location { get; set; }

        /// <summary>
        /// Наименование: Адрес
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Наименование: Телефон/мобильный телефон
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Наименование: Телефакс
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Наименование: e-mail
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Наименование: Веб сайт
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Наименование: Примечание
        /// Тип данных БД: nvarchar(1000) NULL
        /// </summary>
        public string Note { get; set; }

        #region Relationships

        /// <summary>
        /// Контрагент
        /// </summary>
        public int? CustomerId { get; set; }
        public DicCustomer Customer { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(CustomerAttorneyInfoHistory);
        }
    }
}