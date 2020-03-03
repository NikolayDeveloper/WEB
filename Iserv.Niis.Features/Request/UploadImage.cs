using System;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class UploadImage
    {
        public class Command : IRequest<string>
        {
            public Command(int requestId, IFormFile file)
            {
                RequestId = requestId;
                File = file;
            }

            public int RequestId { get; }

            public IFormFile File { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, string>
        {
            private readonly NiisWebContext _context;
            private readonly ILogoUpdater _logoUpdater;

            public CommandHandler(NiisWebContext context, ILogoUpdater logoUpdater)
            {
                _context = context;
                _logoUpdater = logoUpdater;
            }

            public async Task<string> Handle(Command message)
            {
                var requestId = message.RequestId;
                var request = await _context.Requests
                    .SingleOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                        DataNotFoundException.OperationType.Update, requestId);

                _logoUpdater.Update(request, message.File);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }

                return $"/api/requests/{message.RequestId}/image?{DateTimeOffset.Now.Ticks}";
            }
        }
    }
}