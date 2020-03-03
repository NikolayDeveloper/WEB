using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.DataBridge.TestQuery
{
	//тестовый простейший класс BaseQuery
	//public class GetDicProtectionDocTypesQuery1 : BaseQuery
	//{
	//	public async Task<List<DicProtectionDocType>> ExecuteAsync()
	//	{
	//		var protectionDocTypeRepo = Uow.GetRepository<DicProtectionDocType>();
	//		return await protectionDocTypeRepo
	//			.AsQueryable()
	//			.Include(d => d.Route)
	//			.ToListAsync();
	//	}
	//}
}
