using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using AutoMapper;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.DataAccess.EntityFramework;


namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    public class GetAllTariffs
    {
        public class Query : IRequest<List<Tariff>>
        {

        }
        public class QueryPreLogging : AbstractPreLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }
        public class QueryValidator : AbstractCommonValidate<Query>
        {
            public override void Validate(Query message)
            {
            }
        }
        public class QueryHandler : IRequestHandler<Query, List<Tariff>>
        {
            private readonly NiisWebContext _niisContext;
            public QueryHandler(NiisWebContext niisContext)
            {
                _niisContext = niisContext;
            }
            public List<Tariff> Handle(Query message)
            {
                var tariffs = _niisContext.DicTariffs.ToList();
                return Mapper.Map<List<Tariff>>(tariffs);
            }
        }
        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }
        public class QueryException : AbstractionExceptionHandler<Query, List<Tariff>>
        {
            public override List<Tariff> GetExceptionResult(Query message, Exception ex)
            {
                return null;
            }
        }
    }
}
