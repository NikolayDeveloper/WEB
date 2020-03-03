using Iserv.Niis.DataBridge.Abstract;
using Iserv.Niis.DataBridge.Repositories;
using NetCoreDataAccess.UnitOfWork;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class HandlerExecutor<THandler> : BaseExecuter<THandler>, IHandlerExecutor<THandler> 
		where THandler: IBaseCommand
	{
		#region Свойства
		//private readonly IUnitOfWork _uow;
		//private readonly IServiceProvider _services;
		//private readonly IServiceScope _services;
		//private readonly IExecutor _executor;
		//private readonly Stack<IBaseCommand> _stackToDispose = new Stack<IBaseCommand>();

		#endregion Свойства

		#region Конструктор
		//public HandlerExecutor(IServiceScope services, IExecutor executer)
		public HandlerExecutor(IServiceScope services, IExecutor executer):base(services, executer)
		{
			//_services = services;
			//_uow = _services.GetService<IUnitOfWork>();
			//_uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
			//_executor = executer;
		}
		#endregion Конструктор
		#region Деструктор
		//private bool _disposed = false;
		// Деструктор
		//~HandlerExecutor()
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
		//// внутренние действия
		//protected virtual void Dispose(bool disposing)
		//{
		//	if (_disposed)
		//		return;

		//	if (disposing)
		//	{
		//		//Освобождаем управляемые ресурсы.
		//		//while (_stackToDispose?.Count > 0)
		//		//{
		//		//	_stackToDispose?.Pop()?.Dispose();
		//		//}
		//		//_uow?.Dispose();
		//		//_services?.Dispose();
		//		//_executor?.Dispose();
		//	}
		//	//Освобождаем принудительные управляемые ресурсы.
		//	while (_stackToDispose?.Count > 0)
		//	{
		//		_stackToDispose?.Pop()?.Dispose();
		//	}

		//	// освобождаем неуправляемые объекты
		//	_disposed = true;
		//}
		#endregion

		#region Методы
		public void Process<THandlerResult>(Action<THandler> handlerAction)
		{
			THandler handler = GetInstance();
			handlerAction(handler);
			//using (THandler handler = GetInstance())
			//{
			//	handlerAction(handler);
			//}
		}

		public THandlerResult Process<THandlerResult>(Func<THandler, THandlerResult> handlerFunc)
		{
			THandler handler = GetInstance();
			var result = handlerFunc(handler);
			return result;

			//using (THandler handler = GetInstance())
			//{
			//	var result = handlerFunc(handler);
			//	return result;
			//}

		}
		#endregion Методы


		//private THandler GetInstance()
		//{
		//	THandler instance = _services.GetService<THandler>();
		//	var _uow = _services.GetService<IUnitOfWork>();
		//	//THandler instance = _services.ServiceProvider.GetRequiredService<THandler>();
		//	//var _uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//	instance.SetContext(_uow, _executor);
		//	_stackToDispose.Push(instance);
		//	return instance;
		//}
	}
}
