using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class FilterOwnersByOwnerTypeHandler: BaseHandler
    {
        public List<MaterialOwnerDto> Execute(Owner.Type ownerType, List<MaterialOwnerDto> ownerDtos)
        {
            return ownerDtos.Where(o => o.OwnerType == ownerType)
                .ToList();
        }
    }
}
