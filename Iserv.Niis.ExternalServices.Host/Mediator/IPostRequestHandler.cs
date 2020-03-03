using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iserv.Niis.ExternalServices.Host.Mediator
{
    public interface IPostRequestHandler<TRequest, TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}