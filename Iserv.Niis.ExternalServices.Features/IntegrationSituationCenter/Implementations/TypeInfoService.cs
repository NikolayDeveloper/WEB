using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Implementations
{
    public class TypeInfoService : ITypeInfoService
    {
        private const string PatentTypeConst = "PatentType";
        private const string DocumentTypeConst = "DocumentType";
        private readonly DictionaryHelper _dictionaryHelper;

        private readonly NiisWebContext _niisWebContext;

        public TypeInfoService(NiisWebContext niisWebContext, DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisWebContext;
            _dictionaryHelper = dictionaryHelper;
        }

        public void GetTypesInfo(GetReferenceArgument argument, GetReferenceResult result)
        {
            var allTypes = GetAllProtectionDocTypes().Union(GetProtectionDocSubTypes());
            result.Items = allTypes.ToArray();
        }

        #region PrivateMethods

        private IEnumerable<RefItem> GetAllProtectionDocTypes()
        {
            var protectionDocTypes = _niisWebContext.DicProtectionDocTypes.ToList();
            return protectionDocTypes.Select(protectionDocType => new RefItem
            {
                ID = protectionDocType.Id,
                RefName = $"{PatentTypeConst} - {DocumentTypeConst}",
                NameRu = protectionDocType.NameRu,
                Code = protectionDocType.Code,
                NameEn = protectionDocType.NameEn,
                NameKz = protectionDocType.NameKz,
                Description = protectionDocType.Description
            });
        }

        private IEnumerable<RefItem> GetProtectionDocSubTypes()
        {
            var protectionDocTypeCommercializationContractId =
                _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                    DicProtectionDocType.Codes.CommercializationContract);
            var protectionDocTypeCopyrightId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                DicProtectionDocType.Codes.Copyright);
            var protectionDocSubTypes = _niisWebContext.DicProtectionDocSubTypes
                .Where(x => x.TypeId == protectionDocTypeCommercializationContractId ||
                            x.TypeId == protectionDocTypeCopyrightId).ToList();
            return protectionDocSubTypes.Select(protectionDocSubType => new RefItem
            {
                ID = protectionDocSubType.Id,
                RefName = $"{PatentTypeConst} - {DocumentTypeConst}",
                NameRu = protectionDocSubType.NameRu,
                Code = protectionDocSubType.Code,
                NameEn = protectionDocSubType.NameEn,
                NameKz = protectionDocSubType.NameKz,
                Description = protectionDocSubType.Description,
                ParentID = protectionDocSubType.TypeId
            });
        }

        #endregion
    }
}