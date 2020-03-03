using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Materials
{
    public class GenerateOutgoingNumber
    {
        public class Command : IRequest<Document>
        {
            public Command(int documentId, int currentUserId)
            {
                DocumentId = documentId;
                CurrentUserId = currentUserId;
            }

            public int DocumentId { get; }
            public int CurrentUserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Document>
        {
            private readonly NiisWebContext _context;
            private readonly INumberGenerator _numberGenerator;
            private readonly INotificationTaskRegister _notificationTaskRegister;
            private readonly IGeneratedNumberApplier<Domain.Entities.Request.Request> _generatedNumberApplierForRequest;
            private readonly IGeneratedNumberApplier<Domain.Entities.Contract.Contract> _generatedNumberApplierForContract;
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

            public CommandHandler(NiisWebContext context, 
                    INumberGenerator numberGenerator,
                    IGeneratedNumberApplier<Domain.Entities.Request.Request> generatedNumberApplierForRequest,
                    IGeneratedNumberApplier<Domain.Entities.Contract.Contract> generatedNumberApplierForContract,
                    INotificationTaskRegister notificationTaskRegister,
                    IDocumentGeneratorFactory templateGeneratorFactory)
            {
                _context = context;
                _numberGenerator = numberGenerator;
                _generatedNumberApplierForRequest = generatedNumberApplierForRequest;
                _generatedNumberApplierForContract = generatedNumberApplierForContract;
                _notificationTaskRegister = notificationTaskRegister;
                _templateGeneratorFactory = templateGeneratorFactory;
            }

            public async Task<Document> Handle(Command message)
            {
                var document = await _context.Documents.SingleAsync(d => d.Id == message.DocumentId);

                _numberGenerator.GenerateOutgoingNum(document);
                document.SendingDate = DateTimeOffset.Now;
                await _generatedNumberApplierForRequest.ApplyAsync(document.Id);
                await _generatedNumberApplierForContract.ApplyAsync(document.Id);
                await _context.SaveChangesAsync();

                var doc = _context.Documents
                    .Include(d => d.Type)
                    .SingleOrDefault(d => d.Id == document.Id);

                var input = _context.DocumentUserInputs.SingleOrDefault(ui =>
                    ui.DocumentId == document.Id);

                var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(doc.Type.Code);
                var generatedDocument = documentGenerator.Process(new Dictionary<string, object>
                {
                    {"UserId", message.CurrentUserId},
                    {"RequestId", dto.OwnerId},
                    {"DocumentId", document.Id},
                    {"UserInputFields", dto.Fields},
                    {"SelectedRequestIds", dto.SelectedRequestIds},
                    {"PageCount", dto.PageCount}
                });

                await _notificationTaskRegister.RegisterDocumentAsync(document.Id, generatedDocument.File);
                await _context.SaveChangesAsync();

                return document;
            }
        }
    }
}
