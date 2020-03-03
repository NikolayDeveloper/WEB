using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Journal;
using MediatR;

namespace Iserv.Niis.Features.Journal.StaffTasks
{
    public class List
    {
        public class Query : IRequest<IQueryable<StaffTaskDto>>
        {
        }
        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<StaffTaskDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            Task<IQueryable<StaffTaskDto>> IAsyncRequestHandler<Query, IQueryable<StaffTaskDto>>.Handle(Query message)
            {
                //TODO: заменить когда будут данные
                var mockData = Enumerable.Range(0, 200)
                    .Select(x => new StaffTaskDto()
                    {
                        Id = x,
                        FullName = "user " + x,
                        Incoming = new Random(x).Next(5, 50),
                        Executed = new Random(x + 2).Next(5, 50),
                        OnJob = new Random(x + 3).Next(5, 50),
                        NotOnJob = new Random(x + 4).Next(5, 50),
                        Overdue = new Random(x + 5).Next(5, 50),
                        Outgoing = new Random(x + 6).Next(5, 50),
                    }).AsQueryable();
                return Task.FromResult(mockData);
            }
        }
    }
}
