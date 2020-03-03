using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisContractService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesService;
        private readonly OldNiisDictionaryService _oldNiisDictionaryService;

        public OldNiisContractService(
            OldNiisContext context,
            DictionaryTypesHelper dictionaryTypesService,
            OldNiisDictionaryService oldNiisDictionaryService)
        {
            _context = context;
            _dictionaryTypesService = dictionaryTypesService;
            _oldNiisDictionaryService = oldNiisDictionaryService;
        }


        public List<int> GetAllOldContractIds()
        {
            var oldContractTypeIds = _dictionaryTypesService.GetContractTypeIds();

            var oldContractIds = _context.DDDocuments
                .Where(r => oldContractTypeIds.Contains(r.DocTypeId))
                .OrderBy(r => r.Id)
                .Select(r => r.Id).ToList();

            return oldContractIds;
        }

        public Contract GetContractByBarcodeId(int barcodeId)
        {
            var oldContract = _context.DDDocuments
                .Include(d => d.Patent)
                .FirstOrDefault(r => r.Id == barcodeId);

            if (oldContract == null)
            {
                return null;
            }

            var protectionDocTypes = _oldNiisDictionaryService.GetDicProtectionDocTypes();

            return new Contract
            {
                Id = oldContract.Id,
                ExternalId = oldContract.Id,
                Barcode = oldContract.Id,
                NameRu = oldContract.DescMlRu,
                NameEn = oldContract.DescMlEn,
                NameKz = oldContract.DescMlKz,
                DateCreate = oldContract.DateCreate ?? DateTime.Now,
                DateUpdate = oldContract.DateUpdate ?? DateTimeOffset.Now,
                Description = $"{oldContract.DescMlRu} {oldContract.DescMlKz} {oldContract.DescMlEn}",
                ReceiveTypeId = oldContract.SendType,
                OutgoingNumber = oldContract.OutNum,
                AddresseeId = oldContract.CustomerId,
                CopyCount = oldContract.CopyCount,
                PageCount = oldContract.PageCount,
                DepartmentId = oldContract.DepartmentId,
                DivisionId = oldContract.DivisionId,
                ProtectionDocTypeId = _dictionaryTypesService.GetProtectionDocTypeIdByOldContractTypeId(oldContract.DocTypeId, protectionDocTypes),
                //TODO заполнить справочник заранее
                TypeId = _dictionaryTypesService.GetContractTypeIdByOldContractTypeId(oldContract.DocTypeId),

                RegistrationPlace = "",
                BulletinDate = oldContract.Patent?.DBy,
                NumberBulletin = oldContract.Patent?.NBy,
                GosDate = oldContract.Patent?.GosDate11,
                GosNumber = oldContract.Patent?.GosNumber11,
                RegDate = oldContract.Patent?.ReqDate22,
                StatusId = oldContract.Patent?.StatusId,
                ValidDate = oldContract.Patent?.Stz17?.ToString("dd.MM.yyyy"),
                ApplicationNum = oldContract.Patent?.ReqNumber21,
                ContractNum = oldContract.Patent?.ReqNumber21,
            };
        }

        public List<ContractWorkflow> GetContractWorkflowsByOldContractId(int oldContractId)
        {
            var oldContractWorkflows = _context.WTPTWorkoffices
                .AsNoTracking()
                .Where(w => w.DocumentId == oldContractId)
                .OrderBy(w => w.Id)
                .ToList();

            var contractWorkflows = oldContractWorkflows.Select(w => new ContractWorkflow
            {
                IsComplete = CustomConverter.StringToNullableBool(w.IsComplete),
                OwnerId = oldContractId,
                ControlDate = w.ControlDate,
                DateCreate = w.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = w.DateUpdate ?? DateTimeOffset.Now,
                CurrentStageId = w.ToStageId,
                CurrentUserId = w.ToUserId,
                FromStageId = w.FromStageId,
                FromUserId = w.FromUserId,
                Description = w.Description,
                IsMain = true,
                IsSystem = CustomConverter.StringToNullableBool(w.IsSystem),
                RouteId = w.TypeId
            }).ToList();

            return contractWorkflows;
        }

        public List<ContractCustomer> GetContractCustomerByOldContractId(int oldContractId)
        {
            var oldContractCustomer = _context.RfCustomers.Where(r => r.DocId == oldContractId).ToList();

            if (oldContractCustomer.Any())
            {
                return oldContractCustomer.Select(r => new ContractCustomer
                {
                    ContractId = oldContractId,
                    CustomerId = r.CustomerId,
                    CustomerRoleId = r.CType,
                    DateCreate = r.DateCreate ?? DateTime.Now,
                    DateUpdate = DateTime.Now
                }).ToList();
            }

            return new List<ContractCustomer>();
        }

        public List<ContractDocument> GetContractsDocumentsByOldContractId(int contractId)
        {
            return null;
        }
    }
}