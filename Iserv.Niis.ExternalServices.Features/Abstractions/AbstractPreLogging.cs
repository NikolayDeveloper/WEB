namespace Iserv.Niis.ExternalServices.Features.Abstractions
{
    public abstract class AbstractPreLogging<T>
    {
        public abstract void Logging(T message);
    }
}
