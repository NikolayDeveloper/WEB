using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Model.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Dictionary.SelectOption
{
    public class Single
    {
        public class Query : IRequest<SelectOptionDto>
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

        public class QueryHandler : IAsyncRequestHandler<Query, SelectOptionDto>
        {
            private readonly NiisWebContext _context;
            private readonly IDicTypeResolver _dicTypeResolver;

            public QueryHandler(NiisWebContext context, IDicTypeResolver dicTypeResolver)
            {
                _context = context;
                _dicTypeResolver = dicTypeResolver;
            }

            async Task<SelectOptionDto> IAsyncRequestHandler<Query, SelectOptionDto>.Handle(Query message)
            {
                var dicType = _dicTypeResolver.Resolve(message.Dictype);
                var dictionaries = _context.Set(dicType);
                var dto = await dictionaries.ProjectTo<SelectOptionDto>().SingleOrDefaultAsync(d => d.Id == message.Id);
                if (dto == null)
                    throw new DataNotFoundException(nameof(dicType),
                        DataNotFoundException.OperationType.Read, message.Id);

                return dto;
            }
        }
    }
}