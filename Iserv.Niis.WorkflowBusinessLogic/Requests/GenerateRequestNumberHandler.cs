using System;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicSelectionAchieveTypes;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class GenerateRequestNumberHandler: BaseHandler
    {
        public object Execute(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!string.IsNullOrEmpty(request.RequestNum))
            {
                return null;
            }

            string protectionDocTypeCode;
            string selectionAchieveTypeCode;

            if (request.ProtectionDocType == null)
            {
                var protectionDocType = Executor.GetQuery<GetDicProtectionDocTypeByIdQuery>()
                    .Process(q => q.Execute(request.ProtectionDocTypeId));
                protectionDocTypeCode = protectionDocType?.Code;
            }
            else
            {
                protectionDocTypeCode = request.ProtectionDocType.Code;
            }
            if(request.SelectionAchieveType == null)
            {
                var selectionAchieveType = Executor.GetQuery<GetDicSelectionAchieveTypeByIdQuery>()
                    .Process(q => q.Execute(request.SelectionAchieveTypeId ?? 0));
                selectionAchieveTypeCode = selectionAchieveType?.Code;
            }
            else
            {
                selectionAchieveTypeCode = request.SelectionAchieveType.Code;
            }
            

            if (string.IsNullOrWhiteSpace(protectionDocTypeCode))
            {
                throw new ArgumentNullException($"Protection doc type code");
            }
            if (string.IsNullOrWhiteSpace(selectionAchieveTypeCode) && protectionDocTypeCode == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
            {
                throw new ArgumentNullException($"Selection Achieve Type");
            }

            var counterCode = NumberGenerator.RequestNumberCodePrefix + protectionDocTypeCode + DateTime.Now.Year;
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(counterCode));
            string requestNumber;

            switch (protectionDocTypeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    requestNumber = $"{DateTime.Now.Year}{count:D3}.3";
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    requestNumber = $"{DateTime.Now.Year}/{count:D4}.1";
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

            return null;
        }
    }
}
