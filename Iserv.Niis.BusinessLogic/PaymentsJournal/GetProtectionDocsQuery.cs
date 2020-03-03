using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetProtectionDocsQuery : BaseQuery, IBasePaymentsJournalQuery<IQueryable<DocumentDto>>
    {
        private static readonly string[] AuthorCertificateDocumentTypes =
        {
            "PAT_AVT_CD_RASTENIE",
            "PAT_AVT_CD_ZHIVOD",
            "GR_TZ_SVID_DUPLIKAT",
            "SD_PAT",
            "PAT_AVT_PO",
            "PAT_AVT_IZ",
            "PAT_AVT_PM"
        };

        public IQueryable<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            return GetQuery(searchParameters)
                .Select(DocumentDto.FromProtectionDoc)
                .Sort(httpRequest.Query);
        }

        private IQueryable<SearchProtectionDocViewEntity> GetQuery(DocumentsSearchParametersDto searchParameters)
        {
            var protectionDocs = ApplySearch(Uow.GetRepository<ProtectionDoc>().AsQueryable().AsNoTracking(), searchParameters);

            var query = Uow.GetRepository<SearchProtectionDocViewEntity>().AsQueryable().AsNoTracking();

            return from pd in protectionDocs
                   join q in query on pd.Id equals q.Id
                   select q;
        }

        private static IQueryable<ProtectionDoc> ApplySearch(IQueryable<ProtectionDoc> protectionDocs, DocumentsSearchParametersDto searchParameters)
        {
            if (searchParameters.DocTypeId != null)
                protectionDocs = protectionDocs.Where(x => x.TypeId == searchParameters.DocTypeId);

            if (searchParameters.Barcode != null)
                protectionDocs = protectionDocs.Where(x => x.Barcode == searchParameters.Barcode.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.NameRu))
                protectionDocs = protectionDocs.Where(x => x.NameRu.Contains(searchParameters.NameRu));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameKz))
                protectionDocs = protectionDocs.Where(x => x.NameKz.Contains(searchParameters.NameKz));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameEn))
                protectionDocs = protectionDocs.Where(x => x.NameEn.Contains(searchParameters.NameEn));

            if (!string.IsNullOrWhiteSpace(searchParameters.ProtectionDocNumber))
                protectionDocs = protectionDocs.Where(x => x.GosNumber == searchParameters.ProtectionDocNumber);

            if (searchParameters.ProtectionDocMaintainYear != null)
                protectionDocs = protectionDocs.Where(x => x.MaintainDate.Value.Year == searchParameters.ProtectionDocMaintainYear);

            if (searchParameters.ProtectionDocValidDate != null)
                protectionDocs = protectionDocs.Where(x => x.ValidDate.Value.Date >= searchParameters.ProtectionDocValidDate.Value.Date
                                                           && x.ValidDate.Value.Date < searchParameters.ProtectionDocValidDate.Value.Date.AddDays(1));

            if (searchParameters.ProtectionDocExtensionDate != null)
                protectionDocs = protectionDocs.Where(x => x.ExtensionDate.Value.Date >= searchParameters.ProtectionDocExtensionDate.Value.Date
                                                           && x.ExtensionDate.Value.Date < searchParameters.ProtectionDocExtensionDate.Value.Date.AddDays(1));

            if (searchParameters.ProtectionDocStatusId != null)
                protectionDocs = protectionDocs.Where(x => x.StatusId == searchParameters.ProtectionDocStatusId.Value);

            if (searchParameters.ProtectionDocDate != null)
                protectionDocs = protectionDocs.Where(x => x.GosDate.Value.Date >= searchParameters.ProtectionDocDate.Value.Date
                                                           && x.GosDate.Value.Date < searchParameters.ProtectionDocDate.Value.Date.AddDays(1));

            if (searchParameters.ProtectionDocOutgoingDate != null)
                protectionDocs = protectionDocs.Where(x => x.OutgoingDate.Value.Date >= searchParameters.ProtectionDocOutgoingDate.Value.Date
                                                           && x.OutgoingDate.Value.Date < searchParameters.ProtectionDocOutgoingDate.Value.Date.AddDays(1));

            if (searchParameters.IcgsId != null)
                protectionDocs = protectionDocs.Where(x => x.IcgsProtectionDocs.Any(y => y.IcgsId == searchParameters.IcgsId.Value));

            if (searchParameters.SelectionAchieveTypeId != null)
                protectionDocs = protectionDocs.Where(x => x.SelectionAchieveTypeId == searchParameters.SelectionAchieveTypeId.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.DeclarantName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "1" && y.Customer.NameRu.Contains(searchParameters.DeclarantName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.PatentOwnerName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "3" && y.Customer.NameRu.Contains(searchParameters.PatentOwnerName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.AuthorName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "2" && y.Customer.NameRu.Contains(searchParameters.AuthorName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.PatentAttorneyName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "4" && y.Customer.NameRu.Contains(searchParameters.PatentAttorneyName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.CorrespondenceName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "12" && y.Customer.NameRu.Contains(searchParameters.CorrespondenceName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.ConfidantName))
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "6" && y.Customer.NameRu.Contains(searchParameters.ConfidantName)));

            if (searchParameters.IsNotMention)
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "2" && y.Customer.IsNotMention));

            if (searchParameters.ConfidantDateFrom != null)
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "6"
                                                                                             && y.DateBegin.Value.Date >= searchParameters.ConfidantDateFrom.Value.Date
                                                                                             && y.DateBegin.Value.Date < searchParameters.ConfidantDateFrom.Value.Date.AddDays(1)));

            if (searchParameters.ConfidantDateTo != null)
                protectionDocs = protectionDocs.Where(x => x.ProtectionDocCustomers.Any(y => y.CustomerRole.Code == "6"
                                                                                             && y.DateEnd.Value.Date >= searchParameters.ConfidantDateTo.Value.Date
                                                                                             && y.DateEnd.Value.Date < searchParameters.ConfidantDateTo.Value.Date.AddDays(1)));

            if (!string.IsNullOrWhiteSpace(searchParameters.AuthorCertificateNumber))
                protectionDocs = protectionDocs.Where(x => x.Documents.Any(y => AuthorCertificateDocumentTypes.Contains(y.Document.Type.Code)
                                                                                && y.Document.DocumentNum == searchParameters.AuthorCertificateNumber));

            if (!string.IsNullOrWhiteSpace(searchParameters.BulletinNumber))
                protectionDocs = protectionDocs.Where(x => x.Bulletins.Any(y => y.Bulletin.Number == searchParameters.BulletinNumber));

            if (searchParameters.BulletinDate != null)
                protectionDocs = protectionDocs.Where(x => x.Bulletins.Any(y => y.Bulletin.PublishDate.Value.Date >= searchParameters.BulletinDate.Value.Date
                                                                                && y.Bulletin.PublishDate.Value.Date < searchParameters.BulletinDate.Value.Date.AddDays(1)));

            return protectionDocs;
        }
    }
}