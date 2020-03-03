using System.Threading.Tasks;
using MediatR;

namespace Iserv.Niis.Portal.Mediator
{
    public class AsyncMediatorPipeline<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;

        private readonly IAsyncPreRequestHandler<TRequest>[] _preRequestHandlers;

        private readonly IAsyncPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;

        public AsyncMediatorPipeline(IAsyncRequestHandler<TRequest, TResponse> inner, IAsyncPreRequestHandler<TRequest>[] preRequestHandlers, IAsyncPostRequestHandler<TRequest, TResponse>[] postRequestHandlers)
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                await preRequestHandler.Handle(message);
            }

            var result = await _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                await postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}