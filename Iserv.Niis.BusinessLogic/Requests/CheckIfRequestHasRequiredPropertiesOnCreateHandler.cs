using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class CheckIfRequestHasRequiredPropertiesOnCreateHandler : BaseHandler
    {
        public async Task<bool> ExecuteAsync(int requestId)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            bool returnValue = false;
            switch (request.ProtectionDocType.Code)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    returnValue = request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementTrademark) &&
                       request.Documents.Any(rd => new[] { DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032 }.Contains(rd.Document.Type.Code)) &&
                       request.Documents.Any(rd => new[] { DicDocumentTypeCodes._001_001_1A, DicDocumentTypeCodes.Image }.Contains(rd.Document.Type.Code));
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    returnValue = request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementInventions) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.InventionDescription) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.Essay) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.FormulaInvention) &&
                        (request.NameRu != null || request.NameKz != null || request.NameEn != null) &&
                        request.AddresseeAddress != null;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    returnValue = request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementIndustrialDesigns) &&
                    request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.IndustrialModelDescription) &&
                    //request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SetOfImagesOfTheProductOrDrawingAndOtherMaterials);
                    request.Documents.Any(rd => new[] { DicDocumentTypeCodes._001_001_1A, DicDocumentTypeCodes.Image, DicDocumentTypeCodes.SetOfImagesOfTheProductOrDrawingAndOtherMaterials }.Contains(rd.Document.Type.Code));
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    returnValue = request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementSelectiveAchievs)
                        /* &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SAQuestionnaire) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.AttributesTable) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SANegativesOrColorSlides) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.SAInformationAboutPreviouslyMadeSale) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes._001_032
                        )*/;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    returnValue = request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.StatementUsefulModels) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.UsefulModelDescription) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.FormulaUsefulModel) &&
                        request.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.Essay);
                    break;
                default:
                    returnValue = false;
                    break;
            }

            return returnValue;
        }
    }
}