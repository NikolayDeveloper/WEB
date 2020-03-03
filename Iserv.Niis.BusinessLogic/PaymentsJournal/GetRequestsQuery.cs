using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetRequestsQuery : BaseQuery, IBasePaymentsJournalQuery<IQueryable<DocumentDto>>
    {
        public IQueryable<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            return GetQuery(searchParameters)
                .Select(DocumentDto.FromRequest)
                .Sort(httpRequest.Query);
        }

        private IQueryable<SearchRequestViewEntity> GetQuery(DocumentsSearchParametersDto searchParameters)
        {
            var requests = ApplySearch(Uow.GetRepository<Request>().AsQueryable().AsNoTracking(), searchParameters);

            var query = Uow.GetRepository<SearchRequestViewEntity>().AsQueryable().AsNoTracking();

            return from r in requests
                   join q in query on r.Id equals q.Id
                   select q;
        }

        private static IQueryable<Request> ApplySearch(IQueryable<Request> requests, DocumentsSearchParametersDto searchParameters)
        {
            if (searchParameters.DocTypeId != null)
                requests = requests.Where(x => x.ProtectionDocTypeId == searchParameters.DocTypeId);

            if (searchParameters.Barcode != null)
                requests = requests.Where(x => x.Barcode == searchParameters.Barcode.Value);

            if (searchParameters.ReceiveTypeId != null)
                requests = requests.Where(x => x.ReceiveTypeId == searchParameters.ReceiveTypeId.Value);

            if (searchParameters.RequestSubTypeId != null)
                requests = requests.Where(x => x.RequestTypeId == searchParameters.RequestSubTypeId.Value);

            if (searchParameters.RequestTypeId != null)
                requests = requests.Where(x => x.ConventionTypeId == searchParameters.RequestTypeId.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.RequestIncomingNumber))
                requests = requests.Where(x => x.IncomingNumber == searchParameters.RequestIncomingNumber);

            if (!string.IsNullOrWhiteSpace(searchParameters.RequestRegNumber))
                requests = requests.Where(x => x.RequestNum == searchParameters.RequestRegNumber);

            if (searchParameters.RequestCreateDateFrom != null)
                requests = requests.Where(x => x.DateCreate.Date >= searchParameters.RequestCreateDateFrom.Value.Date);

            if (searchParameters.RequestCreateDateTo != null)
                requests = requests.Where(x => x.DateCreate.Date < searchParameters.RequestCreateDateTo.Value.Date.AddDays(1));

            if (searchParameters.RequestRegDateFrom != null)
                requests = requests.Where(x => x.RequestDate.Value.Date >= searchParameters.RequestRegDateFrom.Value.Date);

            if (searchParameters.RequestRegDateTo != null)
                requests = requests.Where(x => x.RequestDate.Value.Date < searchParameters.RequestRegDateTo.Value.Date.AddDays(1));

            if (searchParameters.RequestStatusId != null)
                requests = requests.Where(x => x.StatusId == searchParameters.RequestStatusId.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.NameRu))
                requests = requests.Where(x => x.NameRu.Contains(searchParameters.NameRu));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameKz))
                requests = requests.Where(x => x.NameKz.Contains(searchParameters.NameKz));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameEn))
                requests = requests.Where(x => x.NameEn.Contains(searchParameters.NameEn));

            if (searchParameters.IcgsId != null)
                requests = requests.Where(x => x.ICGSRequests.Any(y => y.IcgsId == searchParameters.IcgsId.Value));

            if (searchParameters.SelectionAchieveTypeId != null)
                requests = requests.Where(x => x.SelectionAchieveTypeId == searchParameters.SelectionAchieveTypeId.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.DeclarantName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "1" && y.Customer.NameRu.Contains(searchParameters.DeclarantName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.PatentOwnerName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "3" && y.Customer.NameRu.Contains(searchParameters.PatentOwnerName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.AuthorName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "2" && y.Customer.NameRu.Contains(searchParameters.AuthorName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.PatentAttorneyName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "4" && y.Customer.NameRu.Contains(searchParameters.PatentAttorneyName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.CorrespondenceName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "12" && y.Customer.NameRu.Contains(searchParameters.CorrespondenceName)));

            if (!string.IsNullOrWhiteSpace(searchParameters.ConfidantName))
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "6" && y.Customer.NameRu.Contains(searchParameters.ConfidantName)));

            if (searchParameters.IsNotMention)
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "2" && y.Customer.IsNotMention));

            if (searchParameters.ConfidantDateFrom != null)
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "6"
                                                                           && y.DateBegin.Value.Date >= searchParameters.ConfidantDateFrom.Value.Date
                                                                           && y.DateBegin.Value.Date < searchParameters.ConfidantDateFrom.Value.Date.AddDays(1)));

            if (searchParameters.ConfidantDateTo != null)
                requests = requests.Where(x => x.RequestCustomers.Any(y => y.CustomerRole.Code == "6"
                                                                           && y.DateEnd.Value.Date >= searchParameters.ConfidantDateTo.Value.Date
                                                                           && y.DateEnd.Value.Date < searchParameters.ConfidantDateTo.Value.Date.AddDays(1)));

            if (!string.IsNullOrWhiteSpace(searchParameters.BulletinNumber))
                requests = requests.Where(x => x.NumberBulletin == searchParameters.BulletinNumber);

            if (searchParameters.BulletinDate != null)
                requests = requests.Where(x => x.PublicDate.Value.Date >= searchParameters.BulletinDate.Value.Date
                                               && x.PublicDate.Value.Date < searchParameters.BulletinDate.Value.Date.AddDays(1));

            return requests;
        }
    }
}