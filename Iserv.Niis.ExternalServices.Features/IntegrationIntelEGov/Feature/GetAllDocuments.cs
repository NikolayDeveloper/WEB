using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using MediatR;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    /// <summary>
    /// Получение типов документов
    /// </summary>
    public class GetAllDocuments
    {
        public class Query : IRequest<List<DocumentInfo>>
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
        public class QueryHandler : IRequestHandler<Query, List<DocumentInfo>>
        {
            private readonly NiisWebContext _niisContext;
            public QueryHandler(NiisWebContext niisContext)
            {
                _niisContext = niisContext;
            }
            public List<DocumentInfo> Handle(Query message)
            {
                var colors = _niisContext.DicDocumentTypes.ToList();
                return Mapper.Map<List<DocumentInfo>>(colors);
            }
        }
        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }
        public class QueryException : AbstractionExceptionHandler<Query, List<DocumentInfo>>
        {
            public override List<DocumentInfo> GetExceptionResult(Query message, Exception ex)
            {
                return null;
            }
        }
    }
}
