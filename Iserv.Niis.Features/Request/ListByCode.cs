using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Request
{
    public class ListByCode
    {
        public class Query : IRequest<IQueryable<IntellectualPropertySearchDto>>
        {
            public string Code { get; }
            public int Userid { get; }

            public Query(string code, int userid)
            {
                Code = code;
                Userid = userid;
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

            Task<IQueryable<IntellectualPropertySearchDto>> IAsyncRequestHandler<Query, IQueryable<IntellectualPropertySearchDto>>.Handle(Query message)
            {
                var stageCodes = new[]
                {
                    "B03.3.3",
                    "TM03.3.4",
                    "U03.4",
                    "PO03.5",
                    "NMPT03.4",
                    "TMI03.3.4",
                    "SA03.3.4"
                };
                var pdCode = message.Code.Substring(
                        message.Code.IndexOf(";", StringComparison.Ordinal)
                    )
                    .Split(';');

                var requests = _context.Requests
                    .Where(r => stageCodes.Contains(r.CurrentWorkflow.CurrentStage.Code)
                                //&& r.CurrentWorkflow.CurrentUserId == message.Userid
                                && pdCode.Contains(r.ProtectionDocType.Code));

                return Task.FromResult(requests.ProjectTo<IntellectualPropertySearchDto>());
            }
        }
    }
}
