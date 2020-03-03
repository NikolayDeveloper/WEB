using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch
{
    public class SingleImage
    {
	    public class Command : IRequest<byte[]>
	    {
		    public Command(int requestId, bool isPreview)
		    {
			    RequestId = requestId;
			    IsPreview = isPreview;
		    }

		    public int RequestId { get; }
		    public bool IsPreview { get; }
	    }

	    public class CommandValidator : AbstractValidator<Command>
	    {
	    }

	    public class CommandHandler : IAsyncRequestHandler<Command, byte[]>
	    {
		    private readonly NiisWebContext _context;

		    public CommandHandler(NiisWebContext context)
		    {
			    _context = context;
		    }

		    public async Task<byte[]> Handle(Command message)
			{
				var requestId = message.RequestId;
				var request = await _context.Requests.SingleOrDefaultAsync(r => r.ProtectionDocId == requestId);
				if (request == null)
					throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
						DataNotFoundException.OperationType.Update, requestId);

				return message.IsPreview ? request.PreviewImage : request.Image;
			}
	    }
    }
}

