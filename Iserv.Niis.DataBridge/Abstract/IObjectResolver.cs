using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.Implementations
{
	public interface IObjectResolver
	{
		TObject ResolveObject<TObject>();
	}
}
