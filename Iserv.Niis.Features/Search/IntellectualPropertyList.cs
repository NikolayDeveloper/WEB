using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Search
{
    public class IntellectualPropertyList
    {
        public class Query : IRequest<IQueryable<IntellectualPropertySearchDto>>
        {

            public Query()
            {
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<IntellectualPropertySearchDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<IntellectualPropertySearchDto>> Handle(Query message)
            {
                var requests = _context.Requests
                    .ProjectTo<IntellectualPropertySearchDto>();
                var contracts = _context.Contracts
                    .ProjectTo<IntellectualPropertySearchDto>();
                var protectionDocs = _context.ProtectionDocs
                    .ProjectTo<IntellectualPropertySearchDto>();
                return Task.FromResult(requests.Concat(contracts.Concat(protectionDocs)));
            }
        }
    }
}
