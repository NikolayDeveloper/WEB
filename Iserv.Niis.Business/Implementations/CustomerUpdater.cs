using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Abstractions;
using Iserv.Niis.Model.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class CustomerUpdater : ICustomerUpdater
    {
        private readonly NiisWebContext _context;
        private readonly IGbdFlService _gbdFlService;
        private readonly IGbdJuridicalService _gbdJuridicalService;

        public CustomerUpdater(NiisWebContext context, IGbdJuridicalService gbdJuridicalService,
            IGbdFlService gbdFlService)
        {
            _context = context;
            _gbdJuridicalService = gbdJuridicalService;
            _gbdFlService = gbdFlService;
        }

        public void UpdateAddressee(Request request, RequestDetailDto requestDetailDto)
        {
            var customer = GetCustomer(request.AddresseeId);

            if (customer == null)
            {
                return;
            }

            customer.Address = requestDetailDto.AddresseeAddress;
            customer.ShortAddress = requestDetailDto.AddresseeShortAddress;
        }

        public void Update(DicCustomer customer)
        {
            var originalCustomer = GetCustomer(customer.Id);
            if (originalCustomer == null)
            {
                return;
            }

            originalCustomer.NameRu = customer.NameRu;
            originalCustomer.NameEn = customer.NameEn;
            originalCustomer.NameKz = customer.NameKz;
            originalCustomer.ShortAddress = customer.ShortAddress;
            originalCustomer.Address = customer.Address;
            originalCustomer.AddressKz = customer.AddressKz;
            originalCustomer.AddressEn = customer.AddressEn;
            originalCustomer.IsBeneficiary = customer.IsBeneficiary;
            originalCustomer.TypeId = customer.TypeId;
            originalCustomer.CountryId = customer.CountryId;
            originalCustomer.JurRegNumber = customer.JurRegNumber;
            originalCustomer.IsNotMention = customer.IsNotMention;
            originalCustomer.Apartment = customer.Apartment;
            originalCustomer.BeneficiaryTypeId = customer.BeneficiaryTypeId;
            originalCustomer.City = customer.City;
            originalCustomer.Oblast = customer.Oblast;
            originalCustomer.Republic = customer.Republic;
            originalCustomer.Region = customer.Region;
            originalCustomer.Street = customer.Street;
            foreach (var contactInfo in originalCustomer.ContactInfos)
            {
                _context.ContactInfos.Remove(contactInfo);
            }

            originalCustomer.ContactInfos = customer.ContactInfos;
            _context.SaveChanges();
        }

        public async Task<DicCustomer> GetCustomer(string xin, bool? isPatentAttorney)
        {
            //var customer = await FindCustomerAsync(c => c.Xin.Equals(xin), xin, isPatentAttorney);

            //if (customer != null)
            //{
            //    return customer;
            //}


            var customerJur = _gbdJuridicalService.GetCustomerInfo(xin);
            if (customerJur != null)
            {
                return customerJur;

                //await SaveNewCustomer(customerJur);

                //return await FindCustomerAsync(c => c.Id == customerJur.Id, xin, isPatentAttorney);
            }

            var customerFl = _gbdFlService.GetCustomer(xin);
            if (customerFl != null)
            {
                return customerFl;

                //await SaveNewCustomer(customerFl);

                //return await FindCustomerAsync(c => c.Id == customerFl.Id, xin, isPatentAttorney);
            }

            return null;
        }

        private async Task SaveNewCustomer(DicCustomer customerJur)
        {
            await _context.DicCustomers.AddAsync(customerJur);

            await _context.SaveChangesAsync();
        }

        private DicCustomer GetCustomer(int? customerId)
        {
            return customerId == null
                ? null
                : _context.DicCustomers.Include(dc => dc.ContactInfos)
                    .SingleOrDefault(c => c.Id == customerId);
        }

        private async Task<DicCustomer> FindCustomerAsync(Expression<Func<DicCustomer, bool>> predicate, string xin,
            bool? isPatentAttorney)
        {
            var customers = await _context.DicCustomers
                .Include(c => c.Type)
                .Include(c => c.CustomerAttorneyInfos)
                .Include(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Where(d => d.IsDeleted == false)
                .Where(predicate)
                .Where(r => isPatentAttorney == null || (isPatentAttorney.Value
                                ? r.PowerAttorneyFullNum != null
                                : r.PowerAttorneyFullNum == null))
                .ToArrayAsync();

            if (customers.Count() > 1 && isPatentAttorney != null)
            {
                throw new ValidationException($"Argument XIN with {xin} value has multiple match!");
            }

            return customers.FirstOrDefault();
        }
    }
}