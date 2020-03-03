using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Common;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Utils
{
    public class DictionaryTypesHelper : BaseHelper
    {
        private readonly OldNiisContext _oldContext;
        private readonly NiisWebContextMigration _newContext;

        public DictionaryTypesHelper(
            NewNiisDictionaryService newNiisDictionaryService,
            OldNiisContext oldContext,
            NiisWebContextMigration newContext)
        {
            _newContext = newContext;
            _oldContext = oldContext;
        }
        public List<int> GetRequestTypeIds()
        {
            return new List<int>
            {
                CLDocumentId.Copyright,
                CLDocumentId.Eapo,
                CLDocumentId.IndustrialSample,
                CLDocumentId.InnovativeInvention,
                CLDocumentId.InternationalTrademark,
                CLDocumentId.Invention,
                CLDocumentId.NameOfOrigin,
                CLDocumentId.Rst,
                CLDocumentId.SelectionAchieve,
                CLDocumentId.Trademark,
                CLDocumentId.TrademarkGenerallyKnown,
                CLDocumentId.UsefulModel,
                CLDocumentId.PreliminaryIndustrialSample
            };
        }

        public int MapOldRequestTypeToProtectionDocType(int oldRequestTypeId, List<DicProtectionDocType> dicProtectionDocTypes)
        {
            var protectionDocTypeCode = string.Empty;

            switch (oldRequestTypeId)
            {
                case CLDocumentId.Copyright:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeCopyrightCode;
                    break;
                case CLDocumentId.Eapo:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeEapoCode;
                    break;
                case CLDocumentId.IndustrialSample:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode;
                    break;
                case CLDocumentId.InnovativeInvention:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeInnovativeInventionCode;
                    break;
                case CLDocumentId.InternationalTrademark:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode;
                    break;
                case CLDocumentId.Invention:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeInventionCode;
                    break;
                case CLDocumentId.NameOfOrigin:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode;
                    break;
                case CLDocumentId.Rst:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeRstCode;
                    break;
                case CLDocumentId.SelectionAchieve:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode;
                    break;
                case CLDocumentId.Trademark:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeTrademarkCode;
                    break;
                case CLDocumentId.TrademarkGenerallyKnown:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode;
                    break;
                case CLDocumentId.UsefulModel:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeUsefulModelCode;
                    break;
                case CLDocumentId.PreliminaryIndustrialSample:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypePreliminaryIndustrialSampleCode;
                    break;
                default:
                    throw new ArgumentNullException(nameof(oldRequestTypeId));
            }
            return dicProtectionDocTypes.Single(p => p.Code == protectionDocTypeCode).Id;
        }

        public int GetDicRouteIdForDicProtectionDocType(string dicProtectionDocTypeCode, List<DicRoute> dicRoutes)
        {
            var dicRouteCode = string.Empty;
            switch (dicProtectionDocTypeCode)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeCopyrightCode:
                    dicRouteCode = RouteCodes.Copyright;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInnovativeInventionCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypePreInventionCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeEapoCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeRstCode:
                    dicRouteCode = RouteCodes.InnovativeInvention;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypePreliminaryIndustrialSampleCode:
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    dicRouteCode = RouteCodes.IndustrialDesigns;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode:
                    dicRouteCode = RouteCodes.InternationalTrademarks;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    dicRouteCode = RouteCodes.Inventions;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    dicRouteCode = RouteCodes.AppellationOfOrigin;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    dicRouteCode = RouteCodes.SelectiveAchievements;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeOther:
                    dicRouteCode = RouteCodes.TradeMark;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    dicRouteCode = RouteCodes.UsefulModel;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeContractCode:
                    dicRouteCode = RouteCodes.Contract;
                    break;
                case DicProtectionDocTypeCodes.TIM:
                    dicRouteCode = RouteCodes.TIM;
                    break;
                default:
                    throw new ArgumentNullException(nameof(dicProtectionDocTypeCode));
            }
            return dicRoutes.Single(r => r.Code == dicRouteCode).Id;
        }

        //список подтипов договоров
        private static readonly Dictionary<string, int> ContractTypes = new Dictionary<string, int>{
                { "DOG_ISKL_LIC_IZ", 228 },
                { "DOG_CHAST_UST", 229 },
                { "DOG_SUB", 230 },
                { "DOG_FRANCHISE", 232 },
                { "DOG_ISKL_LIC_TZ", 233 },
                { "DOG_NEISKL_LIC_IZ", 234 },
                { "DOG_NEISKL_LIC", 235 },
                { "DOG_ZALOG", 236 },
                { "DOG_NEISKL_SUBLIC_IZ_PM_PO", 238 },
                { "DOG_NEISKL_SUBLIC_TZ", 239 },
                { "DOG_USTUP_IZ", 240 },
                { "DOG_USTUP_TZ", 241 },
                { "DOG_USTUP_NA_PATENT_PM", 242 },
                { "DOG_USTUP_PO", 243 },
                { "DOG_USTUP_NA_OD", 244 },
                { "DOG_POLN_LIC", 4120 },
                { "DOG_OTKRYT_LIC", 4121 },
                { "DOG_OPCION", 4122 },
                { "DOG_KOPPI", 4210 },
                { "DOG_SUB_NEISK", 4211 },
                { "DOG_USTUP_SD", 4260 }
            };

        public int GetProtectionDocTypeIdByOldContractTypeId(int contractTypeId, List<DicProtectionDocType> dicProtectionDocTypes)
        {
            if (ContractTypes.ContainsValue(contractTypeId))
            {
                return dicProtectionDocTypes.Single(r => r.Code == DicProtectionDocType.Codes.CommercializationContract).Id;
            }

            throw new Exception("Contract type isn't defined");
        }

        public int GetContractTypeIdByOldContractTypeId(int contractTypeId)
        {
            if (ContractTypes.ContainsValue(contractTypeId))
            {
                var contractSubTypeCode = "";
                var contractType = ContractTypes.FirstOrDefault(r => r.Value == contractTypeId);
                switch (contractType.Key)
                {
                    case "DOG_SUB"://Сублицензионный договор исключительной лицензии
                        contractSubTypeCode = "Contract_10";
                        break;
                    case "DOG_SUB_NEISK":// Сублицензионный договор неисключительной лицензии
                        contractSubTypeCode = "Contract_10";
                        break;
                    case "DOG_NEISKL_LIC":// Лицензионный договор о предоставлении неисключительной лицензии
                        contractSubTypeCode = "Contract_11";
                        break;
                    case "DOG_ISKL_LIC_IZ":// - Лицензионный договор о предоставлении исключительной лицензии
                        contractSubTypeCode = "Contract_12";
                        break;
                    case "DOG_USTUP_IZ"://  Договор об уступке охранного документа на изобретение
                        contractSubTypeCode = "Contract_14";
                        break;
                    case "DOG_USTUP_TZ":// Договор об уступке исключительного права на товарный знак
                        contractSubTypeCode = "Contract_14";
                        break;
                    case "DOG_USTUP_NA_PATENT_PM":// Договор об уступке  патента на полезную модель
                        contractSubTypeCode = "Contract_14";
                        break;
                    case "DOG_USTUP_PO":// Договор об уступке охранного документа на промышленный образец
                        contractSubTypeCode = "Contract_14";
                        break;
                    case "DOG_USTUP_SD":// Договор об уступке патента на селекционное достижение
                        contractSubTypeCode = "Contract_14";
                        break;
                    case "DOG_CHAST_UST":// Договор о частичной уступке
                        contractSubTypeCode = "Contract_15";
                        break;
                    case "DOG_OPCION":// Договор на опционное соглашение
                        contractSubTypeCode = "Contract_16";
                        break;
                    case "DOG_FRANCHISE":// Договор франчайзинга
                        contractSubTypeCode = "Contract_17";
                        break;
                    case "DOG_ZALOG":// Договор о залоге исключительных прав
                        contractSubTypeCode = "Contract_18";
                        break;
                    case "DOG_ISKL_LIC_TZ":// Дополнительное соглашение
                        contractSubTypeCode = "Contract_20";
                        break;
                    case "DOG_USTUP_NA_OD":// Договор уступки права на получение охранного документа
                        contractSubTypeCode = "Contract_23";
                        break;


                    case "DOG_NEISKL_LIC_IZ"://Договор неисключительной лицензии на использование изобретения
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                    case "DOG_NEISKL_SUBLIC_IZ_PM_PO":// --Договор о продаже/ покупке неисключительной сублицензии на использование изобретения / полезной модели / промышленного образца
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                    case "DOG_NEISKL_SUBLIC_TZ":// --Договор о покупке/ продаже неисключительной сублицензии на использование товарного знака
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                    case "DOG_POLN_LIC"://
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                    case "DOG_OTKRYT_LIC":// Договор на открытую лицензию
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;

                    case "DOG_KOPPI":// Договор копия –используется при создании карточки «Заявление на договор коммерциализации», создается в материалах заявления как внутренний документ
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                    default:
                        contractSubTypeCode = "Contract_NotUsedCodes";
                        break;
                }

                return _newContext.DicContractTypes.Single(r => r.Code == contractSubTypeCode).Id;
            }

            throw new Exception("Contract sub type isn't defined");
        }

        public List<int> GetCountryIds(IList<int> oldIds)
        {
            var ids = _newContext.DicCountries.Select(d => d.Id).Where(d => oldIds.Contains(d)).ToList();

            return ids;
        }

        public List<int> GetContractTypeIds()
        {
            var contractTypeIds = _oldContext.Documents
                .Where(r => r.Code.StartsWith("DOG"))
                .Select(r => r.Id).ToList();

            return contractTypeIds;
        }

        public List<int> GetDocumentTypeIds()
        {
            var contractTypeIds = GetContractTypeIds();
            var requestTypeIds = GetRequestTypeIds();

            var documentTypeIds = _oldContext.Documents
                .Where(d => requestTypeIds.Contains(d.Id) == false && contractTypeIds.Contains(d.Id) == false)
                .Select(d => d.Id)
                .ToList();

            return documentTypeIds;
        }

        public int GetPaymentStatusIdByCode(string code, List<DicPaymentStatus> dicPaymentStatuses)
        {
            return dicPaymentStatuses.Single(p => p.Code == code).Id;
        }

        public int GetDicDocumentStatusIdByCode(string code, List<DicDocumentStatus> dicDicDocumentStatuses)
        {
            return dicDicDocumentStatuses.Single(p => p.Code == code).Id;
        }
    }
}
