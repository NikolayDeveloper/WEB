using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IRequisitionSendService
    {
        void RequisitionBlockCustomerAdd(Customer[] customers, int requestId);
        void RequisitionBlockFileAdd(AttachedFile[] blockFile, int requestId, string sender);
        void RequisitionColorAdd(RefKey[] blockColor, int requestId, int protectionDocTypeId);

        (int requestId, int onlineStatusId, string incomingNum, int barcode) RequisitionDocumentAdd(
            RequisitionSendArgument argument);

        void RequisitionEarlyRegAdd(EarlyReg[] blockEarlyReg, int requestId);
        void RequisitionMktuAdd(RefKey[] blockClassification, int requestId);
    }
}