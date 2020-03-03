using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.DataBridge.Repositories;
using Microsoft.EntityFrameworkCore;
using NetCoreDataAccess.UnitOfWork;
using System;


namespace Iserv.Niis.DataBridge.Abstract
{
	public interface IBaseCommand : IDisposable
	{
		IExecutor Executor { get; set; }
		IUnitOfWork Uow { get; set; }
		//void SetContext(IUnitOfWork unitOfWork);
		void SetContext(IUnitOfWork unitOfWork, IExecutor executor);
	}
}
