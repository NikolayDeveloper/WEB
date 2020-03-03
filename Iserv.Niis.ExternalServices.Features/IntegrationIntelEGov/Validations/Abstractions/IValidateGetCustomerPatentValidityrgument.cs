using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions
{
    public interface IValidateGetCustomerPatentValidityArgument
    {
        string GetValidationErrors(CustomerPatentValidityRequest request);
    }
}