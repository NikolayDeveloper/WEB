using Iserv.Niis.ExternalServices.Features.Abstractions;

namespace Iserv.Niis.ExternalServices.Host.Mediator.PreRequestHandler
{
    public class LoggingPreRequestHandler<TRequest> : IPreRequestHandler<TRequest>
    {
        private readonly AbstractPreLogging<TRequest> _preLogging;

        public LoggingPreRequestHandler(AbstractPreLogging<TRequest> preLogging)
        {
            _preLogging = preLogging;
        }

        public void Handle(TRequest message)
        {
            _preLogging.Logging(message);
        }
    }
}