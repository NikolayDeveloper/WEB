using System;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetPayTariffService
    {
        bool GetDiscontinued(int? protectionDocState, string protectionDocType, DateTime? flNotValidity, DateTime? flValidityExtention);
        ProtectionDoc GetProtectionDoc(string protectionDocType, string protectionDocNumber, GetPayTarifResult result);
        void GetTariffId(string protectionDocType, int? protectionDocId, GetPayTarifResult result);
        DateTime? GetValidity(string protectionDocType, DateTime? flValidityExtention);
    }
}