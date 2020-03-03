using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GenerateRequestNumberHandler: BaseHandler
    {
        public async Task ExecuteAsync(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!string.IsNullOrEmpty(request.RequestNum))
            {
                return;
            }

            var protectionDocTypeCode = request.ProtectionDocType.Code;
            var requestSubtypeCode = request.RequestType?.Code ?? string.Empty;
            var selectionAchieveTypeCode = request.SelectionAchieveType?.Code ?? string.Empty;

            if (string.IsNullOrWhiteSpace(protectionDocTypeCode))
            {
                throw new ArgumentNullException($"Protection doc type code");
            }
            if (string.IsNullOrWhiteSpace(selectionAchieveTypeCode) && protectionDocTypeCode == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
            {
                throw new ArgumentNullException($"Selection Achieve Type");
            }

            var counterCode = NumberGenerator.RequestNumberCodePrefix + protectionDocTypeCode + requestSubtypeCode +
                              selectionAchieveTypeCode + DateTime.Now.Year;
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(counterCode));
            string requestNumber;

            switch (protectionDocTypeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    requestNumber = $"{DateTime.Now.Year}{count:D3}.3";
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    switch (requestSubtypeCode)
                    {
                        case DicProtectionDocSubtypeCodes.NationalInvention:
                        case DicProtectionDocSubtypeCodes.InternationalInvetion:
                            requestNumber = $"{DateTime.Now.Year}/{count:D4}.1";
                            break;
                        case DicProtectionDocSubtypeCodes.EurasianApplication:
                            requestNumber = $"{DateTime.Now.Year}/{count:D3}";
                            break;
                        case DicProtectionDocSubtypeCodes.InternationalApplication:
                            requestNumber = $"{DateTime.Now.Year}/{count:D6}";
                            break;
                        default:
                            throw new ValidationException("Некорректный подвид заявки");
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    requestNumber = $"{DateTime.Now.Year}/{count:D3}";
                    if (selectionAchieveTypeCode == DicSelectionAchieveTypeCodes.AnimalHusbandry)
                    {
                        requestNumber += ".5";
                    }
                    else if (selectionAchieveTypeCode == DicSelectionAchieveTypeCodes.Agricultural || selectionAchieveTypeCode == DicSelectionAchieveTypeCodes.VarietiesPlant)
                    {
                        requestNumber += ".4";
                    }
                    else
                    {
                        throw new ValidationException("Тип заявки должен быть либо 'Животноводство' либо 'Растениеводство'");
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    requestNumber = $"{DateTime.Now.Year}/{count:D4}.2";
                    break;
                default:
                    requestNumber = $"{count}";
                    break;
            }

            request.RequestNum = requestNumber;
            request.RequestDate = request.DateCreate;
            await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
        }
    }
}
