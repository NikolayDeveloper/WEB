using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Abstract
{
	public interface IHandlerExecutor<out THandler> : IBaseExecuter where THandler : IBaseCommand
	{
		void Process<THandlerResult>(Action<THandler> handlerAction);
		THandlerResult Process<THandlerResult>(Func<THandler, THandlerResult> handlerFunc);
		//IEnumerable<TMapResult> Process<THandlerResult, TMapResult>(Func<THandler, IEnumerable<THandlerResult>> handlerFunc, Func<THandlerResult, TMapResult> handlerResultMapFunc);
		//ValueTask<IEnumerable<TMapResult>> ProcessAsync<THandlerResult, TMapResult>(Func<THandler, ValueTask<IEnumerable<THandlerResult>>> handlerFunc, Func<THandlerResult, TMapResult> handlerResultMapFunc);
	}
}
