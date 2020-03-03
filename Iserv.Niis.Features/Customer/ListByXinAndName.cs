using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Subject;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestSharp.Extensions;

namespace Iserv.Niis.Features.Customer
{
    public class ListByXinAndName
    {
        public class Query : IRequest<IQueryable<SubjectDto>>
        {
            public string Xin { get; }
            public string Name { get; }
            public string IsPatentAttorney { get; }
            public string RegNumber { get; }

            public Query(string xin, string name, string isPatentAttorney, string regNumber)
            {
                Xin = xin;
                Name = name;
                IsPatentAttorney = isPatentAttorney;
                RegNumber = regNumber;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<SubjectDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<SubjectDto>> Handle(Query message)
            {
                var isPatentAttorney = Convert.ToBoolean(message.IsPatentAttorney);
                return Task.FromResult(_context.DicCustomers.Include(c => c.Type)
                    .Where(c => (!message.Xin.HasValue() || c.Xin.ToLower().Contains(message.Xin)) &&
                                (!message.Name.HasValue() || c.NameRu.ToLower().Contains(message.Name.ToLower())) &&
                                (!message.RegNumber.HasValue() || c.PowerAttorneyFullNum.ToLower()
                                     .Contains(message.RegNumber.ToLower())) &&
                                (isPatentAttorney ? c.PowerAttorneyFullNum != null : c.PowerAttorneyFullNum == null))
                    .ProjectTo<SubjectDto>());
            }
        }
    }
}
