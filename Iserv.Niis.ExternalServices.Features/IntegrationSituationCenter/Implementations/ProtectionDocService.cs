using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Implementations
{
    public class ProtectionDocService : IProtectionDocService
    {
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly NiisWebContext _niisWebContext;

        public ProtectionDocService(NiisWebContext niisWebContext, DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisWebContext;
            _dictionaryHelper = dictionaryHelper;
        }

        public void GetProtectionDocs(GetBtBasePatentListArgument argument, GetBtBasePatentListResult result)
        {
            if (!argument.DateBegin.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateBegin));
            }

            if (!argument.DateEnd.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateEnd));
            }

            result.List = GetBtBasePatents(argument);
        }

        #region PrivatesMethods

        private string GetSubTypeName(int typeId, string typeName, string subTypeName)
        {
            var protectionDocTypeCommercializationContractId =
                _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                    DicProtectionDocType.Codes.CommercializationContract);
            var protectionDocTypeCopyrightId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                DicProtectionDocType.Codes.Copyright);
            if (typeId == protectionDocTypeCommercializationContractId ||
                typeId == protectionDocTypeCopyrightId)
            {
                return subTypeName ?? string.Empty;
            }

            return typeName ?? string.Empty;
        }

        private BtBasePatent[] GetBtBasePatents(GetBtBasePatentListArgument argument)
        {
            var protectionDocTypeCommercializationContractId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                DicProtectionDocType.Codes.CommercializationContract);
            var basePatents = new List<BtBasePatent>();
            var protectionDocs = _niisWebContext.ProtectionDocs
                .Where(x => x.DateUpdate.Date >= argument.DateBegin.Value.Date &&
                            x.DateUpdate.Date <= argument.DateEnd.Value.Date)
                .Select(x => new
                {
                    x.Barcode,
                    x.DateUpdate,
                    TypeName = x.Type.NameRu,
                    x.TypeId,
                    x.RegNumber,
                    x.RegDate,
                    x.GosNumber,
                    BulletinDate = x.TypeId == protectionDocTypeCommercializationContractId
                        ? x.GosDate
                        : x.Bulletins.Any()
                            ? x.Bulletins.FirstOrDefault().Bulletin.PublishDate
                            : null,
                    SubTypeName = GetSubTypeName(x.TypeId, x.Type.NameRu, x.SubType.NameRu)
                });
            foreach (var protectionDoc in protectionDocs)
            {
                basePatents.Add(new BtBasePatent
                {
                    UID = protectionDoc.Barcode,
                    GosNumber = protectionDoc.GosNumber,
                    PublishDate = protectionDoc.BulletinDate?.DateTime,
                    ReqDate = protectionDoc.RegDate?.DateTime,
                    ReqNumber = protectionDoc.RegNumber,
                    RefType = protectionDoc.SubTypeName,
                    TypeID = protectionDoc.TypeId,
                    TypeName = protectionDoc.TypeName
                });
            }

            return basePatents.ToArray();
        }

        #endregion
    }
}