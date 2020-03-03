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
    public class OldNiisProtectionDocService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesService;

        private List<OldRequestInfo> _oldRequestInfos;

        public OldNiisProtectionDocService(
            OldNiisContext context,
            DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesService = dictionaryTypesHelper;
            InitializeOldRequestInfos();
        }

        public List<ProtectionDoc> GetProtectionDocs(int packageSize, int lastId)
        {
            var oldPatents = _context.BtBasePatents
                .AsNoTracking()
                .Where(p => p.Id > lastId)
                .OrderBy(p => p.Id)
                .Take(packageSize)
                .ToList();

            var protectionDocs = oldPatents.Select(p => new ProtectionDoc
            {
                Id = p.Id,
                ExternalId = p.Id,
                Barcode = p.Id,
                TypeId = p.TypeId,
                SubTypeId = p.SubTypeId,
                StatusId = p.StatusId,
                ConventionTypeId = p.TypeIiId,
                ConsiderationTypeId = p.ConsidId,
                DateCreate = p.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = p.Stamp ?? DateTimeOffset.Now,
                NameRu = p.Name540Ru,
                NameEn = p.Name540En,
                NameKz = p.Name540Kz,
                GosNumber = p.GosNumber11,
                GosDate = p.GosDate11,
                RegNumber = p.ReqNumber21,
                RegDate = p.ReqDate22,
                ValidDate = p.Stz17,
                ExtensionDate = p.Stz176,
                ProxyWithDate = p.DPos,
                ProxyForDate = p.DPop,
                DisclaimerRu = p.DisclamRu,
                DisclaimerKz = p.DisclamKz,
                TransferDate = p.Date85,
                DeclarantEmployer = CustomConverter.StringToNullableBool(p.DeclarantEmployer),
                CopyrightEmployer = CustomConverter.StringToNullableBool(p.CopyrightEmployer),
                CopyrightAuthor = CustomConverter.StringToNullableBool(p.CopyrightAuthor),
                Referat = p.Ref57,
                SelectionNameOffer = p.SelectionNameOffer,
                Otkaz = CustomConverter.IntToNullableBool(p.Otkaz),
                AdditionalInfo = p.Proch,
                OtherDocuments = p.DDok,
                DataInitialPublication = p.Paro,
                NumberApxWork = p.KodOei,
                EarlyTerminationDate = p.DatPdpat,
                LicenseInfo = p.Licenz,
                LicenseInfoStateRegister = p.Licenz1,
                NumberCopyrightCertificate = p.Nac,
                PaperworkStateRegister = p.Dvpp,
                RecoveryPetitionDate = p.DHodvost,
                ExtensionDateTz = p.Stz156,
                Code60 = p.Nm60,
                ToPm = p.ToPm,
                Transliteration = p.Transliteration,
                Image = p.Image,
                PreviewImage = p.SysImageSmall,
                IsImageFromName = false,
                RequestId = GetRequestIdByPatentId(p.Id)
            }).ToList();

            return protectionDocs;
        }

        public List<ProtectionDocInfo> GetProtectionDocInfos(List<int> protectionDocIds)
        {
            var patentInfos = _context.DdInfos
                .Where(d => protectionDocIds.Contains(d.U_ID))
                .ToList();

            var protectionDocInfos = patentInfos.Select(r => new ProtectionDocInfo
            {
                ProtectionDocId = r.U_ID,
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

            return protectionDocInfos;
        }

        public int GetProtectionDocsCount()
        {
            return _context.BtBasePatents
                .AsNoTracking()
                .Count();
        }

        #region Private Methods

        private int? GetRequestIdByPatentId(int patentId)
        {
            return _oldRequestInfos.FirstOrDefault(r => r.PatentId == patentId)?.Id;
        }

        private void InitializeOldRequestInfos()
        {
            var requestTypeIds = _dictionaryTypesService.GetRequestTypeIds();

            _oldRequestInfos = _context.DDDocuments
                .Where(d => d.Id != 0 && requestTypeIds.Contains(d.DocTypeId) && d.DocId != null)
                .Select(d => new OldRequestInfo
                {
                    Id = d.Id,
                    PatentId = d.DocId.Value
                }).ToList();
        }

        #endregion
    }

    class OldRequestInfo
    {
        public int Id { get; set; }
        public int PatentId { get; set; }
    }
}
