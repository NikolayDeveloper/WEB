namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions
{
    public interface ISession
    {
        void PerformSendDocument();
        void PerformSendStatuses();
    }
}