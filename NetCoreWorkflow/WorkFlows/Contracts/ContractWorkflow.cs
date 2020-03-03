using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.ContractDocuments;
using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicContractStatuse;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.RulesContract;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace NetCoreWorkflow.WorkFlows.Contracts
{
    /// <summary>
    ///     Рабочий процесс по договору
    /// </summary>
    public class ContractWorkflow : BaseContractWorkflow
    {
        public ContractWorkflow()
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "73E7493E-E942-4377-B622-675F71CF8BB3")
                .UseForAllStages()
                .And<IsContractExistsNextRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendContractDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "126BE89D-985B-47A9-8CEE-8924E21BE6E6")
                .UseForAllStages()
                .And<IsContractExistsPreviousRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendContractDocumentToNextHandStage());



            WorkflowStage(
                    "Из этапа 'Зачтение оплаты за регистрацию договора' на этап 'Регистрация договора'",
                    "A43B4F00-0C3D-49BD-8696-3720C7A90894")
                .WhenCurrentStageCode(RouteStageCodes.DK02_1_1)
                   .And<IsContractHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes._1030,
                    DicTariff.Codes._34,
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK02_2));


            WorkflowStage(
                    "Из этапа 'Регистрация договора' на этап 'Подготовка для публикации'",
                    "45E3633B-0289-40B6-ABBE-F149B80F6B6C")
                .WhenCurrentStageCode(RouteStageCodes.DK02_2)
                .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.DK_UVED_POL_TZ
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK03_01));


            WorkflowStage("Из этапа 'Регистрация договора' на этап 'Отказано в регистрации'",
                    "9B378FED-9551-46EA-9D11-54FDA20487BA")
                .WhenCurrentStageCode(RouteStageCodes.DK02_2)
                .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.NoticeRefusalRegisterContract
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK02_9_3));


            WorkflowStage("Из этапа 'Регистрация договора' на этап 'Ожидается ответ на запрос'",
                    "D672CEA8-6E7B-4BBB-B749-70EE69BC2B4F")
                .WhenCurrentStageCode(RouteStageCodes.DK02_2)
                .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.DK_ZAPROS
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK02_4_2));


            WorkflowStage("Из этапа 'Ожидается ответ на запрос' на этап 'Регистрация договора'",
                    "693F8642-C4D8-4B94-8601-D50D04CFF64F")
                .WhenCurrentStageCode(RouteStageCodes.DK02_4_2)
                .And<IsContractHasAnyDocumentWithCodesRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.AnswerToRequest
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK02_2));


            WorkflowStage("Из этапа 'Подготовка для публикации' на этап 'Внесение данных в Госреестр'",
                    "DD8A1A78-7F68-477A-94FE-75BBB45AE768")
                .WhenCurrentStageCode(RouteStageCodes.DK03_01)
                .And<IsContractDocumentHasStageByStageCodeAndDocumentCodeRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.RegisterTransferInformationContracts,
                }, RouteStageCodes.DocumentInternalIN01_1_1))
                .Then(SendContractToNextStage(RouteStageCodes.DK03_2));


            WorkflowStage("Из этапа 'Внесение данных в Госреестр' на этап 'Зарегистрировано'",
                    "DDF8199D-5212-4B7F-A9A5-94C803C45A6D")
                .WhenCurrentStageCode(RouteStageCodes.DK03_2)
                .And<IsContractHasExpiredBulletinDate>(c => c.Eval())
                .Then(SendContractToNextStage(RouteStageCodes.DK02_9_1));


            WorkflowStage("Из этапа 'Зарегистрировано' на этап 'Аннулирование'",
                    "9F53572B-AA97-44B7-ADF1-4EAE90432827")
                .WhenCurrentStageCode(RouteStageCodes.DK02_9_1)
                .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.CancellationNotice
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK05_2));


            WorkflowStage("Из этапа 'Зарегистрировано' на этап 'Расторжение'",
                    "66A6537E-9624-40B5-B72E-602FD17800C7")
                .WhenCurrentStageCode(RouteStageCodes.DK02_9_1)
                 .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
                {
                    DicDocumentTypeCodes.TerminationNotice
                }))
                .Then(SendContractToNextStage(RouteStageCodes.DK05_3));



            WorkflowStage("Из любого этапа на этап 'Делопроизводство прекращено по просьбе заявителя'",
                    "D1999930-D9B6-4F08-83EE-964C1CA6870B")
              .UseForAllStages()
              .And<IsAnyDocumentHasOutgoingNumberByCodesInContractRule>(c => c.Eval(new[]
              {
                  DicDocumentTypeCodes.OUT_UV_Pred_Prekr_del_bez_opl_v1_19
              }))
              .Then(SendToNextStageAndChangeStatus(RouteStageCodes.DK02_7_2, DicContractStatuseCode._03_30));
        }

        private Action SendToNextStageAndChangeStatus(string stageCode, string statusCode)
        {
            return () =>
            {
                SendContractToNextStage(stageCode)?.Invoke();

                var contract = Executor.GetQuery<GetContractByIdQuery>()
                    .Process(q => q.Execute(WorkflowRequest.ContractId));
                var statusId = Executor.GetQuery<GetDicContractStatusIdByCodeQuery>()
                    .Process(q => q.Execute(statusCode));
                if (statusId != default(int))
                {
                    contract.StatusId = statusId;
                    Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
                }
            };
        }
        private Action SendToNextStageAndSetRegDate(string stageCode)
        {
            return () =>
            {
                SendContractToNextStage(stageCode)?.Invoke();
                var documentSendContract = Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                        .Process(q => q.Execute(WorkflowRequest.ContractId, new[] { DicDocumentTypeCodes.CoveringLetterOfDk }))
                        .FirstOrDefault();
                if (documentSendContract == null)
                {
                    return;
                }
                var docUserInput = Executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>()
                        .Process(q => q.Execute(documentSendContract.Id));
                if (docUserInput == null)
                {
                    return;
                }

                var userInput = JsonConvert.DeserializeObject<UserInputDto>(docUserInput.UserInput);
                var valuePairGosDate = userInput.Fields.Where(f => f.Key == "ContractDateReg_UserInput").FirstOrDefault();

                var isSuccessParseGosDate = DateTimeOffset.TryParse(valuePairGosDate.Value, out DateTimeOffset gosDate);
                if (isSuccessParseGosDate)
                {
                    var contract = Executor.GetQuery<GetContractByIdQuery>()
                        .Process(q => q.Execute(WorkflowRequest.ContractId));
                    contract.GosDate = gosDate;
                    Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
                }
            };
        }
    }
}