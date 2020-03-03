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
    public class GetAllColors
    {
        public class Query : IRequest<List<Color>>
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
        public class QueryHandler : IRequestHandler<Query, List<Color>>
        {
            private readonly NiisWebContext _niisContext;
            public QueryHandler(NiisWebContext niisContext)
            {
                _niisContext = niisContext;
            }
            public List<Color> Handle(Query message)
            {
                var colors = _niisContext.DicColorTZs.ToList();
                return Mapper.Map<List<Color>>(colors);
            }
        }
        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }
        public class QueryException : AbstractionExceptionHandler<Query, List<Color>>
        {
            public override List<Color> GetExceptionResult(Query message, Exception ex)
            {
                return null;
            }
        }
    }
}
