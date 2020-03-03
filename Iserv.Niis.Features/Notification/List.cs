using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;
using MediatR;

namespace Iserv.Niis.Features.Notification
{
    public class List
    {
        public class Query : IRequest<List<DicNotificationStatus>>
        {
            public Query(Owner.Type ownerType, int ownerId, int documentId)
            {
                OwnerType = ownerType;
                OwnerId = ownerId;
                DocumentId = documentId;
            }

            public Owner.Type OwnerType { get; }
            public int OwnerId { get; }
            public int DocumentId { get; }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, List<DicNotificationStatus>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<List<DicNotificationStatus>> Handle(Query message)
            {
                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                    {
                        var notificationsStatuses = _context.RequestNotificationStatuses
                            .Where(rn => rn.RequestId.Equals(message.OwnerId))
                            .Select(rn => rn.NotificationStatus)
                            .ToList();

                        return Task.FromResult(notificationsStatuses);
                    }
                    case Owner.Type.Contract:
                    {
                        var notificationsStatuses = _context.ContractNotificationStatuses
                            .Where(cn => cn.ContractId.Equals(message.OwnerId))
                            .Select(cn => cn.NotificationStatus)
                            .ToList();

                        return Task.FromResult(notificationsStatuses);
                    }
                    case Owner.Type.Material:
                    {
                        var notificationsStatuses = _context.DicNotificationStatuses
                            .Where(ns => ns.Documents
                                .Any(dn => dn.DocumentId == message.DocumentId))
                            .Select(ns => new DicNotificationStatus
                            {
                                Id = ns.Id,
                                Code = ns.Code,
                                NameRu = ns.NameRu,
                                NameKz = ns.NameKz,
                                NameEn = ns.NameEn
                            }).ToList();

                        return Task.FromResult(notificationsStatuses);
                    }
                }

                return null;
            }
        }
    }
}
