using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch.Similarities
{
    public class Update
    {
        public class Command : IRequest<Unit>
        {
            public Command(ExpertSearchSimilarDto[] similarResults, Owner.Type ownerType, int ownerId, string keywords)
            {
                SimilarResults = similarResults;
                OwnerType = ownerType;
                OwnerId = ownerId;
                Keywords = keywords;
            }

            public ExpertSearchSimilarDto[] SimilarResults { get; set; }
            public Owner.Type OwnerType { get; }
            public int OwnerId { get; }
            public string Keywords { get; }
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
                foreach (var dto in message.SimilarResults)
                {
                    var similarity = _context.ExpertSearchSimilarities
                        .Single(ess => ess.Id == dto.Id);
                    similarity.ProtectionDocFormula = dto.ProtectionDocFormula;
                    similarity.ProtectionDocCategory = dto.ProtectionDocCategory;
                    _context.ExpertSearchSimilarities.Update(similarity);
                    await _context.SaveChangesAsync();
                }

                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                        var request = _context.Requests.Single(r => r.Id == message.OwnerId);
                        request.ExpertSearchKeywords = message.Keywords;
                        await _context.SaveChangesAsync();
                        break;
                    default:
                        break;
                }

                return await Unit.Task;
            }
        }
    }
}
