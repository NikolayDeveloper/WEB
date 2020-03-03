using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Internal;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models;
using MediatR;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Iserv.Niis.Features.Dictionary.SelectOption
{
    public class List
    {
        public class Query : IRequest<IQueryable<SelectOptionDto>>
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
                Selector = Selector.GetSelector(SelectMode.ByCode);
            }

            public string Dictype { get; }
            public string[] Codes { get; }
            internal Selector Selector { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        internal enum SelectMode
        {
            All,
            ByCode
        }

        internal abstract class Selector
        {
            internal static Selector GetSelector(SelectMode selectMode)
            {
                switch (selectMode)
                {
                    case SelectMode.All:
                        return new SelectorAll();
                    case SelectMode.ByCode:
                        return new SelectorByCode();
                    default:
                        return null;
                }
            }

            internal abstract Task<IQueryable<SelectOptionDto>> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message);
        }

        internal class SelectorAll : Selector
        {
            internal override Task<IQueryable<SelectOptionDto>> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message)
            {
                var dicType = dicTypeResolver.Resolve(message.Dictype);
                var dictionaries = context.Set(dicType);
                
                return Task.FromResult(dictionaries.ProjectTo<SelectOptionDto>());
            }
        }

        internal class SelectorByCode: Selector
        {
            internal override Task<IQueryable<SelectOptionDto>> Handle(IDicTypeResolver dicTypeResolver, NiisWebContext context, Query message)
            {
                var dicType = dicTypeResolver.Resolve(message.Dictype);

                if (message.Dictype.Equals(nameof(DicDocumentType)))
                {
                    return Task.FromResult(context.DicDocumentTypes
                        .Where(dt => dt.Classification.Code.StartsWith(message.Codes[0])
                                     && !dt.Classification.Code.Equals("01.01.01")
                                     && !dt.Classification.Code.Equals("01.02")
                                     && !dt.Classification.Code.Equals("001.06")
                        ).ProjectTo<SelectOptionDto>());
                }

                var dictionaries = context.Set(dicType).Cast<DictionaryEntity<int>>();

                return Task.FromResult(dictionaries.Where(d => message.Codes.Contains(d.Code))
                    .ProjectTo<SelectOptionDto>());
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<SelectOptionDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IDicTypeResolver _dicTypeResolver;

            public QueryHandler(NiisWebContext context, IDicTypeResolver dicTypeResolver)
            {
                _context = context;
                _dicTypeResolver = dicTypeResolver;
            }

            Task<IQueryable<SelectOptionDto>> IAsyncRequestHandler<Query, IQueryable<SelectOptionDto>>.Handle(Query message)
            {
                return message.Selector.Handle(_dicTypeResolver, _context, message);
            }
        }
    }
}