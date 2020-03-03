using Microsoft.EntityFrameworkCore;
using NetCoreDataAccess.Repository;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Implementations
{

	/// <summary>
	/// Прокладка для того что бы сохранить клыссы BaseQuery в первозданном виде
	/// </summary>
	public class UowBridge
	{
		#region Свойства
		private readonly DbContext _context;

		#endregion Свойства
		public UowBridge(DbContext context)
		{
			this._context = context;
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
		//Task SaveChangesAsync();

		public DbSet<T> GetRepositoryOld<T>() where T : class
		{
			return _context.Set<T>();
		}
		public Repository<T> GetRepository<T>() where T : class
		{
			return new Repository<T>(_context);
		}
		public CommonRepository GetRepository()
		{
			return new CommonRepository(_context);
		}

		//CommonRepository GetRepository();
		//void SaveChanges();
		//Task SaveChangesAsync();
	}
}
