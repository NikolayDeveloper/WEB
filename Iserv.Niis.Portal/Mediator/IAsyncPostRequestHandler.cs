using System.Threading.Tasks;

namespace Iserv.Niis.Portal.Mediator
{
    public interface IAsyncPostRequestHandler<TRequest, TResponse>
    {
        Task Handle(TRequest request, TResponse response);
    }
}