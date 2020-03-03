using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;

namespace Iserv.Niis.ExternalServices.Host.Mediator.PreRequestHandler
{
    public class ValidationPreRequestHandler<TRequest> : IPreRequestHandler<TRequest>
    {
        private readonly AbstractCommonValidate<TRequest> _abstractCommonValidate;

        public ValidationPreRequestHandler(AbstractCommonValidate<TRequest> abstractCommonValidate)
        {
            _abstractCommonValidate = abstractCommonValidate;
        }
        public void Handle(TRequest request)
        {
            _abstractCommonValidate.Validate(request);
        }
    }
}