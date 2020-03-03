using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GenerateAutoNotificationHandler: BaseHandler
    {
        //public async Task Execute(int requestId, int amount = 100)
        //{
        //    var rules = Executor.GetQuery<GetNotificationAutoGenerationRulesQuery>().Process(q => q.Execute());
        //    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
        //    var requestDocuments = await Executor.GetQuery<GetDocumentsByRequestIdQuery>()
        //        .Process(q => q.ExecuteAsync(requestId));
        //    var paymentInvoices = await Executor.GetQuery<GetPaymentInvoicesByRequestIdQuery>()
        //        .Process(q => q.ExecuteAsync(requestId));
        //    var documentTypeIds = requestDocuments.Select(rd => rd.TypeId);
        //    var tariffIds = paymentInvoices.Where(pi => pi.Tariff.Price >= pi.PaymentUses.Sum(pu => pu.Amount) + amount
        //                                                || new[]
        //                                                {
        //                                                    DicPaymentStatusCodes.Credited,
        //                                                    DicPaymentStatusCodes.Charged
        //                                                }.Contains(pi.Status.Code)).Select(pi => pi.TariffId);
        //    var currentStageRules = rules.Where(r => r.StageId == request.CurrentWorkflow.CurrentStageId);
        //    if (currentStageRules.Any() &&
        //        currentStageRules.Select(r => r.PetitionTypeId).All(documentTypeIds.Contains) &&
        //        currentStageRules.Select(r => r.TariffId).All(tariffIds.Contains))
        //    {
        //        var documentCodes = currentStageRules.Where(d => !documentTypeIds.Contains(d.NotificationTypeId)).Select(r => r.NotificationType.Code).Distinct();
        //        foreach (var documentCode in documentCodes)
        //        {
        //            if (!string.IsNullOrEmpty(documentCode))
        //            {
        //                var userInputDto = new UserInputDto
        //                {
        //                    Code = documentCode,
        //                    Fields = new List<KeyValuePair<string, string>>(),
        //                    OwnerId = requestId,
        //                    OwnerType = Owner.Type.Request
        //                };
        //                Executor.GetHandler<WorkflowBusinessLogic.Document.CreateDocumentHandler>().Process(h =>
        //                    h.Execute(requestId, Owner.Type.Request, documentCode, DocumentType.Outgoing,
        //                        userInputDto));
        //            }
        //        }
        //    }
        //}
    }
}
