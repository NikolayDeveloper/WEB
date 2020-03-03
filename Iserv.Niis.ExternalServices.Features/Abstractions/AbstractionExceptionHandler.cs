using System;

namespace Iserv.Niis.ExternalServices.Features.Abstractions
{
    public abstract class AbstractionExceptionHandler<TRequest, TResponse>
    {
        public abstract TResponse GetExceptionResult(TRequest message, Exception ex);
    }
}
