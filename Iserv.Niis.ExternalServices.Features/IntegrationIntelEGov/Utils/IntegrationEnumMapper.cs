using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    public class IntegrationEnumMapper
    {
        public string MapProtectionDocTypeToDocumentType(string protectionDocType)
        {
            switch (protectionDocType)
            {
                case DicProtectionDocType.Codes.Invention:
                    return DicDocumentTypeCodes.StatementInventions;
                case DicProtectionDocType.Codes.PlaceOfOrigin:
                    return DicDocumentTypeCodes.StatementNamePlaces;
                case DicProtectionDocType.Codes.SelectiveAchievement:
                    return DicDocumentTypeCodes.StatementSelectiveAchievs;
                case DicProtectionDocType.Codes.IndustrialModel:
                    return DicDocumentTypeCodes.StatementIndustrialDesigns;
                default: return DicDocumentTypeCodes.StatementTrademark;
            }
        }

        public string MapToDepartment(string protectionDocType)
        {
            switch (protectionDocType)
            {
                case DicProtectionDocType.Codes.Trademark:
                case DicProtectionDocType.Codes.PlaceOfOrigin:
                    return DicDepartmentCodes.D_3_1;
                case DicProtectionDocType.Codes.IndustrialModel:
                    return DicDepartmentCodes.D_3_4;
                default:
                    return DicDepartmentCodes.D_2_1;
            }
        }
    }
}