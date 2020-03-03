using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class ConvertRequestHandler: BaseHandler
    {
        public async Task<ConvertResponseDto> ExecuteAsync(ConvertDto convertDto)
        {
            int requestId = convertDto.OwnerId;
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            var workflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>().Process(q => q.Execute(requestId));
            var result = new ConvertResponseDto();

            var fullExamCodes = new[]
            {
                RouteStageCodes.TZFirstFullExpertizePerformerChoosing,
                RouteStageCodes.TZFirstFullExpertize,
                RouteStageCodes.TZSecondFullExpertizePerformerChoosing,
                RouteStageCodes.TZSecondFullExpertize
            };

            #region Генерация уведомления

            string notificationCode;
            var hasBeenOnFullExam = workflows.Any(w => fullExamCodes.Contains(w.CurrentStage.Code));
            var trademarkTypeCode = request.SpeciesTradeMark.Code;

            //Смерджили два справочника, получилось так, не стал трогать, мало ли прийдется вернуть
            if (hasBeenOnFullExam)
            {
                if (trademarkTypeCode == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
                else
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
            }
            else
            {
                if (trademarkTypeCode == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
                else
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
            }
            var userInputDto = new UserInputDto
            {
                Code = notificationCode,
                Fields = new List<KeyValuePair<string, string>>(),
                OwnerId = requestId,
                OwnerType = Owner.Type.Request
            };
            Executor.GetHandler<WorkflowBusinessLogic.Document.CreateDocumentHandler>().Process(h =>
                h.Execute(requestId, Owner.Type.Request, notificationCode, DocumentType.Outgoing, userInputDto));


            #endregion

            #region Преобразование заявки

            var protectionDocTypeCode = request.ProtectionDocType.Code;
            request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));

            switch (protectionDocTypeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    if (!string.IsNullOrEmpty(convertDto.ColectiveTrademarkParticipantsInfo))
                    {
                        request.ColectiveTrademarkParticipantsInfo = convertDto.ColectiveTrademarkParticipantsInfo;
                    }
                    DicProtectionDocSubType newProtectionDocSubType = null;
                    if (request.SpeciesTradeMark.Code == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                    {
                        newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                            .Process(q => q.Execute(DicProtectionDocSubtypeCodes.RegularTradeMark));
                    }
                    else
                    {
                        newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                            .Process(q => q.Execute(DicProtectionDocSubtypeCodes.CollectiveTrademark));
                    }

                    request.SpeciesTradeMark = newProtectionDocSubType;
                    request.SpeciesTradeMarkId = newProtectionDocSubType.Id;

                    Executor.GetCommand<WorkflowBusinessLogic.Requests.UpdateRequestCommand>().Process(c => c.Execute(request));

                    result.SpeciesTradeMarkId = request.SpeciesTradeMarkId;
                    result.ColectiveTrademarkParticipantsInfo = request.ColectiveTrademarkParticipantsInfo;
                    result.SpeciesTrademarkCode = request.SpeciesTradeMark?.Code;
                    break;
            }

            #endregion

            return result;
        }
    }
}
