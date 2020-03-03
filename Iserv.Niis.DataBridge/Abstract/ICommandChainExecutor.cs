using Iserv.Niis.DataBridge.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Abstract
{
	public interface ICommandChainExecutor : IBaseExecuter
	{
		ICommandChainExecutor AddCommand<TCommand>(Action<TCommand> commandAction) where TCommand : BaseCommand;
		void ExecuteAll();
		void ExecuteAllWithTransaction();
	}
}
