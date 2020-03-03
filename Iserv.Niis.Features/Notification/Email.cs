using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Notification
{
    public class Email
    {
        public class Query : IRequest<Unit>
        {
            public Owner.Type OwherType { get; }
            public int OwnerId { get; }
            public int DocumentId { get; }
            public int UserId { get; }

            public Query(Owner.Type owherType, int ownerId, int documentId, int userId)
            {
                OwherType = owherType;
                OwnerId = ownerId;
                DocumentId = documentId;
                UserId = userId;
            }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, Unit>
        {
            private readonly NiisWebContext _context;
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
            private readonly INotificationSender _notificationSender;

            public QueryHandler(NiisWebContext context, 
                IDocumentGeneratorFactory templateGeneratorFactory, 
                INotificationSender notificationSender)
            {
                _context = context;
                _templateGeneratorFactory = templateGeneratorFactory;
                _notificationSender = notificationSender;
            }

            public Task<Unit> Handle(Query message)
            {
                var code = _context.Documents.Where(d => d.Id.Equals(message.DocumentId)).Select(d => d.Type.Code)
                    .SingleOrDefault();

                var input = _context.DocumentUserInputs.SingleOrDefault(ui => ui.DocumentId.Equals(message.DocumentId));
                var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(code);
                var generatedDocument = documentGenerator.Process(new Dictionary<string, object>
                {
                    {"UserId", message.UserId},
                    {"RequestId", message.OwnerId},
                    {"DocumentId", message.DocumentId},
                    {"UserInputFields", dto.Fields},
                    {"SelectedRequestIds", dto.SelectedRequestIds},
                    {"PageCount", dto.PageCount}
                });

                _notificationSender.ProcessEmailAsync(generatedDocument, message.DocumentId).Wait();
                //_context.SaveChangesAsync().Wait();

                //var notificationStatuses = _context.DocumentNotificationStatuses
                //    .Include(ds => ds.NotificationStatus)
                //    .Where(dn => dn.DocumentId.Equals(message.DocumentId))
                //    .Select(dn => dn.NotificationStatus);

                return Unit.Task;
            }
        }
    }
}
