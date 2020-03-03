using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch.Similarities
{
	public class Delete
	{
		public class Command : IRequest<Unit>
		{
			public Command(IEnumerable<int> similarities)
			{
			    Similarities = similarities;
			}
            public IEnumerable<int> Similarities { get; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
		}

		public class CommandHandler : IAsyncRequestHandler<Command, Unit>
		{
			private readonly NiisWebContext _context;

			public CommandHandler(NiisWebContext context)
			{
				_context = context;
			}

			public async Task<Unit> Handle(Command message)
			{
			    var protectionDocSimilarities = await _context.ExpertSearchSimilarities
			        .Where(x => message.Similarities.Contains(x.Id))
			        .ToListAsync();

				// Remove
					_context.ExpertSearchSimilarities.RemoveRange(protectionDocSimilarities);
					await _context.SaveChangesAsync();

				return await Unit.Task;
			}
		}
	}
}