using Iserv.Niis.DataBridge.Abstract;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.DataBridge.TestQuery;
using Iserv.Niis.Domain.Entities.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Repositories
{
	public interface IExecutor //: IDisposable
	{
		//Task<List<DicProtectionDocType>> GetTest();
		//T GetQuery<T>() where T : BaseQuery, new();
		
		ICommandChainExecutor CommandChain();
		IQueryExecutor<TQueryResult> GetQuery<TQueryResult>() where TQueryResult : IBaseCommand;// BaseQuery;//, new();
		ICommandExecutor<TCommand> GetCommand<TCommand>() where TCommand : IBaseCommand;
		IHandlerExecutor<THandler> GetHandler<THandler>() where THandler : IBaseCommand; // BaseQuery; //BaseHandler;



	}
}
//namespace NetCoreCQRS
//{
//	public class Executor : IExecutor
//	{
//		public Executor(DbContext context);
//		public ICommandChainExecutor CommandChain();
//		public ICommandExecutor<TCommand> GetCommand<TCommand>();
//		public IHandlerExecutor<THandler> GetHandler<THandler>();
//		public IQueryExecutor<TQuery> GetQuery<TQuery>();
//	}
//}