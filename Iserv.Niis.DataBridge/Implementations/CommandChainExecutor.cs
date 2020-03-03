using Iserv.Niis.DataBridge.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetCoreDataAccess.UnitOfWork;
using Iserv.Niis.DataBridge.Repositories;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class CommandChainExecutor : BaseExecuter<IBaseCommand>, ICommandChainExecutor
	{
		private struct CommandList {
			public Delegate action { get; set; }
			public Type type { get; set; }
		}


		#region Свойства
		//private readonly IUnitOfWork _uow;
		private List<CommandList> _commandList { get; set; }
		private DbContext _dbContext { get; set; }
		//private readonly IServiceProvider _services;
		//private readonly Stack<IBaseCommand> _stackToDispose = new Stack<IBaseCommand>();
		//private readonly IServiceScope _services;
		#endregion Свойства

		#region Конструктор
		//public CommandChainExecutor(DbContext dbContext, IServiceScope services)
		//public CommandChainExecutor(DbContext dbContext, IServiceScope services):base(services)
		//{
		//	_commandList = new List<CommandList>();
		//	_dbContext = dbContext;
		//	//_services = services;
			
		////_uow = _services.GetService<IUnitOfWork>();
		////_uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//}
		public CommandChainExecutor(DbContext dbContext, IServiceScope services, IExecutor executer) : base(services, executer)
		{
			_commandList = new List<CommandList>();
			_dbContext = dbContext;
			//_services = services;
			//_uow = _services.GetService<IUnitOfWork>();
			//_uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
			//_executor = executer;
		}
		#endregion Конструктор
		#region Деструктор
		//private bool _disposed = false;
		// Деструктор
		//~CommandChainExecutor()
		//{
		//	Dispose(false);
		//}

		//// реализация интерфейса IDisposable.
		//public void Dispose()
		//{
		//	Dispose(true);
		//	// подавляем финализацию
		//	GC.SuppressFinalize(this);
		//}
		// внутренние действия
		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				//Освобождаем управляемые ресурсы.

				//_dbContext?.Dispose();
				//_serviceScope?.Dispose();
				_uow?.Dispose();
			}
			//Освобождаем принудительные управляемые ресурсы.
			_commandList?.Clear();
			while (_stackToDispose?.Count > 0)
			{
				_stackToDispose?.Pop()?.Dispose();
			}

			// освобождаем неуправляемые объекты
			_disposed = true;
		}
		#endregion
		#region Методы
		public ICommandChainExecutor AddCommand<TCommand>(Action<TCommand> commandAction) where TCommand : BaseCommand
		{
			Type curtype = typeof(TCommand);
			_commandList.Add(new CommandList { action = commandAction, type = curtype });
			return this;
		}

		// Выполнить все команды.
		public void ExecuteAll()
		{
			foreach (CommandList item in _commandList)
			{
				IBaseCommand tCommand = GetInstance(item.type);
				item.action.DynamicInvoke(tCommand);
				//using (IBaseCommand tCommand = GetInstance(item.type))
				//{
				//	item.action.DynamicInvoke(tCommand);
				//}

				//var tCommand = _services.GetService(item.type);
				//(tCommand as BaseCommand).SetContext(_uow);
				//item.action.DynamicInvoke(tCommand);
			}
		}

		// Выполнить все команды в одной транзакции.
		public void ExecuteAllWithTransaction()
		{
			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					ExecuteAll();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction?.Rollback();
					throw ex;
				}
			}
		}
		#endregion Методы
		//private IBaseCommand GetInstance(Type type)
		//{
		//	var instance = (_services.GetService(type) as IBaseCommand);
		//	var _uow = _services.GetService<IUnitOfWork>();
		//	//var instance = (_services.ServiceProvider.GetRequiredService(type) as IBaseCommand);
		//	//var _uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//	instance.SetContext(_uow);
		//	_stackToDispose.Push(instance);
		//	return instance;
		//}
	}
}
