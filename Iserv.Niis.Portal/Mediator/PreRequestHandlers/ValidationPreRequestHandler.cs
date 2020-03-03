using System.Threading.Tasks;
using FluentValidation;

namespace Iserv.Niis.Portal.Mediator.PreRequestHandlers
{
    public class ValidationPreRequestHandler<TRequest> : IAsyncPreRequestHandler<TRequest>
    {
        private readonly AbstractValidator<TRequest> _validator;

        public ValidationPreRequestHandler(AbstractValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task Handle(TRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
        }
    }
}