using System;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Utils.Helpers
{
    public class GenerateHelper
    {
        private const string StringTrue = "T";
        private const string StringFalse = "F";

        public static bool StringToBool(string value)
        {
            switch (value)
            {
                case StringTrue:
                    return true;
                case StringFalse:
                    return false;
                default:
                    throw new ArgumentNullException(value);
            }
        }

        public static bool? StringToNullableBool(string value)
        {
            switch (value)
            {
                case StringTrue:
                    return true;
                case StringFalse:
                    return false;
                default:
                    return null;
            }
        }
        
        public static DocumentType GetDocumentType(string workTypeCode)
        {
            switch (workTypeCode)
            {
                case "IN": //Обработка входящей корреспонденции 
                    return DocumentType.Incoming;
                case "OUT": //Обработка исходящей корреспонденции
                    return DocumentType.Outgoing;
                case "W": //Внутренняя переписка
                    return DocumentType.Internal;
                case "INТТТ": //Обработка входящей корреспонденции 
                    return DocumentType.Incoming;
                case "DocReq": //Документ даявки
                    return DocumentType.DocumentRequest;
                default:
                    return DocumentType.DocumentRequest;
            }
        }

    }

    public static class MapOldRequestTypeToProtectionDocType
    {
        public static string Get(int oldRequestTypeId)
        {
            string protectionDocTypeCode;

            switch (oldRequestTypeId)
            {
                case ClDocumentId.Copyright:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeCopyrightCode;
                    break;
                case ClDocumentId.IndustrialSample:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode;
                    break;
                case ClDocumentId.InnovativeInvention:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeInnovativeInventionCode;
                    break;
                case ClDocumentId.InternationalTrademark:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode;
                    break;
                case ClDocumentId.Invention:
                case ClDocumentId.Eapo:
                case ClDocumentId.Rst:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeInventionCode;
                    break;
                case ClDocumentId.NameOfOrigin:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode;
                    break;
                case ClDocumentId.SelectionAchieve:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode;
                    break;
                case ClDocumentId.Trademark:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeTrademarkCode;
                    break;
                case ClDocumentId.TrademarkGenerallyKnown:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode;
                    break;
                case ClDocumentId.UsefulModel:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.RequestTypeUsefulModelCode;
                    break;
                case ClDocumentId.PreliminaryIndustrialSample:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypePreliminaryIndustrialSampleCode;
                    break;
                default:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeOther;
                    break;
            }

            return protectionDocTypeCode;
            //return GlobalIdentity.GetDbPrimaryKeyByCode<DicProtectionDocTypes>(protectionDocTypeCode);
        }
    }

    public static class ContractTypeIdByOldContractTypeId
    {
        public static string Get(string contractTypeCode)
        {
            string contractSubTypeCode;
            switch (contractTypeCode)
            {
                case "DOG_SUB": //Сублицензионный договор исключительной лицензии
                    contractSubTypeCode = "Contract_10";
                    break;
                case "DOG_SUB_NEISK": // Сублицензионный договор неисключительной лицензии
                    contractSubTypeCode = "Contract_10";
                    break;
                case "DOG_NEISKL_LIC": // Лицензионный договор о предоставлении неисключительной лицензии
                    contractSubTypeCode = "Contract_11";
                    break;
                case "DOG_ISKL_LIC_IZ": // - Лицензионный договор о предоставлении исключительной лицензии
                    contractSubTypeCode = "Contract_12";
                    break;
                case "DOG_USTUP_IZ": //  Договор об уступке охранного документа на изобретение
                    contractSubTypeCode = "Contract_14";
                    break;
                case "DOG_USTUP_TZ": // Договор об уступке исключительного права на товарный знак
                    contractSubTypeCode = "Contract_14";
                    break;
                case "DOG_USTUP_NA_PATENT_PM": // Договор об уступке  патента на полезную модель
                    contractSubTypeCode = "Contract_14";
                    break;
                case "DOG_USTUP_PO": // Договор об уступке охранного документа на промышленный образец
                    contractSubTypeCode = "Contract_14";
                    break;
                case "DOG_USTUP_SD": // Договор об уступке патента на селекционное достижение
                    contractSubTypeCode = "Contract_14";
                    break;
                case "DOG_CHAST_UST": // Договор о частичной уступке
                    contractSubTypeCode = "Contract_15";
                    break;
                case "DOG_OPCION": // Договор на опционное соглашение
                    contractSubTypeCode = "Contract_16";
                    break;
                case "DOG_FRANCHISE": // Договор франчайзинга
                    contractSubTypeCode = "Contract_17";
                    break;
                case "DOG_ZALOG": // Договор о залоге исключительных прав
                    contractSubTypeCode = "Contract_18";
                    break;
                case "DOG_ISKL_LIC_TZ": // Дополнительное соглашение
                    contractSubTypeCode = "Contract_20";
                    break;
                case "DOG_USTUP_NA_OD": // Договор уступки права на получение охранного документа
                    contractSubTypeCode = "Contract_23";
                    break;
                case "DOG_NEISKL_LIC_IZ": //Договор неисключительной лицензии на использование изобретения
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
                case "DOG_NEISKL_SUBLIC_IZ_PM_PO"
                    : // --Договор о продаже/ покупке неисключительной сублицензии на использование изобретения / полезной модели / промышленного образца
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
                case "DOG_NEISKL_SUBLIC_TZ"
                    : // --Договор о покупке/ продаже неисключительной сублицензии на использование товарного знака
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
                case "DOG_POLN_LIC": //
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
                case "DOG_OTKRYT_LIC": // Договор на открытую лицензию
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;

                case "DOG_KOPPI"
                    : // Договор копия –используется при создании карточки «Заявление на договор коммерциализации», создается в материалах заявления как внутренний документ
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
                default:
                    contractSubTypeCode = "Contract_NotUsedCodes";
                    break;
            }

            if (string.IsNullOrEmpty(contractSubTypeCode))
                throw new NotSupportedException("Код типа договора не найден " + nameof(contractSubTypeCode));
            return contractSubTypeCode;
        }
    }
}