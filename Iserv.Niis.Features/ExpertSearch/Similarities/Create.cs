using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch.Similarities
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {
            public Command(int requestId, ExpertSearchSimilarDto[] similarResults)
            {
                RequestId = requestId;
                SimilarResults = similarResults;
            }

            public int RequestId { get; set; }
            public ExpertSearchSimilarDto[] SimilarResults { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command message)
            {
                try
                {
                    var oldExpertSearchSimilars = await _context.ExpertSearchSimilarities
                        .Where(x => x.RequestId == message.RequestId)
                        .ToListAsync();
                    var expertSearchSimilars =
                        _mapper.Map<IEnumerable<ExpertSearchSimilarDto>, IEnumerable<ExpertSearchSimilar>>(
                            message.SimilarResults);

                    _context.ExpertSearchSimilarities.RemoveRange(oldExpertSearchSimilars);
                    _context.ExpertSearchSimilarities.AddRange(expertSearchSimilars);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return await Unit.Task;
            }
        }
    }
}