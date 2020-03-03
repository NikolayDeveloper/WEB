using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions
{
    public interface IValidateGetCountTextForPaySumArgument
    {
        string GetValidationErrors(GetCountTextForPaySumArgument argument);
    }
}