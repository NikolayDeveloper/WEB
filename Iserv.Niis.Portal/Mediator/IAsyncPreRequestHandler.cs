using System.Threading.Tasks;

namespace Iserv.Niis.Portal.Mediator
{
    public interface IAsyncPreRequestHandler<TRequest>
    {
        Task Handle(TRequest request);
    }
}