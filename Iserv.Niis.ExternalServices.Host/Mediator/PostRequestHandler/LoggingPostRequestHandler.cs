using Iserv.Niis.ExternalServices.Features.Abstractions;

namespace Iserv.Niis.ExternalServices.Host.Mediator.PostRequestHandler
{
    public class LoggingPostRequestHandler<TRequest, TResponse> : IPostRequestHandler<TRequest, TResponse>
    {
        private readonly AbstractPostLogging<TRequest> _postLogging;

        public LoggingPostRequestHandler(AbstractPostLogging<TRequest> postLogging)
        {
            _postLogging = postLogging;
        }
        public void Handle(TRequest request, TResponse response)
        {
            _postLogging.Logging(request);
        }
    }
}