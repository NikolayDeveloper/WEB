using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Integration.OneC.Infrastructure
{
    /// <summary>
    /// Заглушка для работы с <see cref="BaseQuery"/> и <see cref="BaseCommand"/>.
    /// </summary>
    public class NiisOneEmptyDbContext : DbContext
    {
    }
}
