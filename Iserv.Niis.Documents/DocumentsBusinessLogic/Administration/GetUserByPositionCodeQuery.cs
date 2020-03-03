using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Administration
{
    public class GetUserByPositionCodeQuery : BaseQuery
    {
        public ApplicationUser Execute(string positionCode)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var user = userRepository
                .AsQueryable()
                .FirstOrDefault(u => u.Position.Code == positionCode && u.IsDeleted == false);

            return user;
        }
    }
}
