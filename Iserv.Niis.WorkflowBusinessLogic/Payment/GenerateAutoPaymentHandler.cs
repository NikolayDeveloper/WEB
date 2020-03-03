using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.Security;
using Iserv.Niis.WorkflowBusinessLogic.System;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class GenerateAutoPaymentHandler: BaseHandler
    {
        public bool Execute(int requestId)
        {
            //var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestId));
            //var rules = Executor.GetQuery<GetPaymentInvoiceGenerationRulesQuery>().Process(q => q.Execute());

            //if (!int.TryParse(
            //    Executor.GetQuery<GetSystemSettingsByTypeQuery>()
            //        .Process(q => q.Execute(SettingType.IcgsCountThreshold)), out var icgsThreshold))
            //{
            //    icgsThreshold = 3;
            //}

            //var tariffs = rules.Where(r => r.StageId == request.CurrentWorkflow.CurrentStageId
            //                               && r.Tariff.IsDeleted != true
            //                               &&
            //                               (!r.Tariff.IsCtm.HasValue || request.SpeciesTradeMarkId.HasValue &&
            //                                (r.Tariff.IsCtm == true &&
            //                                 request.SpeciesTradeMark.Code == DicProtectionDocSubtypeCodes
            //                                     .CollectiveTrademark || r.Tariff.IsCtm == false &&
            //                                 request.SpeciesTradeMark.Code != DicProtectionDocSubtypeCodes
            //                                     .CollectiveTrademark))
            //                               &&
            //                               (r.Tariff.ReceiveTypeGroup == ReceiveTypeGroupEnum.None ||
            //                                request.ReceiveTypeId.HasValue &&
            //                                (r.Tariff.ReceiveTypeGroup == ReceiveTypeGroupEnum.Paper &&
            //                                 request.ReceiveType.Code == DicReceiveTypeCodes.Courier ||
            //                                 r.Tariff.ReceiveTypeGroup ==
            //                                 ReceiveTypeGroupEnum.Electronically &&
            //                                 new[]
            //                                 {
            //                                     DicReceiveTypeCodes.ElectronicFeed,
            //                                     DicReceiveTypeCodes.ElectronicFeedEgov
            //                                 }.Contains(request.ReceiveType.Code)))).Select(r => r.Tariff).ToList();

            //var systemUser = Executor.GetQuery<GetUserByXinQuery>()
            //    .Process(q => q.Execute(UserConstants.SystemUserXin));

            //foreach (var tariff in tariffs)
            //{
            //    if (!request.PaymentInvoices.Any(pi =>
            //        pi.Tariff.Code == tariff.Code && pi.Status.Code == DicPaymentStatusCodes.Notpaid))
            //    {
            //        if (tariff.IsMoreThanIcgsThreshold.HasValue)
            //        {
            //            var tariffCount = request.ICGSRequests.Count - icgsThreshold;
            //            if (tariff.IsMoreThanIcgsThreshold == true)
            //            {
            //                if (tariffCount > 0)
            //                {
            //                    Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
            //                        h.Execute(Owner.Type.Request, requestId, tariff.Code, DicPaymentStatusCodes.Notpaid,
            //                            systemUser?.Id, tariffCount));
            //                }
            //            }
            //            else
            //            {
            //                Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
            //                    h.Execute(Owner.Type.Request, requestId, tariff.Code, DicPaymentStatusCodes.Notpaid, systemUser?.Id));
            //            }
            //        }
            //        else
            //        {
            //            Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
            //                h.Execute(Owner.Type.Request, requestId, tariff.Code, DicPaymentStatusCodes.Notpaid, systemUser?.Id));
            //        }
            //    }
            //}

            return true;
        }
    }
}
