using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisRequestRelationService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesHelper;

        public OldNiisRequestRelationService(OldNiisContext context, DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesHelper = dictionaryTypesHelper;
        }

        public List<DicColorTZRequestRelation> GetRequestColotTZs(List<int> requestIds)
        {
            var colorsRequests = _context.RfTmIColorTms
                .AsNoTracking()
                .Where(c => requestIds.Contains(c.DOC_ID ?? 0))
                .ToList()
                .Distinct()
                .ToList();

            return colorsRequests.Select(c => new DicColorTZRequestRelation
            {
                ColorTzId = c.LCFEM_ID,
                RequestId = c.DOC_ID.Value
            }).ToList();
        }

        public List<RequestCustomer> GetRequestCustomers(List<int> requestIds)
        {
            var requestCustomers = _context.RfCustomers
                .AsNoTracking()
                .Where(rc => requestIds.Contains(rc.DocId))
                .OrderBy(rc => rc.Id)
                .ToList();

            return requestCustomers.Select(rc => new RequestCustomer
            {
                Id = rc.Id,
                CustomerId = rc.CustomerId,
                CustomerRoleId = rc.CType,
                RequestId = rc.DocId,
                DateBegin = rc.DateBegin,
                DateEnd = rc.DateEnd
            }).ToList();
        }

        public List<DicIcfemRequestRelation> GetDicIcfemRequestRelations(List<int> requestIds)
        {
            var icfemRequestRelations = _context.RfTmIcfems
                .AsNoTracking()
                .Where(i => requestIds.Contains(i.DOC_ID ?? 0) && i.LCFEM_ID != null)
                .OrderBy(i => i.U_ID)
                .ToList()
                .Distinct()
                .ToList();

            return icfemRequestRelations.Select(i => new DicIcfemRequestRelation
            {
                RequestId = i.DOC_ID.Value,
                DicIcfemId = i.LCFEM_ID.Value,
            }).ToList();
        }

        public List<ICGSRequest> GetICGSRequests(List<int> requestIds)
        {
            var icgsRequests = _context.RfTmIcgses
                .AsNoTracking()
                .Where(i => requestIds.Contains(i.DOC_ID ?? 0) && i.ICPS_ID != null)
                .OrderBy(i => i.U_ID)
                .ToList();

            return icgsRequests.Select(i => new ICGSRequest
            {
                Id = i.U_ID,
                IcgsId = i.ICPS_ID.Value,
                RequestId = i.DOC_ID.Value,
                IsNegative = CustomConverter.StringToNullableBool(i.IS_NEGATIVE),
                Description = i.DSC,
                DescriptionKz = i.DSC_KZ,
                IsNegativePartial = CustomConverter.StringToNullableBool(i.flIsNegativePartial)
            }).ToList();
        }

        public List<ICISRequest> GetICISRequests(List<int> requestIds)
        {
            var icisRequests = _context.RfIciss
                .AsNoTracking()
                .Where(i => requestIds.Contains(i.PATENT_ID))
                .OrderBy(i => i.U_ID)
                .ToList();

            return icisRequests.Select(i => new ICISRequest
            {
                Id = i.U_ID,
                RequestId = i.PATENT_ID,
                IcisId = i.TYPE_ID
            }).ToList();
        }

        public List<IPCRequest> GetIPCRequests(List<int> requestIds)
        {
            var ipcRequests = _context.RfIpcs
                .AsNoTracking()
                .Where(i => requestIds.Contains(i.PATENT_ID))
                .OrderBy(i => i.U_ID)
                .ToList();

            return ipcRequests.Select(i => new IPCRequest
            {
                Id = i.U_ID,
                RequestId = i.PATENT_ID,
                IpcId = i.TYPE_ID,
                IsMain = CustomConverter.StringToNullableBool(i.flIsMain) ?? false
            }).ToList();
        }

        public List<RequestEarlyReg> GetRequestEarlyRegs(List<int> requestIds)
        {
            var requestEarlyRegs = _context.WtPtEarlyRegs
                .AsNoTracking()
                .Where(re => requestIds.Contains(re.DOC_ID ?? 0) && re.ETYPE_ID != null)
                .OrderBy(re => re.U_ID)
                .ToList();

            var oldCountriesIds = requestEarlyRegs.Where(d => d.REQ_COUNTRY != null).Select(d => d.REQ_COUNTRY.GetValueOrDefault(0)).Distinct().ToList();
            var countries = _dictionaryTypesHelper.GetCountryIds(oldCountriesIds);

            return requestEarlyRegs.Select(re => new RequestEarlyReg
            {
                Id = re.U_ID,
                RequestId = re.DOC_ID.Value,
                EarlyRegTypeId = re.ETYPE_ID.Value,
                RegCountryId = countries.Any(d => d == re.REQ_COUNTRY) ? re.REQ_COUNTRY : null,
                RegNumber = re.REQ_NUMBER,
                NameSD = re.SA_NAME,
                RegDate = re.REQ_DATE
            }).ToList();
        }
    }
}
