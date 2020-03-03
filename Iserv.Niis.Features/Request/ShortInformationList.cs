using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Request;
using MediatR;

namespace Iserv.Niis.Features.Request
{
    public class ShortInformationList
    {
        public class Query : IRequest<IQueryable<RequestItemDto>>
        {
            public Query(string protectionDocCode)
            {
                ProtectionDocCode = protectionDocCode;
            }

            public string ProtectionDocCode { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<RequestItemDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            Task<IQueryable<RequestItemDto>> IAsyncRequestHandler<Query, IQueryable<RequestItemDto>>.Handle(Query message)
            {
                var requests = _context.Requests
                    .Where(r => string.IsNullOrEmpty(message.ProtectionDocCode) ||
                                r.ProtectionDocType.Code == message.ProtectionDocCode);
                var requestItemDtos = requests.ProjectTo<RequestItemDto>();
                return Task.FromResult(requestItemDtos);
            }
        }
    }
}