using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Subject;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Customer
{
    public class List
    {
        public class Query : IRequest<IQueryable<SubjectDto>>
        {
            public Query(int ownerId, Owner.Type ownerType)
            {
                OwnerId = ownerId;
                OwnerType = ownerType;
            }

            public int OwnerId { get; }
            public Owner.Type OwnerType { get; }
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
                var ownerId = message.OwnerId;
                var ownerType = message.OwnerType;

                switch (ownerType)
                {
                    case Owner.Type.Request:
                        var requestCustomers = _context.RequestCustomers.Include(rc => rc.CustomerRole)
                            .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                            .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                            .Where(rc => rc.RequestId == ownerId);
                        return Task.FromResult(requestCustomers.ProjectTo<SubjectDto>());
                    case Owner.Type.Contract:
                        var contractCustomers = _context.ContractCustomers.Include(cc => cc.CustomerRole)
                            .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                            .Include(cc => cc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                            .Where(cc => cc.ContractId == ownerId);
                        return Task.FromResult(contractCustomers.ProjectTo<SubjectDto>());
                    case Owner.Type.ProtectionDoc:
                        var protectionDocCustomers = _context.ProtectionDocCustomers.Include(cc => cc.CustomerRole)
                            .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                            .Include(cc => cc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                            .Where(cc => cc.ProtectionDocId == ownerId);
                        return Task.FromResult(protectionDocCustomers.ProjectTo<SubjectDto>());
                    default:
                        throw new ApplicationException(string.Empty,
                            new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
                }
            }
        }
    }
}