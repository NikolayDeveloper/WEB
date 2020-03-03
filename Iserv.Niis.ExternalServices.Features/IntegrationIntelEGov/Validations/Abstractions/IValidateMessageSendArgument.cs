using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions
{
    public interface IValidateMessageSendArgument
    {
        string GetValidationErrors(MessageSendArgument argument);
    }
}