using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisProtectionDocRelationService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesHelper;

        public OldNiisProtectionDocRelationService(OldNiisContext context, DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesHelper = dictionaryTypesHelper;
        }

        public List<DicColorTZProtectionDocRelation> GetDicColorTZProtectionDocRelations(List<int> protectionDocIds)
        {
            var protectionDocColors = _context.RfTmIColorTms
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DOC_ID ?? 0))
                .ToList()
                .Distinct()
                .ToList();

            return protectionDocColors.Select(r => new DicColorTZProtectionDocRelation
            {
                ProtectionDocId = r.DOC_ID.Value,
                ColorTzId = r.LCFEM_ID
            }).ToList();
        }

        public List<ProtectionDocCustomer> GetProtectionDocCustomers(List<int> protectionDocIds)
        {
            var protectionDocCustomers = _context.RfCustomers
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DocId))
                .ToList();

            return protectionDocCustomers.Select(r => new ProtectionDocCustomer
            {
                CustomerId = r.CustomerId,
                ProtectionDocId = r.DocId,
                CustomerRoleId = r.CType,
                DateBegin = r.DateBegin,
                DateEnd = r.DateEnd,
                DateCreate = r.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = r.Stamp ?? DateTimeOffset.Now,
            }).ToList();
        }

        public List<DicIcfemProtectionDocRelation> GetDicIcfemProtectionDocRelations(List<int> protectionDocIds)
        {
            var protectionDocIcfems = _context.RfTmIcfems
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DOC_ID ?? 0) && r.LCFEM_ID != null)
                .ToList()
                .Distinct()
                .ToList();

            return protectionDocIcfems.Select(r => new DicIcfemProtectionDocRelation
            {
                ProtectionDocId = r.DOC_ID.Value,
                DicIcfemId = r.LCFEM_ID.Value
            }).ToList();
        }

        public List<ICGSProtectionDoc> GetICGSProtectionDocs(List<int> protectionDocIds)
        {
            var protectionDocIcgses = _context.RfTmIcgses
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DOC_ID ?? 0) && r.ICPS_ID != null)
                .ToList();

            return protectionDocIcgses.Select(r => new ICGSProtectionDoc
            {
                ProtectionDocId = r.DOC_ID.Value,
                IcgsId = r.ICPS_ID.Value,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.stamp ?? DateTimeOffset.Now,
                Description = r.DSC,
                DescriptionKz = r.DSC_KZ,
                NegativeDescription = r.NDSC_CLOB,
                ClaimedDescription = r.flDscStarted,
                IsNegative = CustomConverter.StringToNullableBool(r.IS_NEGATIVE),
                IsNegativePartial = CustomConverter.StringToNullableBool(r.flIsNegativePartial)
            }).ToList();
        }

        public List<ICISProtectionDoc> GetICISProtectionDocs(List<int> protectionDocIds)
        {
            var protectionDocIcises = _context.RfIciss
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.PATENT_ID))
                .ToList();

            return protectionDocIcises.Select(r => new ICISProtectionDoc
            {
                ProtectionDocId = r.PATENT_ID,
                IcisId = r.TYPE_ID,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.date_create ?? DateTimeOffset.Now,
            }).ToList();
        }

        public List<IPCProtectionDoc> GetIPCProtectionDocs(List<int> protectionDocIds)
        {
            var protectionDocIpcs = _context.RfIpcs
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.PATENT_ID))
                .ToList();

            return protectionDocIpcs.Select(r => new IPCProtectionDoc
            {
                ProtectionDocId = r.PATENT_ID,
                IpcId = r.TYPE_ID,
                IsMain = CustomConverter.StringToNullableBool(r.flIsMain) ?? true,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.stamp ?? DateTimeOffset.Now
            }).ToList();
        }

        public List<ProtectionDocEarlyReg> GetProtectionDocEarlyRegs(List<int> protectionDocIds)
        {
            var protectionDocEarlyRegs = _context.WtPtEarlyRegs
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DOC_ID ?? 0) && r.ETYPE_ID != null)
                .ToList();

            var oldCountriesIds = protectionDocEarlyRegs.Where(d => d.REQ_COUNTRY != null).Select(d => d.REQ_COUNTRY.GetValueOrDefault(0)).Distinct().ToList();
            var countries = _dictionaryTypesHelper.GetCountryIds(oldCountriesIds);

            return protectionDocEarlyRegs.Select(r => new ProtectionDocEarlyReg
            {
                ProtectionDocId = r.DOC_ID.Value,
                EarlyRegTypeId = r.ETYPE_ID.Value,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.date_create ?? DateTimeOffset.Now,
                DateF1 = r.DATE_F1,
                DateF2 = r.DATE_F2,
                Description = r.DESCRIPTION,
                NameSD = r.SA_NAME,
                RegCountryId = countries.Any(d => d == r.REQ_COUNTRY) ? r.REQ_COUNTRY : null,
                RegDate = r.REQ_DATE,
                RegNumber = r.REQ_NUMBER,
                PCTType = r.PCTYPE
            }).ToList();
        }

        public List<ProtectionDocRedefine> GetProtectionDocRedefines(List<int> protectionDocIds)
        {
            var protectionDocRedefines = _context.WtPtRedefines
                .AsNoTracking()
                .Where(r => protectionDocIds.Contains(r.DOC_ID ?? 0))
                .ToList();

            return protectionDocRedefines.Select(r => new ProtectionDocRedefine
            {
                ProtectionDocId = r.DOC_ID.Value,
                RedefinitionTypeId = r.TYPE_ID,
                DateCreate = r.date_create ?? DateTimeOffset.Now,
                DateUpdate = r.date_create ?? DateTimeOffset.Now,
                Description = r.DSC,
                DescriptionKz = r.DSC_KZ
            }).ToList();
        }
    }
}
