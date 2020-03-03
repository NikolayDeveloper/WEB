using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.DataBridge.Implementations
{
	public class ObjectResolver : IObjectResolver
	{
		private readonly IServiceProvider _serviceProvider;
		//private readonly IServiceScope _serviceScope;


		public ObjectResolver(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			//_serviceScope = serviceProvider.CreateScope();
		}

		public TObject ResolveObject<TObject>()
		{
			//return _serviceScope.ServiceProvider.GetRequiredService<TObject>();
			return _serviceProvider.GetRequiredService<TObject>();
		}
	}
}
