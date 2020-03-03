using System.Threading.Tasks;

namespace Iserv.Niis.Business.Abstract
{
    public interface IRoleRouteStagesUpdater
    {
        Task UpdateAsync(int roleId, int[] stagesIds);
    }
}