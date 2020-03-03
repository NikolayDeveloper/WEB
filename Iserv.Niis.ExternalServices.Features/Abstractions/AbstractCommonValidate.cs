namespace Iserv.Niis.ExternalServices.Features.Abstractions
{
    public abstract class AbstractCommonValidate<T>
    {
        public abstract void Validate(T message);
    }
}
