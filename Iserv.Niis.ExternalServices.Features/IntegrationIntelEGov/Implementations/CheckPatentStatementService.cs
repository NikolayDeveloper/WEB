using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class CheckPatentStatementService : ICheckPatentStatementService
    {
        private readonly NiisWebContext _niisWebContext;

        public CheckPatentStatementService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public File GetStatementFile(
            string identifier, 
            string gosNumber)
        {
            return _niisWebContext.Statements
                .Where(s =>
                    s.Identifier == identifier
                    && s.GosNumber == gosNumber)
                .Select(s => new File
                {
                    Content = s.File,
                    Name = s.FileName
                }).FirstOrDefault();
        }

        public void GetCheckPatentResult(
            CheckPatentStatementResult result, 
            File statementFile)
        {
            if (statementFile == null)
            {
                result.IsSuccess = false;
                result.StatusRu = "Документ не найден";
                result.StatusKz = "Документ не найден";
                return;
            }
            result.IsSuccess = true;
            result.StatusRu = "OK";
            result.StatusKz = "OK";
            result.ResultFile = statementFile;
        }
    }
}