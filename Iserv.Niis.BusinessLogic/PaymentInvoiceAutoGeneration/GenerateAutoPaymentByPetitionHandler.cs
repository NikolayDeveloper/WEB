using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentInvoiceAutoGeneration
{
    /// <summary>
    /// Генерация услуги заявки при регистрации ходатайства
    /// </summary>
    public class GenerateAutoPaymentByPetitionHandler: BaseHandler
    {
        ///// <summary>
        ///// Генерация услуги заявки при регистрации ходатайства
        ///// </summary>
        ///// <param name="documentId">Идентификатор входящего материала</param>
        ///// <param name="newRequestId">Идентификатор заявки, к которой нужно прикрепить услугу</param>
        ///// <returns></returns>
        //public async Task Execute(int documentId, int newRequestId)
        //{
        //    var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
        //    var rules = Executor.GetQuery<GetInvoiceAutoGenerationByPetitionRulesQuery>().Process(q => q.Execute());

        //    //получаем заявку, уже прикрепленную к заявке
        //    var oldRequestId = document.Requests.FirstOrDefault()?.RequestId;

        //    //получаем код тарифа из правил генерации услуг
        //    var tariffCode = rules.FirstOrDefault(r => r.PetitionTypeId == document.TypeId)?.Tariff.Code;
        //    //получаем из базы пользователя "Система" - исполнителя создания услуги
        //    var systemUser = Executor.GetQuery<GetUserByXinQuery>()
        //        .Process(q => q.Execute(UserConstants.SystemUserXin));

        //    if (!string.IsNullOrEmpty(tariffCode))
        //    {
        //        var paymentInvoices = await Executor.GetQuery<GetPaymentInvoicesByRequestIdQuery>()
        //            .Process(q => q.ExecuteAsync(oldRequestId ?? 0));
        //        var invoiceToCreate = paymentInvoices.FirstOrDefault(pi =>
        //            pi?.Tariff?.Code == tariffCode && pi?.Status?.Code == DicPaymentStatusCodes.Notpaid);
        //        if (invoiceToCreate == null)
        //        {
        //            //если нужной услуги нет в старой заявке, то создаем его в новой
        //            Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
        //                h.Execute(Owner.Type.Request, newRequestId, tariffCode,
        //                    DicPaymentStatusCodes.Notpaid, systemUser?.Id));
        //        }
        //        else
        //        {
        //            //иначе, переприкрепляем услугу из старой к новой
        //            invoiceToCreate.RequestId = newRequestId;
        //            invoiceToCreate.Request = null;
        //            Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(invoiceToCreate));
        //        }
        //    }
        //}
    }
}
