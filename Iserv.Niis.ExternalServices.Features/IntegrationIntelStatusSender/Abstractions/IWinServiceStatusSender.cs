namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions
{
    public interface IWinServiceStatusSender
    {
        void Start();
        void Stop();
    }
}