using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using MediatR;
using AutoMapper;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    public class GetAllPatentAttorneys
    {
        public class Query : IRequest<List<PatentAttorney>>
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
        public class QueryHandler : IRequestHandler<Query, List<PatentAttorney>>
        {
            private readonly NiisWebContext _niisContext;
            public QueryHandler(NiisWebContext niisContext)
            {
                _niisContext = niisContext;
            }
            public List<PatentAttorney> Handle(Query message)
            {
                var patentAttornies = _niisContext.CustomerAttorneyInfos.ToList();
                var resut = Mapper.Map<List<PatentAttorney>>(patentAttornies);
                return resut;
            }
        }

        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }
        public class QueryException : AbstractionExceptionHandler<Query, List<PatentAttorney>>
        {
            public override List<PatentAttorney> GetExceptionResult(Query message, Exception ex)
            {
                return null;
            }
        }
    }
}
