using NetCoreDataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;

namespace Iserv.Niis.DataBridge.Abstract
{
	public class BaseExecuter<TBaseCommand> : IBaseExecuter
		where TBaseCommand : IBaseCommand
	{
		protected readonly Stack<IBaseCommand> _stackToDispose = new Stack<IBaseCommand>();
		//protected readonly IServiceProvider _services;
		protected readonly IServiceScope _serviceScope;
		protected readonly IExecutor _executor;
		protected readonly IUnitOfWork _uow;

		#region Конструктор
		//public BaseExecuter(IServiceScope services)
		//{
		//	_serviceScope = services;
		//	_uow = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//	//_serviceScope = services.CreateScope();
		//	//_services = services;
		//}
		public BaseExecuter(IServiceScope services, IExecutor executer) 
		{
			_serviceScope = services;
			_uow = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
			//_serviceScope = services.CreateScope();
			//_services = services;
			_executor = executer;
		}

		#endregion Конструктор


		#region Деструктор
		protected bool _disposed = false;
		// Деструктор
		~BaseExecuter()
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
		// внутренние действия
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				//Освобождаем управляемые ресурсы.
				_uow?.Dispose();
				//_serviceScope?.Dispose();
			}
			//Освобождаем принудительные управляемые ресурсы.
			while (_stackToDispose?.Count > 0)
			{
				_stackToDispose?.Pop()?.Dispose();
			}

			// освобождаем неуправляемые объекты
			_disposed = true;
		}

		#endregion
		protected TBaseCommand GetInstance()
		{
			//TBaseCommand instance = _services.GetService<TBaseCommand>();
			//var _uow = _services.GetService<IUnitOfWork>();
			TBaseCommand instance = _serviceScope.ServiceProvider.GetRequiredService<TBaseCommand>();
			//var _uow = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
			instance.SetContext(_uow, _executor);
			//instance.SetContext(_uow);
			_stackToDispose.Push(instance);
			return instance;
		}
		protected IBaseCommand GetInstance(Type type)
		{
			//var instance = (_services.GetService(type) as IBaseCommand);
			//var _uow = _services.GetService<IUnitOfWork>();
			var instance = (_serviceScope.ServiceProvider.GetRequiredService(type) as IBaseCommand);
			//var _uow = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
			//instance.SetContext(_uow);
			instance.SetContext(_uow, _executor);
			_stackToDispose.Push(instance);
			return instance;
		}

	}
}
