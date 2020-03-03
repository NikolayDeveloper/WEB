using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class AmbientContext : IAmbientContext
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IObjectResolver _resolver;

		public IObjectResolver Resolver { get { return _resolver; } }
		//public IObjectResolver Resolver { 
		//	get {
		//		IObjectResolver _resolver  = _serviceProvider.GetRequiredService<IObjectResolver>();
		//		return _resolver; 
		//	} 
		//}


		private static AmbientContext _current;

		public static AmbientContext Current
		{
			get
			{
				if (_current == null)
				{
					throw new Exception($"{nameof(AmbientContext)} current is null");
				}

				return _current;
			}
		}

		public AmbientContext(IServiceProvider serviceProvider) 
		{
			_serviceProvider = serviceProvider;
			_resolver = new ObjectResolver(serviceProvider);
			_current = this;
		}


		//public IUnitOfWork UnitOfWork { get; }

	}
}
