using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class GetValidityExpiredProtectionDocsQuery: BaseQuery
    {
        public List<ProtectionDoc> Execute(int months = 0)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            var result = repo.AsQueryable()
                .Include(pd => pd.Type)
                .Where(pd => pd.ValidDate.HasValue &&
                             pd.ValidDate.Value.AddYears(GetProlongationYears(pd)).AddMonths(months) < NiisAmbientContext.Current.DateTimeProvider.Now &&
                             new[] {RouteStageCodes.OD05, RouteStageCodes.OD05_01}.Contains(pd.CurrentWorkflow
                                 .CurrentStage
                                 .Code));

            return result.ToList();
        }

        private int GetProlongationYears(ProtectionDoc protectionDoc)
        {
            switch (protectionDoc.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    return 10;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    return 5;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    return 3;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    return 10;
                default:
                    return 0;
            }
        }
    }
}
