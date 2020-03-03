using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Model.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Dictionary.Base
{
    public class ListTreeNode
    {
        public class Query : IRequest<IEnumerable<BaseTreeNodeDto>>
        {
            public Query(string dicType)
            {
                DicType = dicType;
            }

            public string DicType { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<BaseTreeNodeDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IDicTypeResolver _dicTypeResolver;
            private readonly IMapper _mapper;

            public QueryHandler(
                NiisWebContext context,
                IDicTypeResolver dicTypeResolver,
                IMapper mapper)
            {
                _context = context;
                _dicTypeResolver = dicTypeResolver;
                _mapper = mapper;
            }

            public async Task<IEnumerable<BaseTreeNodeDto>> Handle(Query message)
            {
                var dicType = _dicTypeResolver.Resolve(message.DicType);
                if (!dicType.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IClassification<>)))
                {
                    throw new ArgumentException(
                        $"{message.DicType} not implements an interface {typeof(IClassification<>).Name}");
                }

                var dictionaries = await _context.Set(dicType).Cast<dynamic>().ToListAsync();
                return _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionaries.Where(d => d.ParentId == null));
            }
        }
    }
}