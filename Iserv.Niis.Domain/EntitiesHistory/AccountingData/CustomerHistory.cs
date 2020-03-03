using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.AccountingData
{
    /// <summary>
    /// Контрагенты История(WT_CUSTOMER_HISTORY)
    /// </summary>
    public class CustomerHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int CustomerTypeId { get; set; }
        public string Xin { get; set; }
        public string Rnn { get; set; }
        public string Phone { get; set; }
        public string PhoneFax { get; set; }
        public string Email { get; set; }
        public string JurRegNumber { get; set; }
        public string ContactName { get; set; }
        public int? CountryId { get; set; }
        public int? AddressId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Subscript { get; set; }
        public string ApplicantsInfo { get; set; }
        public string CertificateSeries { get; set; }
        public string CertificateNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public string Opf { get; set; }
        public string PowerAttorneyFullNum { get; set; }
        public DateTimeOffset? PowerAttorneyDateIssue { get; set; }
        public string NotaryName { get; set; }
        public string ShortDocContent { get; set; }
        public string NameKzLong { get; set; }
        public string NameRuLong { get; set; }
        public string NameEnLong { get; set; }
    }
}