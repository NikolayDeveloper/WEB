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
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ProtectionDoc
{
    public class Create
    {
        public class Command : IRequest<ProtectionDocDetailsDto>
        {
            public Command(ProtectionDocDetailsDto protectionDocDetailsDto, int userId)
            {
                ProtectionDocDetailsDto = protectionDocDetailsDto;
                UserId = userId;
            }

            public ProtectionDocDetailsDto ProtectionDocDetailsDto { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ProtectionDocDetailsDto.Id).Equal(0);
                RuleFor(c => c.ProtectionDocDetailsDto.TypeId).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ProtectionDocDetailsDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly INumberGenerator _numberGenerator;
            private readonly IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> _workflowApplier;

            public CommandHandler(
                IMapper mapper,
                NiisWebContext context,
                INumberGenerator numberGenerator,
                IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> workflowApplier)
            {
                _mapper = mapper;
                _context = context;
                _numberGenerator = numberGenerator;
                _workflowApplier = workflowApplier;
            }

            public async Task<ProtectionDocDetailsDto> Handle(Command message)
            {
                var protectionDocDetailsDto = message.ProtectionDocDetailsDto;
                var protectionDoc = _mapper.Map<ProtectionDocDetailsDto, Domain.Entities.ProtectionDoc.ProtectionDoc>(protectionDocDetailsDto);

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    protectionDoc.StatusId =
                    _context.DicProtectionDocStatuses.SingleOrDefault(pds => pds.Code.Equals("D"))?.Id;

                    _numberGenerator.GenerateBarcode(protectionDoc);

                    _context.ProtectionDocs.Add(protectionDoc);

                    try
                    {
                        await _context.SaveChangesAsync();

                        var requestId = 0;

                        switch (protectionDoc.TypeId)
                        {
                            case 1:
                                requestId = 834;
                                break;
                            case 4:
                                requestId = 740;
                                break;
                            default:
                                requestId = 740;
                                break;
                        }

                        var request = _context.Requests.Include(r => r.ColorTzs)
                            .Include(r => r.EarlyRegs)
                            .Include(r => r.ICGSRequests)
                            .Include(r => r.ICISRequests)
                            .Include(r => r.IPCRequests)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                            .Include(r => r.RequestConventionInfos)
                            .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                            .Single(r => r.Id == requestId);
                        request.ProtectionDocId = protectionDoc.Id;
                        protectionDoc.RegNumber = request.RequestNum;
                        protectionDoc.RegDate = request.DateCreate;
                        protectionDoc.ValidDate = request.DateCreate.AddYears(10);

                        var icfems = request.Icfems.Select(i => new DicIcfemProtectionDocRelation
                        {
                            DicIcfemId = i.DicIcfemId,
                            ProtectionDocId = protectionDoc.Id
                        });
                        await _context.DicIcfemProtectionDocRelations.AddRangeAsync(icfems);
                        var colorTzs = request.ColorTzs.Select(c => new DicColorTZProtectionDocRelation
                        {
                            ColorTzId = c.ColorTzId,
                            ProtectionDocId = protectionDoc.Id
                        });
                        await _context.DicColorTzProtectionDocRelations.AddRangeAsync(colorTzs);
                        var icgs = _mapper.Map<ICGSRequest[], ICGSProtectionDoc[]>(request.ICGSRequests.ToArray());
                        foreach (var icgsProtectionDoc in icgs)
                        {
                            protectionDoc.IcgsProtectionDocs.Add(icgsProtectionDoc);
                        }
                        var ipcs = _mapper.Map<IPCRequest[], IPCProtectionDoc[]>(request.IPCRequests.ToArray());
                        foreach (var ipcProtectionDoc in ipcs)
                        {
                            protectionDoc.IpcProtectionDocs.Add(ipcProtectionDoc);
                        }
                        var icis = _mapper.Map<ICISRequest[], ICISProtectionDoc[]>(request.ICISRequests.ToArray());
                        foreach (var icisProtectionDoc in icis)
                        {
                            protectionDoc.IcisProtectionDocs.Add(icisProtectionDoc);
                        }
                        var earlyRegs = _mapper.Map<RequestEarlyReg[], ProtectionDocEarlyReg[]>(request.EarlyRegs.ToArray());
                        foreach (var protectionDocEarlyReg in earlyRegs)
                        {
                            protectionDoc.EarlyRegs.Add(protectionDocEarlyReg);
                        }
                        var conventionInfos = _mapper.Map<RequestConventionInfo[], ProtectionDocConventionInfo[]>(request.RequestConventionInfos.ToArray());
                        foreach (var protectionDocConventionInfo in conventionInfos)
                        {
                            protectionDoc.ProtectionDocConventionInfos.Add(protectionDocConventionInfo);
                        }
                        var info = _mapper.Map<RequestInfo, ProtectionDocInfo>(request.RequestInfo);
                        protectionDoc.ProtectionDocInfo = info;
                        var subjects = _mapper.Map<RequestCustomer[], ProtectionDocCustomer[]>(request.RequestCustomers.ToArray());
                        foreach (var protectionDocCustomer in subjects)
                        {
                            protectionDoc.ProtectionDocCustomers.Add(protectionDocCustomer);
                        }
                        var invoices = request.PaymentInvoices.Select(pi => new PaymentInvoice
                        {
                            TariffId = pi.TariffId,
                            Coefficient = pi.Coefficient,
                            Nds = pi.Nds,
                            ProtectionDocId = protectionDoc.Id,
                            PenaltyPercent = pi.PenaltyPercent,
                            TariffCount = pi.TariffCount,
                            IsComplete = pi.IsComplete,
                            OverdueDate = pi.OverdueDate,
                            DateFact = pi.DateFact,
                            DateComplete = pi.DateComplete,
                            ApplicantTypeId = pi.ApplicantTypeId,
                            CreateUserId = pi.CreateUserId,
                            WriteOffUserId = pi.WriteOffUserId,
                            WhoBoundUserId = pi.WhoBoundUserId,
                            PaymentUses = pi.PaymentUses,
                            StatusId = pi.StatusId
                        });
                        await _context.PaymentInvoices.AddRangeAsync(invoices);
                        await _context.SaveChangesAsync();

                        await _workflowApplier.ApplyInitialAsync(protectionDoc, message.UserId);
                        await _context.SaveChangesAsync();

                        var protectionDocWithIncludesWithIncludes = await _context.ProtectionDocs
                            .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                            .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                            .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                            .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                            .Include(r => r.Workflows).ThenInclude(r => r.Route)
                            .SingleAsync(r => r.Id == protectionDoc.Id);

                        transaction.Commit();
                        return _mapper.Map<Domain.Entities.ProtectionDoc.ProtectionDoc, ProtectionDocDetailsDto>(protectionDocWithIncludesWithIncludes);
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                    }
                }
            }
        }
    }
}
