using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetContractsQuery : BaseQuery, IBasePaymentsJournalQuery<IQueryable<DocumentDto>>
    {
        public IQueryable<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            return GetQuery(searchParameters)
                .Select(DocumentDto.FromContract)
                .Sort(httpRequest.Query);
        }

        private IQueryable<SearchContractViewEntity> GetQuery(DocumentsSearchParametersDto searchParameters)
        {
            var contracts = ApplySearch(Uow.GetRepository<Contract>().AsQueryable().AsNoTracking(), searchParameters);

            var query = Uow.GetRepository<SearchContractViewEntity>().AsQueryable().AsNoTracking();

            return from c in contracts
                   join q in query on c.Id equals q.Id
                   select q;
        }

        private static IQueryable<Contract> ApplySearch(IQueryable<Contract> contracts, DocumentsSearchParametersDto searchParameters)
        {
            if (searchParameters.DocTypeId != null)
                contracts = contracts.Where(x => x.TypeId == searchParameters.DocTypeId);

            if (searchParameters.Barcode != null)
                contracts = contracts.Where(x => x.Barcode == searchParameters.Barcode.Value);

            if (searchParameters.ReceiveTypeId != null)
                contracts = contracts.Where(x => x.ReceiveTypeId == searchParameters.ReceiveTypeId.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.NameRu))
                contracts = contracts.Where(x => x.NameRu.Contains(searchParameters.NameRu));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameKz))
                contracts = contracts.Where(x => x.NameKz.Contains(searchParameters.NameKz));

            if (!string.IsNullOrWhiteSpace(searchParameters.NameEn))
                contracts = contracts.Where(x => x.NameEn.Contains(searchParameters.NameEn));

            return contracts;
        }
    }
}