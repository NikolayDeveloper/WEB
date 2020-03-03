using System;
using System.Linq;
using System.Linq.Expressions;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class GetDicCustomersQuery : BaseQuery
    {
        public IQueryable<DicCustomer> Execute(GetDicCustomersRequest request)
        {
            var customerRepo = Uow.GetRepository<DicCustomer>();

            var dicCustomerQuery = customerRepo
                .AsQueryable()
                .Include(c => c.Type)
                .Where(d => d.IsDeleted == false)
                .Where(IsCustomerPatentAttorney(request.IsPatentAttorney));

            if (request.HasXin)
            {
                dicCustomerQuery = dicCustomerQuery.Where(IsCustomerXinContains(request.Xin));
            }

            if (request.HasName)
            {
                dicCustomerQuery = dicCustomerQuery.Where(IsCustomerNameContains(request.Name));
            }

            if (request.HasRegNumer)
            {
                dicCustomerQuery = dicCustomerQuery.Where(IsCustomerPowerAttorneyFullNumContains(request.RegNumber));
            }

            if (request.HasId)
            {
                dicCustomerQuery = dicCustomerQuery.Where(IsCustomerIdContains(request.Id));
            }

            if (request.HasCustomerTypeId)
            {
                dicCustomerQuery = dicCustomerQuery.Where(IsCustomerTypeContains(request.CustomerTypeId));
            }

            dicCustomerQuery = dicCustomerQuery.Include(c => c.Type);

            return dicCustomerQuery;
        }

        private static Expression<Func<DicCustomer, bool>> IsCustomerXinContains(string xin)
        {
            return dc => dc.Xin.ToLower().Contains(xin);
        }

        private static Expression<Func<DicCustomer, bool>> IsCustomerIdContains(int id)
        {
            return dc => dc.Id == id;
        }

        private static Expression<Func<DicCustomer, bool>> IsCustomerNameContains(string name)
        {
            return dc => (dc.NameRu.ToLower().Contains(name.ToLower())
                || dc.NameKz.ToLower().Contains(name.ToLower())
                || dc.NameEn.ToLower().Contains(name.ToLower()));
        }

        private static Expression<Func<DicCustomer, bool>> IsCustomerPowerAttorneyFullNumContains(string regNumber)
        {
            return dc => dc.PowerAttorneyFullNum.ToLower().Contains(regNumber.ToLower());
        }

        private static Expression<Func<DicCustomer, bool>> IsCustomerPatentAttorney(bool isPatentAttorney)
        {
            return dc => isPatentAttorney ? !string.IsNullOrWhiteSpace(dc.PowerAttorneyFullNum)
                                            && dc.Type.Code == DicCustomerTypeCodes.Physical : string.IsNullOrWhiteSpace(dc.PowerAttorneyFullNum);
        }
        private static Expression<Func<DicCustomer, bool>> IsCustomerTypeContains(int customerTypeId)
        {
            return dc => dc.TypeId == customerTypeId;
        }
    }
}