using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Report.Reports.ReportDTO;
using System.Linq;
using Iserv.Niis.Common.Codes;
using System.Collections.Generic;
using System;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Report.ReportBusinessLogic
{
    internal class GetReportDataReceivedRequestReportQuery : BaseReportQuery
    {
        internal override ReportData Execute(ReportConditionData reportFilterData)
        {
            var requestRepository = Uow.GetRepository<Request>();

            var query = requestRepository.AsQueryable().Where(r => r.Addressee != null && r.Addressee.Type != null
                                                                  && r.ProtectionDocType.Code  != DicProtectionDocTypeCodes.ProtectionDocTypeDKCode);

            if (reportFilterData.ProtectionDocTypeIds != null && reportFilterData.ProtectionDocTypeIds.Any())
            {
                query = query.Where(r => reportFilterData.ProtectionDocTypeIds.Contains(r.ProtectionDocTypeId));
            }

            query = query.Where(r => r.DateCreate >= reportFilterData.DateFrom && r.DateCreate <= reportFilterData.DateTo);
            
            var protectionDocTypes = Uow.GetRepository<DicProtectionDocType>().AsQueryable().Select(r => new { r.Code, r.NameRu });

            #region По типам ОД

            var emptyReceivedRequestReportDTO = new List<ReceivedRequestReportDTO>();

            foreach (var protectionDocType in protectionDocTypes.Where(r => r.Code != DicProtectionDocTypeCodes.ProtectionDocTypeDKCode
                                                                            && r.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode))
            {
                emptyReceivedRequestReportDTO.Add(new ReceivedRequestReportDTO { RequestTypeName = protectionDocType.NameRu });

            }

            var selectiveProtectionDocTypeName = protectionDocTypes.First(r => r.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode).NameRu;
            emptyReceivedRequestReportDTO.Add(new ReceivedRequestReportDTO { RequestTypeName = string.Format("{0} (сорта растений)", selectiveProtectionDocTypeName) });
            emptyReceivedRequestReportDTO.Add(new ReceivedRequestReportDTO { RequestTypeName = string.Format("{0} (породы животных)", selectiveProtectionDocTypeName) });

            var residents = query.Where(r => !r.Addressee.IsNotResident
                                            && r.ProtectionDocType.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
                                 .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu, r.ProtectionDocType.Code })
                                 .Select(r => new ReceivedRequestReportDTO
                                 {
                                     RequestTypeName = r.Key.NameRu,
                                     NationalCustomerRequestCount = r.Count(),
                                 }).ToList();

            var nonResidents = query.Where(r => r.Addressee.IsNotResident
                                                && r.ProtectionDocType.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
                                 .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu, r.ProtectionDocType.Code })
                                 .Select(r => new ReceivedRequestReportDTO
                                 {
                                     RequestTypeName = r.Key.NameRu,
                                     NotNationalCustomerRequestCount = r.Count(),
                                 }).ToList();

            #endregion

            #region По селекционным достижениям (породы животных)

            //По селекционным достижениям (породы животных) резиденты
            var residentsCattleBreeding = query.Where(r => !r.Addressee.IsNotResident
                                           && r.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                           && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.AnimalHusbandry)
                                 .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu })
                                 .Select(r => new ReceivedRequestReportDTO
                                 {
                                     RequestTypeName = string.Format("{0} (породы животных)", r.Key.NameRu),
                                     NationalCustomerRequestCount = r.Count(),
                                 }).ToList();
            //По селекционным достижениям (породы животных) НЕ резиденты
            var nonresidentsCattleBreeding = query.Where(r => r.Addressee.IsNotResident
                               && r.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                               && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.AnimalHusbandry)
                     .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu })
                     .Select(r => new ReceivedRequestReportDTO
                     {
                         RequestTypeName = string.Format("{0} (породы животных)", r.Key.NameRu),
                         NationalCustomerRequestCount = r.Count(),
                     }).ToList();

            #endregion

            #region По селекционным достижениям (сорта растений)

            //По селекционным достижениям (сорта растений) резиденты
            var residentsPlantGrowing = query.Where(r => !r.Addressee.IsNotResident
                                           && r.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                           && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.Agricultural)
                                 .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu })
                                 .Select(r => new ReceivedRequestReportDTO
                                 {
                                     RequestTypeName = string.Format("{0} (сорта растений)", r.Key.NameRu),
                                     NationalCustomerRequestCount = r.Count(),
                                 }).ToList();
            //По селекционным достижениям (сорта растений) НЕ резиденты
            var nonResidentsPlantGrowing = query.Where(r => r.Addressee.IsNotResident
                               && r.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                               && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.Agricultural)
                     .GroupBy(r => new { r.ProtectionDocTypeId, r.ProtectionDocType.NameRu })
                     .Select(r => new ReceivedRequestReportDTO
                     {
                         RequestTypeName = string.Format("{0} (сорта растений)", r.Key.NameRu),
                         NationalCustomerRequestCount = r.Count(),
                     }).ToList();

            #endregion

            #region Union Data

            var i = 1;
            var receivedRequestReportDTO = residents.Union(nonResidents)
                .GroupBy(r => r.RequestTypeName)
                .Select(r => new ReceivedRequestReportDTO
                {
                    RequestTypeName = r.Key,
                    NationalCustomerRequestCount = r.Sum(g => g.NationalCustomerRequestCount),
                    NotNationalCustomerRequestCount = r.Sum(g => g.NotNationalCustomerRequestCount),
                    FullReqestCountByType = r.Sum(g => g.NationalCustomerRequestCount) + r.Sum(g => g.NotNationalCustomerRequestCount)
                });

            receivedRequestReportDTO = receivedRequestReportDTO.Union(emptyReceivedRequestReportDTO);

            receivedRequestReportDTO = receivedRequestReportDTO.Union(residentsCattleBreeding.Union(nonresidentsCattleBreeding))
                .GroupBy(r => r.RequestTypeName)
                .Select(r => new ReceivedRequestReportDTO
                {
                    RequestTypeName = r.Key,
                    NationalCustomerRequestCount = r.Sum(g => g.NationalCustomerRequestCount),
                    NotNationalCustomerRequestCount = r.Sum(g => g.NotNationalCustomerRequestCount),
                    FullReqestCountByType = r.Sum(g => g.NationalCustomerRequestCount) + r.Sum(g => g.NotNationalCustomerRequestCount)
                });

            receivedRequestReportDTO = receivedRequestReportDTO.Union(residentsPlantGrowing.Union(nonResidentsPlantGrowing))
                .GroupBy(r => r.RequestTypeName)
                .Select(r => new ReceivedRequestReportDTO
                {
                    RowNumber = i++,
                    RequestTypeName = r.Key,
                    NationalCustomerRequestCount = r.Sum(g => g.NationalCustomerRequestCount),
                    NotNationalCustomerRequestCount = r.Sum(g => g.NotNationalCustomerRequestCount),
                    FullReqestCountByType = r.Sum(g => g.NationalCustomerRequestCount) + r.Sum(g => g.NotNationalCustomerRequestCount)
                });

            #endregion

            var rows = receivedRequestReportDTO.Select(r => r.MapReceivedRequestReport()).ToList();

            var footerRow = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{Value = "" },
                    new Cell{Value = "Всего заявок:", IsBold = true},
                    new Cell{Value = receivedRequestReportDTO.Sum(r => r.NationalCustomerRequestCount)},
                    new Cell{Value = receivedRequestReportDTO.Sum(r => r.NotNationalCustomerRequestCount)},
                    new Cell{Value = receivedRequestReportDTO.Sum(r => r.FullReqestCountByType)}
                }
            };

            rows.Add(footerRow);

            return new ReportData { Rows = rows };
        }
    }
}
