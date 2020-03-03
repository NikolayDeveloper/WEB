using Iserv.Niis.DataBridge.Abstract;
using NetCoreDataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Iserv.Niis.DataBridge.Repositories;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class CommandExecutor<TCommand> : BaseExecuter<TCommand>, ICommandExecutor<TCommand>
		where TCommand : IBaseCommand
	{
		#region Свойства
		//private readonly IUnitOfWork _uow;
		//private readonly IServiceProvider _services;
		//private readonly Stack<IBaseCommand> _stackToDispose = new Stack<IBaseCommand>();
		//private readonly IServiceScope _services;

		#endregion Свойства

		#region Конструктор
		//public CommandExecutor(IServiceScope services)
		//public CommandExecutor(IServiceScope services):base(services)
		//{
		//	//_services = services;
		//	//_uow = _services.GetService<IUnitOfWork>();
		//	//_uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//}
		public CommandExecutor(IServiceScope services, IExecutor executer) : base(services, executer)
		{
		}

		#endregion Конструктор
		#region Деструктор
		//private bool _disposed = false;
		// Деструктор
		//~CommandExecutor()
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
		//		//while (_stackToDispose?.Count > 0)
		//		//{
		//		//	_stackToDispose?.Pop()?.Dispose();
		//		//}
		//		//Освобождаем управляемые ресурсы.
		//		//_uow?.Dispose();
		//		//_services?.Dispose();
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
		public void Process(Action<TCommand> commandAction)
		{
			TCommand command = GetInstance();
			commandAction(command);
			//using (TCommand command = GetInstance())
			//{
			//	commandAction(command);
			//}
		}

		public TResult Process<TResult>(Func<TCommand, TResult> commandFunc)
		{
			TCommand command = GetInstance();
			var result = commandFunc(command);
			return result;

			//using (TCommand command = GetInstance())
			//{
			//	var result = commandFunc(command);
			//	return result;
			//}
		}


        public async Task<TResult> Process<TResult>(Func<TCommand, Task<TResult>> commandFunc)
        {
			TCommand command = GetInstance();
			var result = await commandFunc(command);
			return result;

			//using (TCommand command = GetInstance())
   //         {
   //             var result = await commandFunc(command);
   //             return result;
   //         }
        }
        #endregion Методы


  //      private TCommand GetInstance()
		//{
		//	TCommand instance = _services.GetService<TCommand>();
		//	var _uow = _services.GetService<IUnitOfWork>();
		//	//TCommand instance = _services.ServiceProvider.GetRequiredService<TCommand>();
		//	//var _uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();
		//	instance.SetContext(_uow);
		//	_stackToDispose.Push(instance);
		//	return instance;
		//}

	}
}
