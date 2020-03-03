using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisContractService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisContractService(NiisWebContextMigration context)
        {
            _context = context;
        }

        internal int GetLastBarcodeId()
        {
            var lasContract = _context.Contracts.OrderByDescending(r => r.Barcode).FirstOrDefault();
            if(lasContract != null)
            {
                return lasContract.Barcode;
            }

            return 0;
        }

        internal void SaveContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            _context.SaveChanges();

            contract.CurrentWorkflowId = contract.Workflows?.OrderByDescending(r => r.DateCreate).FirstOrDefault()?.Id;
            _context.SaveChanges();
        }

        internal void UpdateContract(Contract updatedContract)
        {
            var contract = _context.Contracts.FirstOrDefault(r => r.Id == updatedContract.Id);
            if(contract == null)
            {
                throw new Exception($"Contract with id {updatedContract.Id} not found");
            }

            contract.CurrentWorkflow = updatedContract.CurrentWorkflow;
            contract.MainAttachmentId = updatedContract.MainAttachmentId;

            _context.SaveChanges();
        }

        internal void SaveContractCustomers(List<ContractCustomer> contractCustomers)
        {
            _context.ContractCustomers.AddRange(contractCustomers);

            _context.SaveChanges();
        }
    }
}
