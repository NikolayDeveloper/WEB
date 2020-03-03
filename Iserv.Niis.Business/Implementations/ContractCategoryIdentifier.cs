using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class ContractCategoryIdentifier : IContractCategoryIdentifier
    {
        private readonly NiisWebContext _context;

        public ContractCategoryIdentifier(NiisWebContext context)
        {
            _context = context;
        }

        public async Task IdentifyAsync(int? contractId)
        {
            if (contractId == null) return;

            var contract = _context.Contracts
                .Include(c => c.Category)
                .Include(c => c.ContractCustomers).ThenInclude(cc => cc.CustomerRole)
                .Include(c => c.ContractCustomers).ThenInclude(cc => cc.Customer).ThenInclude(c => c.Country)
                .SingleOrDefault(c => c.Id == contractId);

            if (contract == null) return;

            if (new[] {DicCustomerRole.Codes.Storona1, DicCustomerRole.Codes.Storona2}
                .Except(contract.ContractCustomers.Select(cc => cc.CustomerRole.Code)).Any())
            {
                return;
            }

            var categoryCode = GetCategoryCode(contract);

            if (string.IsNullOrWhiteSpace(categoryCode) || categoryCode.Equals(contract.Category?.Code)) return;
            
            contract.Category = _context.DicContractCategories.Single(c => c.Code.Equals(categoryCode));

            await _context.SaveChangesAsync();
        }

        private string GetCategoryCode(Contract contract)
        {
            var storona1CountryCodes = contract.ContractCustomers
                .Where(cc => cc.CustomerRole.Code.Equals(DicCustomerRole.Codes.Storona1))
                .Select(cc => cc.Customer?.Country?.Code)
                .ToList();
            var storona2CountryCodes = contract.ContractCustomers
                .Where(cc => cc.CustomerRole.Code.Equals(DicCustomerRole.Codes.Storona2))
                .Select(cc => cc.Customer?.Country?.Code)
                .ToList();

            if (storona1CountryCodes.Any(s1 => s1 != DicCountryCodes.Kazakhstan))
            {
                return storona2CountryCodes.Any(s2 => s2 != DicCountryCodes.Kazakhstan)
                    ? DicContractCategory.Codes.ForeignPartners
                    : DicContractCategory.Codes.ReceivingPartyIsNational;
            }

            return storona2CountryCodes.Any(s2 => s2 != DicCountryCodes.Kazakhstan)
                ? DicContractCategory.Codes.ReceivingPartyIsForeigner
                : DicContractCategory.Codes.NationalPartners;
        }
    }
}