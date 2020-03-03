using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Materials.Incoming;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Incoming;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials
{
    public class ListByOwner
    {
        public class Query : IRequest<IQueryable<MaterialItemDto>>
        {
            public Query(int ownerId, Owner.Type ownerType)
            {
                OwnerId = ownerId;
                OwnerType = ownerType;
            }
            public int OwnerId { get; }
            public Owner.Type OwnerType { get; }
        }
        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<MaterialItemDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<MaterialItemDto>> Handle(Query message)
            {
                IQueryable<MaterialItemDto> result = null;
                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                        result = _context.Documents
                            .Where(x => x.Requests.Any(r => r.RequestId == message.OwnerId)
                                        && !x.IsDeleted)
                            .ProjectTo<MaterialItemDto>();
                        break;
                    case Owner.Type.Contract:
                        result = _context.Documents
                            .Where(x => x.Contracts.Any(c => c.ContractId == message.OwnerId)
                                        && !x.IsDeleted)
                            .ProjectTo<MaterialItemDto>();
                        break;
                    case Owner.Type.ProtectionDoc:
                        result = _context.Documents
                            .Where(x => x.ProtectionDocs.Any(c => c.ProtectionDocId == message.OwnerId)
                                        && !x.IsDeleted)
                            .ProjectTo<MaterialItemDto>();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return Task.FromResult(result);
            }
        }
    }
}
