using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Iserv.Niis.Features.Dictionary.Base
{
    public class List
    {
        public class Query : IRequest<IQueryable>
        {
            public Query(string dictype)
            {
                Dictype = dictype;
                Selector = Selector.GetSelector(SelectMode.All);
            }

            public Query(string dictype, string[] codes)
            {
                Dictype = dictype;
                Codes = codes;
                Selector = Selector.GetSelector(SelectMode.ByCodes);
            }

            public string Dictype { get; }
            public string[] Codes { get; }
            internal Selector Selector { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable>
        {
            private readonly NiisWebContext _context;
            private readonly IDicTypeResolver _dicTypeResolver;

            public QueryHandler(NiisWebContext context, IDicTypeResolver dicTypeResolver)
            {
                _context = context;
                _dicTypeResolver = dicTypeResolver;
            }

            Task<IQueryable> IAsyncRequestHandler<Query, IQueryable>.Handle(Query message)
            {
                return message.Selector.Handle(_dicTypeResolver, _context, message);
            }
        }

        internal enum SelectMode
        {
            All,
            ByCodes
        }

        internal abstract class Selector
        {
            internal static Selector GetSelector(SelectMode selectMode)
            {
                switch (selectMode)
                {
                    case SelectMode.All:
                        return new SelectorAll();
                    case SelectMode.ByCodes:
                        return new SelectorByCode();
                    default:
                        return null;
                }
            }

            internal abstract Task<IQueryable> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message);
        }

        internal class SelectorAll: Selector
        {
            internal override Task<IQueryable> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message)
            {
                var dicType = dicTypeResolver.Resolve(message.Dictype);
                var dictionaries = context.Set(dicType);

                return Task.FromResult(dictionaries);
            }
        }

        internal class SelectorByCode: Selector
        {
            internal override Task<IQueryable> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message)
            {
                var dicType = dicTypeResolver.Resolve(message.Dictype);
                var dictionaries = context.Set(dicType).Cast<DictionaryEntity<int>>();

                return Task.FromResult(
                    dictionaries.Where(d => message.Codes.Contains(d.Code)) as IQueryable);
            }
        }
    }
}