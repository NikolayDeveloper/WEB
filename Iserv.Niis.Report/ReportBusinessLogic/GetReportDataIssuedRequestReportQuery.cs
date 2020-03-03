using Iserv.Niis.Domain.Entities.ProtectionDoc;
using System.Linq;
using System;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Report.Reports.ReportDTO;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Report.ReportBusinessLogic
{
    internal class GetReportDataIssuedProtectionDocumentsReportQuery : BaseReportQuery
    {
        internal override ReportData Execute(ReportConditionData reportFilterData)
        {
            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();

            var query = protectionDocRepository.AsQueryable().Where(r => r.Type != null
                                                                         && r.Request != null && r.Request.Addressee != null && r.Request.Addressee.Type != null
                                                                         && string.IsNullOrEmpty(r.GosNumber) == false
                                                                         && string.IsNullOrEmpty(r.Bulletins.Any() ? r.Bulletins.FirstOrDefault().Bulletin.Number : string.Empty) == false
                                                                         && r.Bulletins.Any() && r.Bulletins.FirstOrDefault().Bulletin.PublishDate.HasValue
                                                            );

            if (reportFilterData.ProtectionDocTypeIds != null && reportFilterData.ProtectionDocTypeIds.Any())
            {
                query = query.Where(r => reportFilterData.ProtectionDocTypeIds.Contains(r.TypeId));
            }

            query = query.Where(r => r.Bulletins.Any() && (r.Bulletins.FirstOrDefault().Bulletin.PublishDate >= reportFilterData.DateFrom && r.Bulletins.FirstOrDefault().Bulletin.PublishDate <= reportFilterData.DateTo));

            var protectionDocTypes = Uow.GetRepository<DicProtectionDocType>().AsQueryable().Select(r => new { r.Code, r.NameRu });

            #region local Filters

            IQueryable<ProtectionDoc> residentFilter(IQueryable<ProtectionDoc> protectionDoc)
            {
                return protectionDoc.Where(r => r.Request.Addressee.IsNotResident);
            }

            IQueryable<ProtectionDoc> nonResidentFilter(IQueryable<ProtectionDoc> protectionDoc)
            {
                return protectionDoc.Where(r => !r.Request.Addressee.IsNotResident);
            }

            var patentCodes = new string[] {
                DicProtectionDocTypeCodes.RequestTypeInventionCode,
                DicProtectionDocTypeCodes.RequestTypeUsefulModelCode,
                DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode,
                DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode,
            };

            var certificateCodes = new string[] {
                DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode,
                DicProtectionDocTypeCodes.RequestTypeTrademarkCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode,
            };

            var patentDocuments = query.Where(r => patentCodes.Contains(r.Type.Code));
            var certificateDocuments = query.Where(r => certificateCodes.Contains(r.Type.Code));

            var residentPatentDocuments = residentFilter(patentDocuments);
            var nonResidentPatentDocuments = nonResidentFilter(patentDocuments);

            var residentCertificateDocuments = residentFilter(certificateDocuments);
            var nonResidentCertificateDocuments = nonResidentFilter(certificateDocuments);

            #endregion


            #region Кол-во выданных охранных документов (патентов)

            var emptyIssuedProtectionDocumentReportDTO = new List<IssuedProtectionDocumentReportDTO>();

            foreach (var protectionDocType in protectionDocTypes.Where(r => patentCodes.Contains(r.Code)
                                                                            && r.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode))
            {
                emptyIssuedProtectionDocumentReportDTO.Add(new IssuedProtectionDocumentReportDTO { ProtectionDocumentTypeName = protectionDocType.NameRu });

            }

            var selectiveProtectionDocTypeName = protectionDocTypes.First(r => r.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode).NameRu;
            emptyIssuedProtectionDocumentReportDTO.Add(new IssuedProtectionDocumentReportDTO { ProtectionDocumentTypeName = string.Format("{0} (сорта растений)", selectiveProtectionDocTypeName) });
            emptyIssuedProtectionDocumentReportDTO.Add(new IssuedProtectionDocumentReportDTO { ProtectionDocumentTypeName = string.Format("{0} (породы животных)", selectiveProtectionDocTypeName) });


            var simpleResidentPatentDocuments = residentPatentDocuments.Where(r => r.Type.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NationalCustomerCount = r.Count(),
                                                                       }).ToList();
            var simpleNonResidentPatentDocuments = nonResidentPatentDocuments.Where(r => r.Type.Code != DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NotNationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var simplePatentDocuments = simpleResidentPatentDocuments.Union(simpleNonResidentPatentDocuments)
                    .GroupBy(r => r.ProtectionDocumentTypeName)
                    .Select(r => new IssuedProtectionDocumentReportDTO
                    {
                        ProtectionDocumentTypeName = r.Key,
                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                    }).ToList();

            #region По селекционным достижениям (породы животных)


            var residentSAPlantGrowingPatentDocuments = residentPatentDocuments.Where(r => r.Type.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                                                                                        && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.AnimalHusbandry)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = string.Format("{0} (породы животных)", r.Key.NameRu),
                                                                           NationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var nonRresidentSAPlantGrowingPatentDocuments = nonResidentPatentDocuments.Where(r => r.Type.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                                                                                        && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.AnimalHusbandry)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = string.Format("{0} (породы животных)", r.Key.NameRu),
                                                                           NotNationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var SAPlantGrowingPatentDocuments = residentSAPlantGrowingPatentDocuments.Union(nonRresidentSAPlantGrowingPatentDocuments)
                    .GroupBy(r => r.ProtectionDocumentTypeName)
                    .Select(r => new IssuedProtectionDocumentReportDTO
                    {
                        ProtectionDocumentTypeName = r.Key,
                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                    }).ToList();

            #endregion

            #region По селекционным достижениям (сорта растений)

            var residentSACattleBreedingPatentDocuments = residentPatentDocuments.Where(r => r.Type.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                                                                                        && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.Agricultural)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = string.Format("{0} (сорта растений)", r.Key.NameRu),
                                                                           NationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var nonResidentSACattleBreedingPatentDocuments = nonResidentPatentDocuments.Where(r => r.Type.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode
                                                                                                        && r.SelectionAchieveType != null && r.SelectionAchieveType.Code == DicSelectionAchieveTypeCodes.Agricultural)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = string.Format("{0} (сорта растений)", r.Key.NameRu),
                                                                           NotNationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var SACattleBreedingPatentDocuments = residentSACattleBreedingPatentDocuments.Union(nonResidentSACattleBreedingPatentDocuments)
                    .GroupBy(r => r.ProtectionDocumentTypeName)
                    .Select(r => new IssuedProtectionDocumentReportDTO
                    {
                        ProtectionDocumentTypeName = r.Key,
                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                    }).ToList();


            #endregion

            var grouppedPatentDocuments = simplePatentDocuments
                                    .Union(SACattleBreedingPatentDocuments)
                                    .Union(SAPlantGrowingPatentDocuments)
                                    .Union(emptyIssuedProtectionDocumentReportDTO)
                                    .GroupBy(r => r.ProtectionDocumentTypeName)
                                    .Select(r => new IssuedProtectionDocumentReportDTO
                                    {
                                        IsPatent = true,
                                        ProtectionDocumentTypeName = r.Key,
                                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                                    }).ToList();


            #endregion

            #region Кол-во выданных охранных документов (свидетельств, выписок)

            var emptyIssuedProtectionDocumentReportDTOCertificate = new List<IssuedProtectionDocumentReportDTO>();

            foreach (var protectionDocType in protectionDocTypes.Where(r => certificateCodes.Contains(r.Code) && r.Code != DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode))
            {
                emptyIssuedProtectionDocumentReportDTOCertificate.Add(new IssuedProtectionDocumentReportDTO { ProtectionDocumentTypeName = protectionDocType.NameRu });
            }

            var simpleResidentCertificatesDocuments = residentCertificateDocuments.Where(r => r.SubType != null && r.SubType.Code == DicProtectionDocSubtypeCodes.WellKnownTradeMark)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var simpleNonResidentCertificatesDocuments = nonResidentCertificateDocuments.Where(r => r.Type.Code != DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode
                                                                                                && r.SubType != null && r.SubType.Code == DicProtectionDocSubtypeCodes.WellKnownTradeMark)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NotNationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var grouppedCertificatesDocuments = simpleResidentCertificatesDocuments
                                                .Union(simpleNonResidentCertificatesDocuments)
                                                .Union(emptyIssuedProtectionDocumentReportDTOCertificate)
                    .GroupBy(r => r.ProtectionDocumentTypeName)
                    .Select(r => new IssuedProtectionDocumentReportDTO
                    {
                        IsCertificate = true,
                        ProtectionDocumentTypeName = r.Key,
                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                    }).ToList();

            #region Международыне товарные знаки

            var grouppedInternationalTrademarks = query.Where(r => r.Type.Code == DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode)
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           FullReqestCountByType = r.Count()
                                                                       }).FirstOrDefault();

            #endregion

            #endregion

            #region Общеизвестные товарные знаки (ТЗ)

            var FTMResidentCertificatesDocuments = residentCertificateDocuments.Where(r => r.SubType == null
                                                                            || (r.SubType != null && r.SubType.Code != DicProtectionDocSubtypeCodes.WellKnownTradeMark))
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var FTMNonResidentCertificatesDocuments = nonResidentCertificateDocuments.Where(r => r.SubType == null
                                                                            || (r.SubType != null && r.SubType.Code != DicProtectionDocSubtypeCodes.WellKnownTradeMark))
                                                                       .GroupBy(r => new { r.TypeId, r.Type.NameRu })
                                                                       .Select(r => new IssuedProtectionDocumentReportDTO
                                                                       {
                                                                           ProtectionDocumentTypeName = r.Key.NameRu,
                                                                           NotNationalCustomerCount = r.Count(),
                                                                       }).ToList();

            var grouppedFTMCertificatesDocuments = FTMResidentCertificatesDocuments.Union(FTMNonResidentCertificatesDocuments)
                    .GroupBy(r => r.ProtectionDocumentTypeName)
                    .Select(r => new IssuedProtectionDocumentReportDTO
                    {
                        IsCertificate = true,
                        ProtectionDocumentTypeName = "Общеизвестные товарные знаки",
                        NationalCustomerCount = r.Sum(g => g.NationalCustomerCount),
                        NotNationalCustomerCount = r.Sum(g => g.NotNationalCustomerCount),
                        FullReqestCountByType = r.Sum(g => g.NationalCustomerCount) + r.Sum(g => g.NotNationalCustomerCount)
                    }).FirstOrDefault();


            #endregion


            var rows = new List<Row>();

            var rowNumber = 1;
            //Патенты
            foreach (var grouppedPatentDocument in grouppedPatentDocuments)
            {
                var row = new Row
                {
                    Cells = new List<Cell>
                    {
                        new Cell { Value = rowNumber++ },
                        new Cell { Value = grouppedPatentDocument.ProtectionDocumentTypeName },
                        new Cell { Value = grouppedPatentDocument.NationalCustomerCount },
                        new Cell { Value = grouppedPatentDocument.NotNationalCustomerCount },
                    }
                };

                if (rowNumber == 2)
                {
                    row.Cells.Add(new Cell { Value = " ", CollSpan = 2, RowSpan = 5, IsTextAlignCenter = true });

                }
                else
                {
                    row.Cells.Add(new Cell());
                    row.Cells.Add(new Cell());
                }

                row.Cells.Add(new Cell { Value = grouppedPatentDocument.FullReqestCountByType });

                rows.Add(row);
            }

            //Свидетельства, выписоки
            foreach (var grouppedCertificatesDocument in grouppedCertificatesDocuments)
            {
                var row = new Row
                {
                    Cells = new List<Cell>
                    {
                        new Cell { Value = rowNumber++ },
                        new Cell { Value = grouppedCertificatesDocument.ProtectionDocumentTypeName }
                    }
                };

                if (rowNumber == 7)
                {
                    row.Cells.Add(new Cell { Value = " ", CollSpan = 2, RowSpan = 3, IsTextAlignCenter = true });

                }
                else
                {
                    row.Cells.Add(new Cell());
                    row.Cells.Add(new Cell());
                }


                row.Cells.Add(new Cell { Value = grouppedCertificatesDocument.NationalCustomerCount });
                row.Cells.Add(new Cell { Value = grouppedCertificatesDocument.NotNationalCustomerCount });
                row.Cells.Add(new Cell { Value = grouppedCertificatesDocument.FullReqestCountByType });

                rows.Add(row);
            }

            //Международные Товарные Знаки
            if (grouppedInternationalTrademarks == null)
            {
                grouppedInternationalTrademarks = new IssuedProtectionDocumentReportDTO
                {
                    ProtectionDocumentTypeName = protectionDocTypes.FirstOrDefault(r => r.Code == DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode).NameRu
                };
            }

            rows.Add(new Row
            {
                Cells = new List<Cell>
                    {
                        new Cell { Value = rowNumber++ },
                        new Cell { Value = grouppedInternationalTrademarks.ProtectionDocumentTypeName },
                        new Cell { },
                        new Cell { },
                        new Cell { Value = grouppedInternationalTrademarks.FullReqestCountByType, CollSpan = 2, IsTextAlignCenter = true  },
                        new Cell { Value = grouppedInternationalTrademarks.FullReqestCountByType},
                    }
            });

            //Общее количество выданных охранных документов на объекты промышленной собственности	11510
            rows.Add(new Row
            {
                Cells = new List<Cell>
                    {
                        new Cell { Value = "Общее количество выданных охранных документов на объекты промышленной собственности", CollSpan = 6, IsBold = true },
                        new Cell { Value = rows.Sum(r=> r.Cells[r.Cells.Count-1].Value) },
                    }
            });


            //Общеизвестные товарные знаки
            if (grouppedFTMCertificatesDocuments == null)
            {
                grouppedFTMCertificatesDocuments = new IssuedProtectionDocumentReportDTO
                {
                    ProtectionDocumentTypeName = "Общеизвестные товарные знаки"
                };
            }

            rows.Add(new Row
            {
                Cells = new List<Cell>
                    {
                        new Cell { Value = rowNumber++ },
                        new Cell { Value = grouppedFTMCertificatesDocuments.ProtectionDocumentTypeName },
                        new Cell { Value = " ", CollSpan = 2, IsTextAlignCenter = true },
                        new Cell { Value = grouppedFTMCertificatesDocuments.NationalCustomerCount},
                        new Cell { Value = grouppedFTMCertificatesDocuments.NotNationalCustomerCount},
                        new Cell { Value = grouppedFTMCertificatesDocuments.FullReqestCountByType},
                    }
            });

            return new ReportData { Rows = rows };
        }
    }
}
