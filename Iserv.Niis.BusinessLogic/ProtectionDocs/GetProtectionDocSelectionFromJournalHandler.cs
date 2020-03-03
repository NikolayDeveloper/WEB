using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocSelectionFromJournalHandler: BaseHandler
    {
        public int[] Execute(bool isAllSelected, bool hasIpc, SelectionMode selectionMode, int[] protectionDocIds)
        {
            if (isAllSelected)
            {
                var protectionDocs = Executor.GetQuery<GetProtectionDocsByUserIdQuery>()
                    .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
                protectionDocIds = protectionDocs
                    .Where(pd =>
                        string.IsNullOrEmpty(pd.GosNumber) &&
                        pd.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_1 && (hasIpc
                            ? new[]
                            {
                                DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                                DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                                DicProtectionDocTypeCodes.RequestTypeInventionCode,
                                DicProtectionDocTypeCodes.RequestTypeUsefulModelCode
                            }.Contains(pd.Type.Code)
                            : !new[]
                            {
                                DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                                DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                                DicProtectionDocTypeCodes.RequestTypeInventionCode,
                                DicProtectionDocTypeCodes.RequestTypeUsefulModelCode
                            }.Contains(pd.Type.Code))).Select(pd => pd.Id).ToArray();
            }
            else
            {
                if (selectionMode == SelectionMode.Except)
                {
                    var protectionDocs = Executor.GetQuery<GetProtectionDocsByUserIdQuery>()
                        .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
                    protectionDocIds = protectionDocs
                        .Where(pd =>
                            string.IsNullOrEmpty(pd.GosNumber) &&
                            pd.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_1 && (hasIpc
                                ? new[]
                                {
                                    DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                                    DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
                                }.Contains(pd.Type.Code)
                                : !new[]
                                {
                                    DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                                    DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
                                }.Contains(pd.Type.Code)) && !protectionDocIds.Contains(pd.Id)).Select(pd => pd.Id).ToArray();
                }
            }
            return protectionDocIds;
        }
    }
}
