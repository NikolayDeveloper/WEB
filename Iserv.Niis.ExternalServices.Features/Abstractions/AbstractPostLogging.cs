namespace Iserv.Niis.ExternalServices.Features.Abstractions
{
    public abstract class AbstractPostLogging<T>
    {
        public abstract void Logging(T message);
    }
}
