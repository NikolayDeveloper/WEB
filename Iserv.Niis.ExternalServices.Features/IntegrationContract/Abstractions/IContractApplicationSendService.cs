using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Abstractions
{
    public interface IContractApplicationSendService
    {
        ContractResponse AddContract(ContractRequest request, ContractResponse response);
    }
}
