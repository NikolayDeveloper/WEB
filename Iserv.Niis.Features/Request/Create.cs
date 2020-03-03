using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class Create
    {
        public class Command : IRequest<RequestDetailDto>
        {
            public Command(RequestDetailDto requestDetailDto, int userId)
            {
                RequestDetailDto = requestDetailDto;
                UserId = userId;
            }

            public RequestDetailDto RequestDetailDto { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.RequestDetailDto.Id).Equal(0);
                RuleFor(c => c.RequestDetailDto.AddresseeId).NotEmpty();
                RuleFor(c => c.RequestDetailDto.ProtectionDocTypeId).NotEmpty();
                RuleFor(c => c.RequestDetailDto.ReceiveTypeId).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, RequestDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly ICustomerUpdater _customerUpdater;
            private readonly IMapper _mapper;
            private readonly INumberGenerator _numberGenerator;
            private readonly IWorkflowApplier<Domain.Entities.Request.Request> _workflowApplier;

            public CommandHandler(
                IMapper mapper, 
                NiisWebContext context, 
                ICustomerUpdater customerUpdater,
                INumberGenerator numberGenerator,
                IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            {
                _mapper = mapper;
                _context = context;
                _customerUpdater = customerUpdater;
                _numberGenerator = numberGenerator;
                _workflowApplier = workflowApplier;
            }

            public async Task<RequestDetailDto> Handle(Command message)
            {
                var requestDto = message.RequestDetailDto;
                var request = _mapper.Map<RequestDetailDto, Domain.Entities.Request.Request>(requestDto);

                _numberGenerator.GenerateIncomingNum(request);
                _numberGenerator.GenerateBarcode(request);
                InitializeFieldsByDefault(request);

                _context.Requests.Add(request);

                try
                {
                    await _context.SaveChangesAsync();
                    
                    await _workflowApplier.ApplyInitialAsync(request, message.UserId);
                    await _context.SaveChangesAsync();
                    
                    var requestWithIncludes = await _context.Requests
                        .Include(r => r.Addressee)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.Route)
                        .SingleAsync(r => r.Id == request.Id);
                    return _mapper.Map<Domain.Entities.Request.Request, RequestDetailDto>(requestWithIncludes);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }


            private  void InitializeFieldsByDefault(Domain.Entities.Request.Request request)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var protectionDocCode = _context.DicProtectionDocTypes.Find(request.ProtectionDocTypeId)?.Code;
                if (string.IsNullOrEmpty(protectionDocCode))
                {
                    return;
                }
                if (DicProtectionDocType.Codes.Trademark.Equals(protectionDocCode) ||
                    DicProtectionDocType.Codes.InternationalTrademark.Equals(protectionDocCode))
                {
                    request.TypeTrademarkId = _context.DicTypeTrademarks
                        .SingleOrDefault(t => DicTypeTrademark.Codes.Combined.Equals(t.Code))?.Id;
                }
            }
        }
    }
}