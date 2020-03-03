using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iserv.Niis.ExternalServices.Host.Mediator
{
    public interface IExceptionRequestHandler<TRequest, TResponse>
    {
        TResponse Handle(TRequest request, Exception ex);
    }
}