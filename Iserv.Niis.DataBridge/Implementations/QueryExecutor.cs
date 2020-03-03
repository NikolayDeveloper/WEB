using Iserv.Niis.DataAccess.EntityFramework;
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
	public class QueryExecutor<TQuery> : BaseExecuter<TQuery>, IQueryExecutor<TQuery>
		where TQuery : IBaseCommand
	{
		#region Свойства
		#endregion Свойства

		#region Конструктор
		//public QueryExecutor(IServiceScope services):base(services)
		//{
		//}
		public QueryExecutor(IServiceScope services, IExecutor executer) : base(services, executer)
		{
		}
		#endregion Конструктор
		#region Деструктор
		#endregion

		#region Методы
		public TQueryResult Process<TQueryResult>(Func<TQuery, TQueryResult> queryFunc)
		{
			TQuery queryResult = GetInstance();
			var result = queryFunc(queryResult);
			return result;
		}
		public async ValueTask<TQueryResult> Process<TQueryResult>(
			Func<TQuery, ValueTask<TQueryResult>> queryFunc)
		{
			TQuery queryResult = GetInstance();
			var result = await queryFunc(queryResult);
			return result;
		}
		public async ValueTask<IEnumerable<TMapResult>> Process<TQueryResult, TMapResult>(
			Func<TQuery, ValueTask<ICollection<TQueryResult>>> queryFunc,
			Func<TQueryResult, TMapResult> queryResultMapFunc)
		{
			TQuery queryResult = GetInstance();
			var listResult = await queryFunc(queryResult);
			var result = listResult.Select(queryResultMapFunc);
			return result;
		}
		public IEnumerable<TMapResult> Process<TQueryResult, TMapResult>(
			Func<TQuery, ICollection<TQueryResult>> queryFunc, 
			Func<TQueryResult, TMapResult> queryResultMapFunc)
		{
			TQuery queryResult = GetInstance();
			var listResult = queryFunc(queryResult);
			var result = listResult.Select(queryResultMapFunc);
			return result;
		}
		#endregion Методы
		//protected TQuery GetInstance()
		//{
		//	TQuery instance = _services.GetService<TQuery>();
		//	var _uow = _services.GetService<IUnitOfWork>();
		//	//TQuery instance = _services.ServiceProvider.GetRequiredService<TQuery>();
		//	//var _uow = _services.ServiceProvider.GetRequiredService<IUnitOfWork>();

		//	instance.SetContext(_uow);
		//	_stackToDispose.Push(instance);
		//	return instance;
		//}




	}
}
