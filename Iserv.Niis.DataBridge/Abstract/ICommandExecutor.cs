using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Abstract
{
	public interface ICommandExecutor<out TCommand> : IBaseExecuter where TCommand : IBaseCommand
	{
		void Process(Action<TCommand> commandAction);
		TResult Process<TResult>(Func<TCommand, TResult> commandFunc);
		Task<TResult> Process<TResult>(Func<TCommand, Task<TResult>> commandFunc);
		//void ProcessWithTransaction(Action<TCommand> action);
		//TResult ProcessWithTransaction<TResult>(Func<TCommand, TResult> commandFunc);
		//Task<TResult> ProcessWithTransactionAsync<TResult>(Func<TCommand, Task<TResult>> commandFunc);
	}
}
