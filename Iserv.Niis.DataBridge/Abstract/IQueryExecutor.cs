using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Abstract
{
	public interface IQueryExecutor<out TQuery>: IBaseExecuter  where TQuery : IBaseCommand
	{
		TQueryResult Process<TQueryResult>(Func<TQuery, TQueryResult> queryFunc);
		IEnumerable<TMapResult> Process<TQueryResult, TMapResult>(Func<TQuery, ICollection<TQueryResult>> queryFunc, Func<TQueryResult, TMapResult> queryResultMapFunc);
		ValueTask<TQueryResult> Process<TQueryResult>(Func<TQuery, ValueTask<TQueryResult>> queryFunc);
		ValueTask<IEnumerable<TMapResult>> Process<TQueryResult, TMapResult>(Func<TQuery, ValueTask<ICollection<TQueryResult>>> queryFunc, Func<TQueryResult, TMapResult> queryResultMapFunc);
	}
}
