using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Models;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Utils.Constans;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class DocumentsCompare : IDocumentsCompare
    {
        private readonly NiisWebContext _context;
        private readonly IFileConverter _fileConverter;
        private readonly IFileStorage _fileStorage;

        private string _protectionDocTypeCode;

        public DocumentsCompare(
            NiisWebContext context,
            IFileStorage fileStorage,
            IFileConverter fileConverter)
        {
            _context = context;
            _fileStorage = fileStorage;
            _fileConverter = fileConverter;
        }

        public async Task<DocumentsCompareDto> GetDocumentsInfoForCompare(int requestId)
        {
            var protectionDocTypeCode = await GetProtectionDocTypeCode(requestId);
            if (string.IsNullOrEmpty(protectionDocTypeCode) ||
                !GetNecessaryProtectionDocTypeCodes().Contains(protectionDocTypeCode))
            {
                return null;
            }

            _protectionDocTypeCode = protectionDocTypeCode;
            var documentsInfo = new DocumentsCompareDto();
            List<DocumentFileInfo> documents;
            switch (_protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Invention:
                {
                    documents = GetNecessaryDocumentsFileInfo(requestId, DicProtectionDocType.Codes.Invention);
                    break;
                }
                case DicProtectionDocType.Codes.IndustrialModel:
                {
                    documents = GetNecessaryDocumentsFileInfo(requestId, DicProtectionDocType.Codes.IndustrialModel);
                    break;
                }
                case DicProtectionDocType.Codes.UsefulModel:
                {
                    documents = GetNecessaryDocumentsFileInfo(requestId, DicProtectionDocType.Codes.UsefulModel);
                    break;
                }
                default:
                    return null;
            }

            if (documents == null)
            {
                return null;
            }

            await InitializeDescription(documents, documentsInfo);
            await InitializeEssay(documents, documentsInfo);
            await InitializeFormula(documents, documentsInfo);
            return documentsInfo;
        }

        #region Private Methods

        private async Task InitializeDescription(List<DocumentFileInfo> documents,
            DocumentsCompareDto documentsCompareDto)
        {
            switch (_protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Invention:
                    documentsCompareDto.Description =
                        await GetOriginalTextFromDocumentFile(documents, DicDocumentTypeCodes.InventionDescription);

                    var inventionInfo = await GetChangedTextFromDocumentFile(documents,
                        DicDocumentTypeCodes.ChangedInventionDescription);
                    documentsCompareDto.ChangedDescription = inventionInfo.text;
                    documentsCompareDto.ChangedDescriptionDocId = inventionInfo.documentId;
                    break;
                case DicProtectionDocType.Codes.UsefulModel:
                    // TODO на данный момент нет подходящих типов документа
                    break;
                case DicProtectionDocType.Codes.IndustrialModel:
                    // TODO на данный момент нет подходящих типов документа
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task InitializeEssay(List<DocumentFileInfo> documents,
            DocumentsCompareDto documentsCompareDto)
        {
            documentsCompareDto.Essay = await GetOriginalTextFromDocumentFile(documents, DicDocumentTypeCodes.Essay,
                DicDocumentTypeCodes.EssayForeign);

            var essayInfo = await GetChangedTextFromDocumentFile(documents, DicDocumentTypeCodes.ChangedEssay);
            documentsCompareDto.ChangedEssay = essayInfo.text;
            documentsCompareDto.ChangedEssayDocId = essayInfo.documentId;
        }

        private async Task InitializeFormula(List<DocumentFileInfo> documents,
            DocumentsCompareDto documentsCompareDto)
        {
            switch (_protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Invention:
                    documentsCompareDto.Formula =
                        await GetOriginalTextFromDocumentFile(documents, DicDocumentTypeCodes.FormulaInvention);

                    var inventionInfo =
                        await GetChangedTextFromDocumentFile(documents, DicDocumentTypeCodes.ChangedFormulaInvention);
                    documentsCompareDto.ChangedFormula = inventionInfo.text;
                    documentsCompareDto.ChangedFormulaDocId = inventionInfo.documentId;

                    break;
                case DicProtectionDocType.Codes.UsefulModel:
                    // TODO на данный момент нет подходящих типов документа
                    break;
                case DicProtectionDocType.Codes.IndustrialModel:
                    // TODO на данный момент нет подходящих типов документа
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<string> GetOriginalTextFromDocumentFile(List<DocumentFileInfo> documents,
            params string[] docTypeCodes)
        {
            var documentFileInfo =
                GetDocumentFileInfoWhereTypeDocxOrPdf(documents, x => docTypeCodes.Contains(x.TypeCode));
            if (documentFileInfo == null)
            {
                return string.Empty;
            }

            return await GetTextOfFile(documentFileInfo.BucketName,
                documentFileInfo.OriginalName, documentFileInfo.ContentType);
        }

        private async Task<(string text, int documentId)> GetChangedTextFromDocumentFile(
            List<DocumentFileInfo> documents,
            params string[] docTypeCodes)
        {
            DocumentFileInfo documentFileInfo;
            var finishedDocument =
                GetDocumentFileInfoWhereTypeDocxOrPdf(documents,
                    x => docTypeCodes.Contains(x.TypeCode) && x.IsFinished == true);

            if (finishedDocument != null)
            {
                documentFileInfo = GetDocumentFileInfoWhereTypeDocxOrPdf(documents, x =>
                    docTypeCodes.Contains(x.TypeCode) &&
                    x.DateUpdate > finishedDocument.DateUpdate);
            }
            else
            {
                documentFileInfo =
                    GetDocumentFileInfoWhereTypeDocxOrPdf(documents, x => docTypeCodes.Contains(x.TypeCode));
            }

            if (documentFileInfo == null)
            {
                return (string.Empty, default(int));
            }

            return (await GetTextOfFile(documentFileInfo.BucketName,
                documentFileInfo.OriginalName, documentFileInfo.ContentType), documentFileInfo.Id);
        }

        private DocumentFileInfo GetDocumentFileInfoWhereTypeDocxOrPdf(List<DocumentFileInfo> documents,
            Func<DocumentFileInfo, bool> predicate)
        {
            return documents
                       .Where(predicate)
                       .Where(x => ContentType.Docx.Equals(x.ContentType))
                       .OrderByDescending(x => x.DateUpdate)
                       .FirstOrDefault() ?? documents
                       .Where(predicate)
                       .Where(x => ContentType.Pdf.Equals(x.ContentType))
                       .OrderByDescending(x => x.DateUpdate)
                       .FirstOrDefault();
        }

        private async Task<string> GetTextOfFile(string bucketName, string fileName, string fileType)
        {
            try
            {
                var file = await _fileStorage.GetAsync(bucketName, fileName);
                return file == null ? string.Empty : _fileConverter.GetTextOfFile(file, fileType);
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<string> GetProtectionDocTypeCode(int requestId)
        {
            return await _context.Requests
                .Where(x => x.Id == requestId)
                .Select(x => x.ProtectionDocType.Code)
                .SingleOrDefaultAsync();
        }

        private List<DocumentFileInfo> GetNecessaryDocumentsFileInfo(int requestId, string protectionDocTypeCode)
        {
            if (string.IsNullOrEmpty(protectionDocTypeCode))
            {
                throw new ArgumentNullException(nameof(protectionDocTypeCode));
            }

            try
            {
                var result = _context.Requests
                    .Where(x => x.Id == requestId && protectionDocTypeCode.Equals(x.ProtectionDocType.Code))
                    .SelectMany(x => x.Documents)
                    .Select(x => x.Document)
                    .Include(x => x.MainAttachment)
                    .Include(d => d.Type)
                    .Where(x => GetNecessaryDocumentTypeCodes().Contains(x.Type.Code))
                    .Select(x => new DocumentFileInfo
                    {
                        Id = x.Id,
                        TypeCode = x.Type.Code,
                        BucketName = x.MainAttachment.BucketName,
                        OriginalName = x.MainAttachment.OriginalName,
                        ContentType = x.MainAttachment.ContentType,
                        DateUpdate = x.DateUpdate,
                        IsFinished = x.IsFinished
                    }).ToList();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///     Метод возвращает типы документов для сравнения
        /// </summary>
        /// <returns></returns>
        private List<string> GetNecessaryDocumentTypeCodes()
        {
            // по мере необходимости добавлять сюда новые типы
            return new List<string>
            {
                DicDocumentTypeCodes.Essay,
                DicDocumentTypeCodes.EssayForeign,
                DicDocumentTypeCodes.ChangedEssay,
                DicDocumentTypeCodes.InventionDescription,
                DicDocumentTypeCodes.ChangedInventionDescription,
                DicDocumentTypeCodes.FormulaInvention,
                DicDocumentTypeCodes.ChangedFormulaInvention
            };
        }

        /// <summary>
        ///     Метод возвращает типы патентов где есть необходимость сравнения документов
        /// </summary>
        /// <returns></returns>
        private List<string> GetNecessaryProtectionDocTypeCodes()
        {
            return new List<string>
            {
                // TODO раскомментировать когда добавят подходящие типы документов
                DicProtectionDocType.Codes.Invention
                // DicProtectionDocType.Codes.UsefulModel,
                // DicProtectionDocType.Codes.IndustrialModel
            };
        }

        #endregion
    }
}