using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class FilterNextStagesByRequestIdHandler : BaseHandler
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int requestId)
        {
            //var mistakeStageCodes = new[]
            //{
            //    /*
            //    RouteStageCodes.SA_02_3,
            //    */
            //    RouteStageCodes.PO_02_3,
            //    RouteStageCodes.PO_02_2,
            //    RouteStageCodes.I_04_0_0_1,
            //    RouteStageCodes.TZ_02_2_1,
            //    RouteStageCodes.UM_03_2_5                
            //};
            //var fomationStageCodes = new[]
            //{
            //    RouteStageCodes.TZ_02_1,
            //    RouteStageCodes.ITZ_02_1,                
            //    RouteStageCodes.I_02_1,
            //    //RouteStageCodes.UM_02_1,
            //    RouteStageCodes.PO_02_1,
            //    /*
            //    RouteStageCodes.SA_02_1
            //    */
            //};
            //var paymentStageCodes = new[]
            //{
            //    RouteStageCodes.TZ_02_2,
            //    RouteStageCodes.TZ_02_2_2,
            //    RouteStageCodes.TZ_03_2_2_0,
                
            //    RouteStageCodes.I_02_2,
            //    RouteStageCodes.I_02_2_0_0,
            //    RouteStageCodes.UM_02_2,
            //    RouteStageCodes.PO_02_2,
            //    /*
            //    RouteStageCodes.SA_02_2
            //    */
            //};
            //var formalExamReadyStageCodes = new[]
            //{
            //    RouteStageCodes.TZ_03_2_2_1,
            //    RouteStageCodes.I_03_2_1_1,
            //    RouteStageCodes.PO_03_2_01
            //};
            //var prepareToSendToGosreestrStageCodes = new[]
            //{
            //    //RouteStageCodes.TZ_03_3_7,
            //    //RouteStageCodes.NMPT_03_7,
            //    RouteStageCodes.I_03_3_7_1,
            //    RouteStageCodes.PO_03_8,
            //    RouteStageCodes.UM_03_7_1,
            //};
            //var nmptPreGrStageCodes = new[]
            //{
            //    RouteStageCodes.NMPT_03_6
            //};
            //var expertiseStageCodes = new[]
            //{
            //    RouteStageCodes.NMPT_03_1,
            //    RouteStageCodes.NMPT_03_2
            //};
            //var changeStageCodes = new[]
            //{
            //    RouteStageCodes.TZ_06
            //};
            //var abortStageCodes = new[]
            //{
            //    RouteStageCodes.TZ_03_3_9
            //};

            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            if (request == null)
            {
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, requestId);
            }
            var nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                .Process(q => q.ExecuteAsync(request.CurrentWorkflow.RouteId ?? default(int)));

            //var nextStages = await Executor.GetQuery<GetNextStagesByCurrentStageIdQuery>()
            //    .Process(q => q.ExecuteAsync(request.CurrentWorkflow.CurrentStageId ?? default(int)));
            //var mistakeStages = await Executor.GetQuery<GetRouteStagesByCodesQuery>()
            //    .Process(q => q.ExecuteAsync(mistakeStageCodes));
            //var changeStages = await Executor.GetQuery<GetRouteStagesByCodesQuery>()
            //    .Process(q => q.ExecuteAsync(changeStageCodes));

            var result = nextStages.ToList();

            return result;

   //         if (paymentStageCodes.Contains(request.CurrentWorkflow.CurrentStage.Code)
   //             || formalExamReadyStageCodes.Contains(request.CurrentWorkflow.CurrentStage.Code)
   //             || fomationStageCodes.Contains(request.CurrentWorkflow.CurrentStage.Code)
   //             || prepareToSendToGosreestrStageCodes.Contains(request.CurrentWorkflow.CurrentStage.Code))
   //         {
   //             var customerExists = request.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
   //             if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeInventionCode && customerExists)
   //             {
   //                 customerExists = request.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Author);
   //             }
   //             var baseTypesCheck = customerExists && !string.IsNullOrEmpty(request.RequestNum);
   //             var isAllDataEntered = true;

   //             switch (request.ProtectionDocType.Code)
   //             {
   //                 case DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode:
   //                 case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
   //                     isAllDataEntered = request.Image != null
   //                                        && request.ICGSRequests.Count > 0
   //                                        && request.SpeciesTradeMarkId.HasValue;
   //                     if (request?.SpeciesTradeMark?.Code == DicProtectionDocSubtypeCodes.CollectiveTrademark)
   //                     {
   //                         isAllDataEntered = isAllDataEntered &&
   //                                            !string.IsNullOrEmpty(request.ColectiveTrademarkParticipantsInfo) &&
   //                                            request.Documents.Any(rd =>
   //                                                rd.Document.Type.Code == DicDocumentTypeCodes._001_095_1);
   //                     }
   //                     if (new[]
   //                     {
   //                         DicTypeTrademark.Codes.Combined, DicTypeTrademark.Codes.Verbal,
   //                         DicTypeTrademark.Codes.Literal
   //                     }.Contains(request?.TypeTrademark?.Code))
   //                     {
   //                         isAllDataEntered = isAllDataEntered && !string.IsNullOrEmpty(request.Transliteration);
   //                     }
   //                     break;
   //                 case DicProtectionDocTypeCodes.RequestTypeInventionCode:
   //                 case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
   //                     isAllDataEntered = request.RequestCustomers.Any(rc =>
   //                                            rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
   //                                        && request.RequestCustomers.Any(rc =>
   //                                            rc.CustomerRole.Code == DicCustomerRoleCodes.Author)
   //                                        && (!string.IsNullOrEmpty(request.NameRu) ||
   //                                            !string.IsNullOrEmpty(request.NameKz) ||
   //                                            !string.IsNullOrEmpty(request.NameEn))
   //                                        && request.RequestDate != null
   //                                        && !string.IsNullOrEmpty(request.Referat)
   //                                        && UsefulModelRequestHasBiblioData(request);
   //                     break;
   //                 case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
   //                     isAllDataEntered = request.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
   //                                        && (!string.IsNullOrEmpty(request.NameRu) || !string.IsNullOrEmpty(request.NameKz) || !string.IsNullOrEmpty(request.NameEn))
   //                                        && request.RequestInfo != null
   //                                        && !string.IsNullOrEmpty(request.SelectionFamily)
   //                                        && !string.IsNullOrEmpty(request.RequestInfo.ProductSpecialProp)
   //                                        && !string.IsNullOrEmpty(request.RequestInfo.ProductPlace);
   //                     break;
   //             }

   //             if (!isAllDataEntered || !baseTypesCheck)
   //             {
   //                 result.Clear();
   //                 result = result.Concat(mistakeStages.Where(s => s.RouteId == request.ProtectionDocType.RouteId))
   //                     .ToList();
   //             }
   //         }
   //         if (nmptPreGrStageCodes.Contains(request.CurrentWorkflow.CurrentStage.Code))
   //         {
   //             if (request.Documents.Any(d => d.Document.Type.Code == DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion))
   //             {
   //                 result.Clear();
   //             }
   //             else
   //             {
   //                 if (request.Documents.Any(d => d.Document.Type.Code == DicDocumentTypeCodes.NotificationOfRegistrationDecision))
   //                 {
   //                     result = result.Where(r => !expertiseStageCodes.Contains(r.Code)).ToList();
   //                 }
   //                 else
   //                 {
   //                     result = result.Where(r => expertiseStageCodes.Contains(r.Code)).ToList();
   //                 }
   //             }

   //         }
           
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_03_3_5)
   //         {
   //             var isExistConclusionOfInventionPatentGrantRefuseInRequest = request.Documents.Any(d =>
   //                d.Document.Type.Code == DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse);
   //             if (isExistConclusionOfInventionPatentGrantRefuseInRequest == false)
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.I_03_3_4_1).ToList();
   //             }
   //         }
   //         //if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_4_0)
   //         //{
   //         //    if (request.Workflows.Any(rw => rw.CurrentStage.Code == RouteStageCodes.UM_03_9))
   //         //    {
   //         //        result.Clear();
   //         //    }
   //         //}
   //         //if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_7_0)
   //         //{
   //         //    var executionDate = request.RequestDate?.AddMonths(12) ?? DateTimeOffset.Now;
   //         //    var hasDocs = request.Documents.Any(rd =>
   //         //        rd.Document.Type.Code ==
   //         //        DicDocumentTypeCodes._001_002 ||
   //         //        rd.Document.Type.Code ==
   //         //        DicDocumentTypeCodes._001_002_1 ||
   //         //        rd.Document.Type.Code ==
   //         //        DicDocumentTypeCodes.IN001_032/*||*/
   //         //                                      //rd.Document.Type.Code == DicDocumentTypeCodes._001_032
   //         //        );
   //         //    var hasPayments = request.PaymentInvoices.Any(pi =>
   //         //                            pi.Tariff.Code == DicTariffCodes.BS2URegistrationAndPublishing && (
   //         //                            pi.Status.Code == DicPaymentStatusCodes.Credited || 
   //         //                            pi.Status.Code == DicPaymentStatusCodes.Charged));
   //         //                      //    pi.Tariff.Code == DicTariff.Codes.StateFee2018 &&
   //         //                      //    pi.Status.Code == DicPaymentStatusCodes.Credited)
   //         //                      //&& request.PaymentInvoices.Any(pi =>
   //         //                      //    pi.Tariff.Code == DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent &&
   //         //                      //    pi.Status.Code == DicPaymentStatusCodes.Credited);
   //         //    if (hasDocs && hasPayments)
   //         //    {
   //         //        if (DateTimeOffset.Now < executionDate)
   //         //        {
   //         //            if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.ApplicationForEarlyPublication))
   //         //            {
   //         //                result = result.Where(r => r.Code != RouteStageCodes.UM_03_3_7).ToList();
   //         //            }
   //         //            else
   //         //            {
   //         //                result = result.Where(r => r.Code != RouteStageCodes.UM_03_8).ToList();
   //         //            }
   //         //        }
   //         //        else
   //         //        {
   //         //            result = result.Where(r => r.Code != RouteStageCodes.UM_03_8).ToList();
   //         //        }
   //         //    }
   //         //    else
   //         //    {
   //         //        result.Clear();
   //         //    }
   //         //}
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_7_3)
   //         {
   //             var executionDate = request.RequestDate?.AddMonths(12) ?? DateTimeOffset.Now;
   //             var hasDocs = request.Documents.Any(rd =>
   //                               rd.Document.Type.Code ==
   //                               DicDocumentTypeCodes._001_002 ||
   //                               rd.Document.Type.Code ==
   //                               DicDocumentTypeCodes._001_002_1 ||
   //                               rd.Document.Type.Code ==
   //                               DicDocumentTypeCodes.IN001_032)
   //                           && request.Documents.Any(rd =>
   //                               rd.Document.Type.Code == DicDocumentTypeCodes
   //                                   .PetitionForRestoreTime);
   //             var hasPayments = request.PaymentInvoices.Any(pi =>
   //                                   pi.Tariff.Code == DicTariff.Codes.StateFee2018 &&
   //                                   pi.Status.Code == DicPaymentStatusCodes.Credited)
   //                               && request.PaymentInvoices.Any(pi =>
   //                                   pi.Tariff.Code == DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent &&
   //                                   pi.Status.Code == DicPaymentStatusCodes.Credited)
   //                               && request.PaymentInvoices.Any(pi =>
   //                                   pi.Tariff.Code == DicTariff.Codes.NEW_058 &&
   //                                   pi.Status.Code == DicPaymentStatusCodes.Credited);
   //             if (hasDocs && hasPayments)
   //             {
   //                 if (DateTimeOffset.Now < executionDate)
   //                 {
   //                     if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.ApplicationForEarlyPublication))
   //                     {
   //                         result = result.Where(r => r.Code != RouteStageCodes.UM_03_3_7).ToList();
   //                     }
   //                     else
   //                     {
   //                         result = result.Where(r => r.Code != RouteStageCodes.UM_03_8).ToList();
   //                     }
   //                 }
   //                 else
   //                 {
   //                     result = result.Where(r => r.Code != RouteStageCodes.UM_03_8).ToList();
   //                 }
   //             }
   //             else
   //             {
   //                 result.Clear();
   //             }
   //         }
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_02_2_1)
   //         {
   //             var isConvention = request.ConventionType?.Code == DicConventionTypeCodes.Conventional 
   //                                 && request.Documents
   //                                     .All(rd => rd.Document.Type.Code != DicDocumentTypeCodes._001_072_1);
   //             if (isConvention)
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.UM_02_2_2_2).ToList();
   //             }
   //             else
   //             {
   //                 if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.EssayForeign))
   //                 {
   //                     result = result.Where(r => r.Code == RouteStageCodes.UM_02_2_4).ToList();
   //                 }
   //                 else
   //                 {
   //                     result = result.Where(r => r.Code == RouteStageCodes.UM_03_1).ToList();
   //                 }
   //             }
   //         }
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_02_2_2_2)
   //         {
   //             var hasDocuments = request.Documents.Any(rd =>
   //                 rd.Document.Type.Code == DicDocumentTypeCodes._001_005);
   //             if (hasDocuments)
   //             {
   //                 if (request.Documents.Any(rd =>
   //                     rd.Document.Type.Code == DicDocumentTypeCodes.EssayForeign))
   //                 {
   //                     result = result.Where(r => r.Code == RouteStageCodes.UM_02_2_4).ToList();
   //                 }
   //                 else
   //                 {
   //                     result = result.Where(r => r.Code == RouteStageCodes.UM_03_1).ToList();
   //                 }
   //             }
   //             else
   //             {
   //                 result.Clear();
   //             }
   //         }
   //         //if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_9)
   //         //{
   //         //    if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.DecisionOfAppealsBoard))
   //         //    {
   //         //        result.Clear();
   //         //    }
   //         //}
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_02_1)
   //         {
   //             if (request.RequestCustomers.Any(rc => rc.Customer.IsNotResident == false && rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant))
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.I_02_2).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.I_02_3).ToList();
   //             }
   //         }
   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_02_1)
   //         {
   //             if (request.RequestCustomers.Any(rc => rc.Customer.IsNotResident == false && rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant))
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.UM_02_2).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.UM_02_1_0).ToList();
   //             }
   //         }
			//#region NMPT
			//if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.NMPT_01_1)
   //         {
   //             if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementNameOfOrigin))
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.NMPT_02_1).ToList();
   //             }
   //             else
   //             {
   //                 result.Clear();
   //             }
   //         }
			///* -- TODO do checking later
			//if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.NMPT_03_3_6)
			//{
			//	if (request.Documents.Any(d => d.Document.Type.Code == DicDocumentTypeCodes.NotificationOfRegistrationDecision || d.Document.Type.Code == DicDocumentTypeCodes.OUT_UV_GR_Reg_TZ_v1_19))
			//	{
			//		result = result.Where(r => r.Code != RouteStageCodes.NMPT_03_3_4).ToList();
			//	}
			//	else
			//	{
			//		result = result.Where(r => r.Code == RouteStageCodes.NMPT_03_3_4).ToList();
			//	}
			//}*/
			//#endregion NMPT
			//if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_01_1)
   //         {
   //             if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementInventions) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.InventionDescription) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.Essay) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.FormulaInvention))
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.BFormationPerformerChoosing).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.BFormationPerformerChoosing).ToList();
   //             }
   //         }

   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_01_1)
   //         {
   //             if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementUsefulModels) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.UsefulModelDescription) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.Essay) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.FormulaUsefulModel))
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.UMFormationPerformerChoosing).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.UMFormationPerformerChoosing).ToList();
   //             }
   //         }

   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PO_01_1)
   //         {
   //             if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementIndustrialDesigns) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.IndustrialModelDescription) &&
			//		request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SetOfImagesOfTheProductOrDrawingAndOtherMaterials))
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.POFormationPerformerChoosing).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.POFormationPerformerChoosing).ToList();
   //             }
   //         }

			//if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PO_02_1)
   //         {
   //             if (request.RequestCustomers.Any(rc => rc.Customer.IsNotResident == false && rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant))
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.PoSecret).ToList();
   //             }
   // //            var CustomerRoleCodeDeclarant = "1";
			//	//var CustomerRoleAuthor = "2";
			//	//var Roles = new List<string>() { CustomerRoleCodeDeclarant, CustomerRoleAuthor };
			//	//if (request.RequestCustomers.Any(c => Roles.Contains(c.CustomerRole.Code) && c.Customer.Type.Code != "NR" 
			//	///*&& c.Customer.IsNotResident == false*/ //не понятно нужна ли эта проверка
			//	//))
			//	//{
			//	//	result = result.Where(r => r.Code == RouteStageCodes.PoSecret).ToList();					
			//	//}
			//	else
			//	{					
			//		result = result.Where(r => r.Code == RouteStageCodes.PO_02_2).ToList();
			//	}
			//}

   //         #region SA

   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.SA_01_1)
   //         {
   //             if (request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementSelectiveAchievs)
   //                 /* &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SAQuestionnaire) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.AttributesTable) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SANegativesOrColorSlides) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SAInformationAboutPreviouslyMadeSale) &&
   //                 request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes._001_032)
   //                 */
   //                 )
   //             {
   //                 result = result.Where(r => r.Code == RouteStageCodes.SAFormationPerformerChoosing).ToList();
   //             }
   //             else
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.SAFormationPerformerChoosing).ToList();
   //             }
   //         }

   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.SA_02_1)
   //         {
   //             if (string.IsNullOrWhiteSpace(request.RequestNum))
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.SA_02_2).ToList();
   //             }
   //         }

   //         #endregion SA

   //         if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.TZNotificationSentToDeclarant)
   //         {
   //             CheckBeforeSendToGr(request, out var isStatementByLegitSender, out var isPaymentPaid);
   //             if (!isStatementByLegitSender || !isPaymentPaid)
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.TZ_03_3_8).ToList();
   //             }
   //             //else
   //             //{
   //             //    result = result.Where(r => r.Code != RouteStageCodes.TZ_03_3_7).ToList();
   //             //}
   //         }
   //         //if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.TZ_03_3_7)
   //         //{
   //         //    CheckBeforeSendToGr(request, out var isStatementByLegitSender, out var isPaymentPaid);
   //         //    if (!isStatementByLegitSender || !isPaymentPaid)
   //         //    {
   //         //        result = result.Where(r => r.Code != RouteStageCodes.TZ_03_3_8).ToList();
   //         //    }
   //         //}
   //         if (result.Any(r => r.Code == RouteStageCodes.TZConvert))
   //         {
   //             if (!(request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.TzConvertPetition) &&
   //                   request.PaymentInvoices.Any(pi =>
   //                       pi.Tariff.Code == DicTariffCodes.TmConvert &&
   //                       new[]
   //                           {
   //                               DicPaymentStatusCodes.Credited,
   //                               DicPaymentStatusCodes.Charged
   //                           }
   //                           .Contains(pi.Status.Code))))
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.TZConvert).ToList();
   //             }
   //         }
   //         if (result.Any(r => r.Code == RouteStageCodes.TZ_03_3_2_2))
   //         {
   //             if (!(request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.RequestSplitPetition) &&
   //                   request.PaymentInvoices.Any(pi =>
   //                       pi.Tariff.Code == DicTariffCodes.TmSplit &&
   //                       new[]
   //                           {
   //                               DicPaymentStatusCodes.Credited,
   //                               DicPaymentStatusCodes.Charged
   //                           }
   //                           .Contains(pi.Status.Code))))
   //             {
   //                 result = result.Where(r => r.Code != RouteStageCodes.TZ_03_3_2_2).ToList();
   //             }
   //         }
            
   //         //Проверка для исключения этапа "Делопроизводство прекращено"
   //         if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.PetitionForInvalidation))
   //             //&& 
   //             //(result.Count > 1 && request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.TZNotificationSentToDeclarant))
   //         {
   //             if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.TZNotificationSentToDeclarant && result.Count == 1)
   //             {
   //                 return result.OrderBy(r => r.IsAuto).ToList();
   //             }

   //             result = result.Where(r => !abortStageCodes.Contains(r.Code)).ToList();
   //         }

   //         return result.OrderBy(r => r.IsAuto).ToList();
        }

        /// <summary>
        /// Проверки перед передачей в Госреестр
        /// </summary>
        /// <param name="request">Заявка, состояние которой нужно проверить</param>
        /// <param name="isStatementByLegitSender">Подано ли заявление на оказание госуслуг от заявителя</param>
        /// <param name="isPaymentPaid">Оплачены ли обязательные платежи</param>
        private static void CheckBeforeSendToGr(Request request, out bool isStatementByLegitSender, out bool isPaymentPaid)
        {
            var services = new[]
                            {
                    DicDocumentTypeCodes.StateServiceRequest,
                    DicDocumentTypeCodes.StateServicesRequest
                };
            var statement = request.Documents.FirstOrDefault(rd => services.Contains(rd.Document.Type.Code) && !rd.Document.IsDeleted);

            var declarants = request.RequestCustomers.Where(rc =>
                rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
            var attorneys = request.RequestCustomers.Where(rc =>
                new[] { DicCustomerRoleCodes.Confidant, DicCustomerRoleCodes.PatentAttorney }.Contains(
                    rc.CustomerRole.Code));
            isStatementByLegitSender = declarants.Any(d => d.Customer.Xin == statement?.Document?.Addressee?.Xin) ||
                attorneys.Any(a => a.Customer.Xin == statement?.Document?.Addressee?.Xin && a.DateEnd >= DateTimeOffset.Now);
            isPaymentPaid = request.PaymentInvoices.Any(pi =>
pi.Tariff.Code == DicTariffCodes.TrademarNmptRegistrationAndPublishing &&
new[]
{
                            DicPaymentStatusCodes.Credited,
                            DicPaymentStatusCodes.Charged
}
.Contains(pi.Status.Code));
        }

        private bool UsefulModelRequestHasBiblioData(Request request)
        {
            var conventionTypeCode = request.ConventionType?.Code;

            switch (conventionTypeCode)
            {
                case DicConventionTypeCodes.PctConvention:
                    return RequestHasConventionInfo(request) && RequestHasEarlyRegs(request);
                case DicConventionTypeCodes.Pct:
                    return RequestHasConventionInfo(request);
                case DicConventionTypeCodes.Eurasian:
                    return request.RequestConventionInfos.Any(ci =>
                        ci.DateEurasianApp.HasValue && ci.PublishDateEurasianApp.HasValue &&
                        !string.IsNullOrWhiteSpace(ci.PublishRegNumberEurasianApp) &&
                        !string.IsNullOrWhiteSpace(ci.RegNumberEurasianApp));
                case DicConventionTypeCodes.Conventional:
                    return RequestHasEarlyRegs(request);
                default:
                    return true;
            }
        }

        private bool RequestHasConventionInfo(Request request)
        {
            return request.RequestConventionInfos.Any(ci =>
                ci.InternationalAppToNationalPhaseTransferDate.HasValue &&
                !string.IsNullOrWhiteSpace(ci.PublishRegNumberInternationalApp) &&
                !string.IsNullOrWhiteSpace(ci.RegNumberInternationalApp) &&
                ci.DateInternationalApp.HasValue &&
                ci.PublishDateInternationalApp.HasValue);
        }

        private bool RequestHasEarlyRegs(Request request)
        {
            return request.EarlyRegs.Any(er =>
                er.EarlyRegType.Code == DicEarlyRegTypeCodes.FirstEarlyReg &&
                !string.IsNullOrWhiteSpace(er.RegNumber) &&
                er.RegDate.HasValue && er.RegCountryId.HasValue);
        }
    }
}



