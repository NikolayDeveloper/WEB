using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class GenerateRequestNumber
    {
        public class Command : IRequest<string>
        {
            public Command(int requestId)
            {
                RequestId = requestId;
            }

            public int RequestId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, string>
        {
            private readonly NiisWebContext _context;
            private readonly INumberGenerator _generator;

            public CommandHandler(
                NiisWebContext context,
                INumberGenerator generator)
            {
                _context = context;
                _generator = generator;
            }

            public async Task<string> Handle(Command message)
            {
                var request = _context.Requests
                    .Include(r => r.ProtectionDocType)
                    .Include(r => r.CurrentWorkflow)
                    .Include(r => r.RequestType)
                    .SingleOrDefault(r => r.Id == message.RequestId);
                if (request == null)
                {
                    return string.Empty;
                }

                await _generator.GenerateRequestNum(request);
                return request.RequestNum;
            }
        }
    }
}