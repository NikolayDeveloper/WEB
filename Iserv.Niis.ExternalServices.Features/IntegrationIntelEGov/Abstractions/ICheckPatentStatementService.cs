using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface ICheckPatentStatementService
    {
        File GetStatementFile(string identifier, string gosNumber);
        void GetCheckPatentResult(CheckPatentStatementResult result, File statementFile);
    }
}