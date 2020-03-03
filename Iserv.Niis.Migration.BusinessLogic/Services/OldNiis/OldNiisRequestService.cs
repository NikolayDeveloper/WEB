using Iserv.Niis.Domain.Entities.Dictionaries;
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
    public class OldNiisRequestService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesService;
        private readonly OldNiisDictionaryService _oldNiisDictionaryService;

        public OldNiisRequestService(
            OldNiisContext context,
            DictionaryTypesHelper dictionaryTypesService,
            OldNiisDictionaryService oldNiisDictionaryService)
        {
            _context = context;
            _dictionaryTypesService = dictionaryTypesService;
            _oldNiisDictionaryService = oldNiisDictionaryService;
        }

        private List<int> _requestTypeIds
        {
            get
            {
                if (RequestTypeIds == null || RequestTypeIds.Any() == false)
                {
                    RequestTypeIds = _dictionaryTypesService.GetRequestTypeIds();
                }

                return RequestTypeIds;
            }
        }
        private static List<int> RequestTypeIds { get; set; }


        private List<DicProtectionDocType> _protectionDocTypes
        {
            get
            {
                if (ProtectionDocTypes == null || ProtectionDocTypes.Any() == false)
                {
                    ProtectionDocTypes = _oldNiisDictionaryService.GetDicProtectionDocTypes();
                }

                return ProtectionDocTypes;
            }
        }
        private static List<DicProtectionDocType> ProtectionDocTypes { get; set; }


        public int GetRequestsCount()
        {
            return _context.DDDocuments
                .AsNoTracking()
                .Where(d => _requestTypeIds.Contains(d.DocTypeId))
                .Count();
        }

        public List<Request> GetRequests(int packageSize, int lastId)
        {
            _context.Database.SetCommandTimeout(600);
            var oldRequests = _context.DDDocuments
                .Include(d => d.Patent)
                .AsNoTracking()
                .Where(d => d.Id > lastId && _requestTypeIds.Contains(d.DocTypeId))
                .OrderBy(d => d.Id)
                .Take(packageSize)
                .ToList();
            
            var requests = oldRequests.Select(r => new Request
            {
                Id = r.Id,
                Barcode = r.Id,
                ExternalId = r.Id,
                ProtectionDocTypeId = _dictionaryTypesService.MapOldRequestTypeToProtectionDocType(r.DocTypeId, _protectionDocTypes),
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.DateUpdate ?? DateTimeOffset.Now,
                AddresseeId = r.CustomerId,
                CopyCount = r.CopyCount,
                PageCount = r.PageCount,
                DepartmentId = r.DepartmentId,
                RequestNum = r.DocNum,
                RequestDate = r.DocDate,
                OutgoingNumber = r.OutNum,
                IncomingNumber = r.InOutNum,
                NameRu = r.DescMlRu,
                NameEn = r.DescMlEn,
                NameKz = r.DescMlKz,
                DivisionId = r.DivisionId,
                FlDivisionId = r.FlDivisionId,
                UserId = r.UserId,
                ReceiveTypeId = r.SendType,
                IncomingNumberFilial = r.InNumAdd,
                IsComplete = CustomConverter.StringToBool(r.IsComplete),
                IsDeleted = false,

                StatusId = r.Patent?.StatusId,
                ConventionTypeId = r.Patent?.TypeIiId,
                SelectionFamily = r.Patent?.SelectionFamily,
                Referat = r.Patent?.Ref57,
                Transliteration = r.Patent?.Transliteration,
                NumberBulletin = r.Patent?.NBy,
                DisclaimerRu = r.Patent?.DisclamRu,
                DisclaimerKz = r.Patent?.DisclamKz,
                TransferDate = r.Patent?.Date85,
                Image = r.Patent?.Image,
                PreviewImage = r.Patent?.SysImageSmall,
                IsImageFromName = false

            }).ToList();

            return requests;
        }

        public List<RequestWorkflow> GetRequestWorkflows(List<int> requestIds)
        {
            var oldRequestWorkflows = _context.WTPTWorkoffices
                .AsNoTracking()
                .Where(w => requestIds.Contains(w.DocumentId))
                .OrderBy(w => w.Id)
                .ToList();

            var requestWorkflows = oldRequestWorkflows.Select(w => new RequestWorkflow
            {
                IsComplete = CustomConverter.StringToNullableBool(w.IsComplete),
                OwnerId = w.DocumentId,
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

            return requestWorkflows;
        }

        public List<RequestInfo> GetRequestInfos(List<int> requestsIds)
        {
            var oldRequestInfos = _context.DdInfos
                .Where(d => requestsIds.Contains(d.U_ID))
                .ToList();

            var requestInfos = oldRequestInfos.Select(r => new RequestInfo
            {
                RequestId = r.U_ID,
                BreedCountryId = r.flBreedCountry == 0 ? null : r.flBreedCountry,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.stamp ?? DateTimeOffset.Now,
                FlagNine = CustomConverter.StringToBool(r.FLAG_NINE),
                FlagTat = CustomConverter.StringToBool(r.FLAG_TAT),
                FlagTpt = CustomConverter.StringToBool(r.FLAG_TPT),
                FlagTth = CustomConverter.StringToBool(r.FLAG_TTH),
                FlagTtw = CustomConverter.StringToBool(r.FLAG_TTW),
                FlagHeirship = CustomConverter.StringToBool(r.FLAG_TN),
                IzCollectiveTZ = CustomConverter.StringToNullableBool(r.COL_TZ),
                IsConventionPriority = CustomConverter.StringToNullableBool(r.PRIOR_TZ),
                Priority = r.TM_PRIORITET,
                IsExhibitPriority = CustomConverter.StringToNullableBool(r.AWARD_TZ),
                IsStandardFont = CustomConverter.StringToNullableBool(r.FONT_TZ),
                IsTransliteration = CustomConverter.StringToNullableBool(r.TRANS_TZ),
                Transliteration = r.TM_TRANSLIT,
                IsTranslation = CustomConverter.StringToNullableBool(r.INVERT_TZ),
                Translation = r.TM_TRANSLATE,
                IsVolumeTZ = CustomConverter.StringToNullableBool(r.D3_TZ),
                IsColorPerformance = CustomConverter.StringToNullableBool(r.COLOR_TZ),
                AcceptAgreement = CustomConverter.StringToNullableBool(r.REG_TZ),
                BreedingNumber = r.SEL_NOMER,
                Breed = r.SEL_ROOT,
                Genus = r.SEL_FAMILY,
                ProductType = r.PN_GOODS,
                ProductSpecialProp = $"{r.PN_DSC ?? string.Empty} {r.flProductSpecialProp ?? string.Empty}",
                ProductPlace = $"{r.PN_PLACE ?? string.Empty} {r.flProductPalce ?? string.Empty}"
            }).ToList();

            return requestInfos;
        }

        public List<int> GetAllRequestIds()
        {
            var requestTypeIds = _dictionaryTypesService.GetRequestTypeIds();
            return _context.DDDocuments
                .AsNoTracking()
                .Where(d => requestTypeIds.Contains(d.DocTypeId))
                .OrderBy(d => d.Id)
                .Select(d => d.Id)
                .ToList();
        }
    }
}
