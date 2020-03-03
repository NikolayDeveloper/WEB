using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.ExternalServices.Host.Mediator
{
    public interface IPreRequestHandler<TRequest>
    {
        void Handle(TRequest request);
    }
}
