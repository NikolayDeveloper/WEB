using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Abstract
{
    public interface IMapBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}