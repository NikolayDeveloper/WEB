using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Model.Models;
using MediatR;

namespace Iserv.Niis.Features.Materials.Compare
{
    public class DocumentsInfoForCompare
    {
        public class Query : IRequest<DocumentsCompareDto>
        {
            public int RequestId { get; }

            public Query(int requestId)
            {
                RequestId = requestId;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, DocumentsCompareDto>
        {
            private readonly IDocumentsCompare _documentsCompare;

            public QueryHandler(IDocumentsCompare documentsCompare)
            {
                _documentsCompare = documentsCompare;
            }

            Task<DocumentsCompareDto> IAsyncRequestHandler<Query, DocumentsCompareDto>.Handle(Query message)
            {
                return _documentsCompare.GetDocumentsInfoForCompare(message.RequestId);
            }
        }
    }
}