using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Materials;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.BibliographicData
{
    public class List
    {
        public class Query : IRequest<BibliographicDataDto>
        {
            public Query(int ownerId, Owner.Type ownerType)
            {
                OwnerId = ownerId;
                OwnerType = ownerType;
            }
            public int OwnerId { get; }
            public Owner.Type OwnerType { get; }
        }
        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, BibliographicDataDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BibliographicDataDto> Handle(Query message)
            {
                BibliographicDataDto result = null;
                switch (message.OwnerType)
                {
                    //TODO! Тянуть только то, что нужно
                    case Owner.Type.Request:
                        var request = await _context.Requests
                            .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                            .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                            .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                            .Include(r => r.ProtectionDocType)
                            .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                            .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                            .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                            .Include(r => r.Addressee)
                            .Include(r => r.ICGSRequests)
                            .Include(r => r.ICISRequests)
                            .Include(r => r.IPCRequests)
                            .Include(r => r.ColorTzs)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                            .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                            .Include(r => r.Icfems)
                            .Include(r => r.RequestType)
                            .Include(r => r.RequestConventionInfos)
                            .Include(r => r.Department)
                            .SingleOrDefaultAsync(r => r.Id == message.OwnerId);
                        result = _mapper.Map<Domain.Entities.Request.Request, BibliographicDataDto>(request);
                        break;
                    case Owner.Type.ProtectionDoc:
                        var protectionDoc = await _context.ProtectionDocs
                            .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                            .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                            .Include(r => r.Type)
                            .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
                            .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                            .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                            .Include(r => r.IcgsProtectionDocs)
                            .Include(r => r.IcisProtectionDocs)
                            .Include(r => r.IpcProtectionDocs)
                            .Include(r => r.ColorTzs)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                            .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                            .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                            .Include(r => r.Icfems)
                            .Include(r => r.SubType)
                            .SingleOrDefaultAsync(r => r.Id == message.OwnerId);
                        result = _mapper.Map<Domain.Entities.ProtectionDoc.ProtectionDoc, BibliographicDataDto>(protectionDoc);
                        break;
                    case Owner.Type.Contract:
                        throw new NotImplementedException();
                }

                return result;
            }
        }
    }
}
