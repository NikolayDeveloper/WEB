using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ProtectionDoc
{
    public class GenerateGosNumber
    {
        public class Command : IRequest<object>
        {
            public Command(int[] ids)
            {
                Ids = ids;
            }
            
            public int[] Ids { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Ids.Length > 0);
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, object>
        {
            private readonly NiisWebContext _context;
            private readonly INumberGenerator _numberGenerator;

            public CommandHandler(NiisWebContext context,
                INumberGenerator numberGenerator)
            {
                _context = context;
                _numberGenerator = numberGenerator;
            }

            public async Task<object> Handle(Command message)
            {
                _numberGenerator.GenerateProtectionDocGosNumber(message.Ids);
                await _context.SaveChangesAsync();
                return null;
            }
        }
    }
}
