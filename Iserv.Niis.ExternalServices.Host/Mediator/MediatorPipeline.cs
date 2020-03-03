using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;

namespace Iserv.Niis.ExternalServices.Host.Mediator
{
    public class MediatorPipeline<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;
        private readonly IExceptionRequestHandler<TRequest, TResponse> _exceptionRequestHandler;

        public MediatorPipeline(IRequestHandler<TRequest, TResponse> inner, IPreRequestHandler<TRequest>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            IExceptionRequestHandler<TRequest, TResponse> exceptionRequestHandler)
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            _exceptionRequestHandler = exceptionRequestHandler;
        }

        public TResponse Handle(TRequest message)
        {
            try
            {
                foreach (var preRequestHandler in _preRequestHandlers)
                {
                    preRequestHandler.Handle(message);
                }
                var result = _inner.Handle(message);
                foreach (var postRequestHandler in _postRequestHandlers)
                {
                    postRequestHandler.Handle(message, result);
                }
                return result;
            }
            catch (Exception ex)
            {
                return _exceptionRequestHandler.Handle(message, ex);
            }
        }
    }
}