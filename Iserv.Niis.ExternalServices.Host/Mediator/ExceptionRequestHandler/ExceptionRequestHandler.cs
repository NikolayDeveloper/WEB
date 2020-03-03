using System;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov;

namespace Iserv.Niis.ExternalServices.Host.Mediator.ExceptionRequestHandler
{
    public class ExceptionRequestHandler<TRequest, TResponse> : IExceptionRequestHandler<TRequest, TResponse>
    {
        private readonly AbstractionExceptionHandler<TRequest, TResponse> _exceptionHandler;

        public ExceptionRequestHandler(AbstractionExceptionHandler<TRequest, TResponse> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }
        public TResponse Handle(TRequest request, Exception ex)
        {
            return _exceptionHandler.GetExceptionResult(request, ex);
        }
    }
}