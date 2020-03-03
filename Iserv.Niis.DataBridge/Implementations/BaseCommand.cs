using Iserv.Niis.DataBridge.Abstract;
using Iserv.Niis.DataBridge.Repositories;
using Microsoft.EntityFrameworkCore;
using NetCoreDataAccess.UnitOfWork;
using System;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class BaseCommand : IBaseCommand
	{
		#region Свойства
		public IExecutor Executor { get; set; }
		public IUnitOfWork Uow { get; set; }
		#endregion Свойства

		#region Конструктор
		public BaseCommand()
		{
			
		}
		#endregion Конструктор


		/// <summary>
		/// Метод в который передаётся текущий  unitOfWork, нужно выполнить после создание класса наследника
		/// Не переносить в конструктор для совместимости.
		/// </summary>
		/// <param name="context"></param>
		//public void SetContext(IUnitOfWork unitOfWork)
		//{
		//	Uow = unitOfWork;
		//}
		public void SetContext(IUnitOfWork unitOfWork, IExecutor executor)
		{
			Uow = unitOfWork;
			Executor = executor;
		}


		#region Деструктор
		private bool _disposed = false;
		// Деструктор
		~BaseCommand()
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
				Uow?.Dispose();
				//Executor?.Dispose();
			}
			//Освобождаем принудительные управляемые ресурсы.

			// освобождаем неуправляемые объекты
			_disposed = true;
		}
		#endregion
	}
}
