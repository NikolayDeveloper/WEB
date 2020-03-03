using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataBridge.Abstract;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.DataBridge.TestQuery;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Repositories
{
	public class NiisRepository : IExecutor
	{
		#region Свойства
		private readonly NiisWebContext _context;
		//private readonly IServiceProvider _services;
		private readonly IServiceScope _serviceScope;
		private readonly Stack<IBaseExecuter> _executors = new Stack<IBaseExecuter>();
		//public string name { get; set; }

		#endregion Свойства

		#region Конструктор
		public NiisRepository(NiisWebContext context, IServiceProvider services)
		{
			_context = context;
			//_services = services;
			_serviceScope = services.CreateScope();
			//name = Guid.NewGuid().ToString();
		}
		//~NiisRepository()
		//{
		//	//(_context as DbContext).Dispose();
		//	_context.Dispose();
		//}
		#endregion Конструктор


		#region Деструктор
		private bool _disposed = false;
		// Деструктор
		~NiisRepository()
		{
			Dispose(false);
		}

		// реализация интерфейса IDisposable.
		public void Dispose()
		{
			Dispose(true);
			// подавляем финализацию
			GC.SuppressFinalize(this);
		}
		//внутренние действия
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				//Освобождаем управляемые ресурсы.
				_serviceScope?.Dispose();
			}
			//Освобождаем принудительные управляемые ресурсы.
			while (_executors?.Count > 0)
			{
				_executors?.Pop()?.Dispose();
			}
			_context?.Dispose();
			_serviceScope?.Dispose();

			// освобождаем неуправляемые объекты
			_disposed = true;
		}
		#endregion
		#region Методы
		public IQueryExecutor<TQuery> GetQuery<TQuery>() where TQuery : IBaseCommand
		{
			var result = new QueryExecutor<TQuery>(_serviceScope, this);
			_executors.Push(result);
			return result;

			//using (QueryExecutor<TQuery> result = new QueryExecutor<TQuery>(_services))
			//{
			//	return result;
			//}

		}

		public ICommandExecutor<TCommand> GetCommand<TCommand>() where TCommand : IBaseCommand
		{
			var result = new CommandExecutor<TCommand>(_serviceScope, this);
			_executors.Push(result);
			return result;

			//using (CommandExecutor<TCommand> result = new CommandExecutor<TCommand>(_services))
			//{
			//	return result;
			//}
		}


		public IHandlerExecutor<THandler> GetHandler<THandler>() where THandler : IBaseCommand
		{
			var result = new HandlerExecutor<THandler>(_serviceScope, this);
			_executors.Push(result);
			return result;

			//using (HandlerExecutor<THandler> result = new HandlerExecutor<THandler>(_services, this))
			//{
			//	return result;
			//}
		}

		public ICommandChainExecutor CommandChain() {
			var result = new CommandChainExecutor(this._context, _serviceScope, this);
			_executors.Push(result);
			return result;

			//using (CommandChainExecutor result = new CommandChainExecutor(this._context, _services))
			//{
			//	return result;
			//}
		}

		#endregion Методы

	}
}
