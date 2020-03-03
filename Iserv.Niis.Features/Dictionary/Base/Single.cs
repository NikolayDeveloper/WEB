using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using MediatR;

namespace Iserv.Niis.Features.Dictionary.Base
{
    public class Single
    {
        public class Query : IRequest<IDictionaryEntity<int>>
        {
            public Query(string dictype, int id)
            {
                Dictype = dictype;
                Id = id;
            }

            public string Dictype { get; }
            public int Id { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IDictionaryEntity<int>>
        {
            private readonly NiisWebContext _context;
            private readonly IDicTypeResolver _dicTypeResolver;

            public QueryHandler(NiisWebContext context, IDicTypeResolver dicTypeResolver)
            {
                _context = context;
                _dicTypeResolver = dicTypeResolver;
            }

            Task<IDictionaryEntity<int>> IAsyncRequestHandler<Query, IDictionaryEntity<int>>.Handle(Query message)
            {
                var dicType = _dicTypeResolver.Resolve(message.Dictype);
                var dictionaries = _context.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
                var dictionary = dictionaries?.SingleOrDefault(d => d.Id == message.Id);

                //var dto = await dictionaries.ProjectTo<BaseDictionaryDto>().SingleOrDefaultAsync(d => d.Id == message.Id);
                if (dictionary == null)
                    throw new DataNotFoundException(nameof(dicType),
                        DataNotFoundException.OperationType.Read, message.Id);

                return Task.FromResult(dictionary);
            }
        }
    }
}