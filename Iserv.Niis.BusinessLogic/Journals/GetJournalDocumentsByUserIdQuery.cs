using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Journal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Journals
{
    public class GetJournalDocumentsByUserIdQuery : BaseQuery
    {
        private readonly IMapper _mapper;

        public GetJournalDocumentsByUserIdQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IPagedList<IntellectualPropertyDto> Execute(long userId, HttpRequest httpRequest)
        {
            var requestRepository = Uow.GetRepository<Request>();
            var requestsQuery = requestRepository
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.CurrentWorkflow)
                .Include(r => r.ProtectionDocType)
                .Where(r => r.CurrentWorkflowId != null && r.CurrentWorkflow.CurrentUserId == userId)
                .Select(IntellectualPropertyDto.MapFromRequest);

            var contractRepository = Uow.GetRepository<Contract>();
            var contractQuery = contractRepository
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.ProtectionDocType)
                .Where(r => r.CurrentWorkflowId != null && r.CurrentWorkflow.CurrentUserId == userId)
                .Select(IntellectualPropertyDto.MapFromContract);

            var protectionDocumentRepository = Uow.GetRepository<ProtectionDoc>();
            var parallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflowsQuery = parallelWorkflowRepository.AsQueryable();
            var protectionDocumentQuery = protectionDocumentRepository
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.Type)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Where(r => r.CurrentWorkflowId != null && r.CurrentWorkflow.CurrentUserId == userId ||
                    r.Workflows.Any(w => parallelWorkflowsQuery.Any(pw => pw.ProtectionDocWorkflowId == w.Id && !pw.IsFinished) && w.CurrentUserId == userId)
                )
                .Select(p => new IntellectualPropertyDto.ProtectionDocWithCurentUserId{ ProtectionDoc = p, UserId = userId })
                .Select(IntellectualPropertyDto.MapFromProtectionDocument);

            var unionQuery = requestsQuery
                .Union(contractQuery)
                .Union(protectionDocumentQuery);
            var filterdQuery = unionQuery.Filter(httpRequest.Query);

            var sortedQuery = filterdQuery;
            if (httpRequest.Query.ContainsKey("_sort"))
            {
                sortedQuery = sortedQuery.Sort(httpRequest.Query);
            }
            else
            {
                sortedQuery = sortedQuery.OrderByDescending(r => r.DateCreate);
            }

            return sortedQuery.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}
