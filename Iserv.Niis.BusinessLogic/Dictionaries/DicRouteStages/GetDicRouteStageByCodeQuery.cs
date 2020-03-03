using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetDicRouteStageByCodeQuery: BaseQuery
    {
        public async Task<DicRouteStage> ExecuteAsync(string code)
        {
            var dicRouteStageRepository = Uow.GetRepository<DicRouteStage>();
            var dicRouteStage = await dicRouteStageRepository.AsQueryable()
                .FirstOrDefaultAsync(rs => rs.Code == code);

            if (dicRouteStage == null)
            {
                throw new DataNotFoundException(nameof(DicRouteStage), DataNotFoundException.OperationType.Read, code);
            }

            return dicRouteStage;
        }
    }
}
