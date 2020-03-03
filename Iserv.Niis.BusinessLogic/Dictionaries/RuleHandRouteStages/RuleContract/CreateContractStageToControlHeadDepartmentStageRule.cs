using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.RuleHandRouteStages.RuleContract
{
    /// <summary>
    ///     Требования для возможности перехода с этапа 'Создание договора (DK01.1)' на этап 'Распределение (DK01.2)'
    /// </summary>
    public class CreateContractStageToControlHeadDepartmentStageRule : BaseHandler
    {
        public bool Execute(Contract contract)
        {
            if (contract.AddresseeId.HasValue == false
                || (contract.Documents.Any(d => d.Document.Type.Code == DicDocumentTypeCodes.DOG_KOPPI) == false)
              )
            {
                return false;
            }

            return true;
        }
    }
}