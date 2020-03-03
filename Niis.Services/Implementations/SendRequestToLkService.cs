using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Intergrations;
using Iserv.Niis.Domain.Intergrations.RequisitionSend;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockClassifications;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockColors;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockCustomers;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockEarlyRegs;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockFiles;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockMessageFiles;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.RequisitionFiles;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Helpers;

namespace Iserv.Niis.Services.Implementations
{
    /// <summary>
    /// Сервис отправки заявки в ЛК
    /// </summary>
    public class SendRequestToLkService : BaseService, ISendRequestToLkService
    {
        private readonly ILkIntergarionHelper _lkIntergarionHelper;
        private readonly IFileStorage _fileStorage;
        
        public SendRequestToLkService(
            ILkIntergarionHelper lkIntergarionHelper, 
            IFileStorage fileStorage)
        {
            _lkIntergarionHelper = lkIntergarionHelper;
            _fileStorage = fileStorage;
        }

        #region Codes

        private static readonly string[] DocTypeCodes = {
            DicDocumentTypeCodes.StatementTrademark,
            DicDocumentTypeCodes.StatementNameOfOrigin,
            DicDocumentTypeCodes.StatementInventions,
            DicDocumentTypeCodes.StatementSelectiveAchievs,
            DicDocumentTypeCodes.StatementIndustrialDesigns,
            DicDocumentTypeCodes.StatementUsefulModels
        };

        private readonly string[] _codes = {
            DicProtectionDocTypeCodes.RequestTypeTrademarkCode,
            DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode,
        };

        private static readonly string[] Payments = {
            DicDocumentTypeCodes._001_002,
            DicDocumentTypeCodes.IN001_032,
        };

        private static readonly string[] PoImages =
        {
            DicDocumentTypeCodes._001_001_1A,
            DicDocumentTypeCodes.Image,
            DicDocumentTypeCodes.SetOfImagesOfTheProductOrDrawingAndOtherMaterials
        };

        private static readonly string[] Images = {
            DicDocumentTypeCodes._001_001_1A,
            DicDocumentTypeCodes.Image,
        };

        private readonly IEnumerable<string> _allCodes = PoImages.Union(Images).Union(Payments);

        #endregion

        /// <summary>
        /// Отправка заявки в ЛК
        /// </summary>
        /// <param name="requestId">Идентефикатор заявки</param>
        /// <returns></returns>
        public async Task<ServerStatus> Send(int requestId)
        {
            var request = await Executor.GetQuery<GetRequestByIdFromExportQuery>().Process(r => r.ExecuteAsync(requestId));

            var status = ValidateRequest(request);
            if (!string.IsNullOrEmpty(status.Message))
                return status;

            try
            {
                var argument = await FillRequestArgument(request);

                var body = new RequisitionSendBody
                {
                    Input = new RequisitionSend
                    {
                        Argument = argument
                    }
                };

                var result = _lkIntergarionHelper.CallWebService(body, SoapActions.RequisitionSend, "req");

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region FillData

        private async Task<Argument> FillRequestArgument(Request request)
        {
            var argument = new Argument();

            FillDocInfo(argument, request);
            
            FillBreeding(request, argument);

            FillProduct(request, argument);

            FillTrademark(request, argument);

            FillCustomer(request, argument);

            FillCorrespondence(request, argument);

            FillData(request, argument);

            await FillRequisition(request, argument);

            FillBlockColor(request, argument);

            FillBlockClassification(request, argument);

            FillBlockEarlyReg(request, argument);

            FillBlockCustomer(request, argument);

            await FillBlockFile(request, argument);

            await BlockMessageFile(request, argument);

            return argument;
        }

        private async Task BlockMessageFile(Request request, Argument argument)
        {
            var documents = request.Documents.Where(d =>
                !_allCodes.Contains(d.Document.Type.Code) && !string.IsNullOrEmpty(d.Document.OutgoingNumber) && d.Document.AdditionalAttachments.Any());
            var attachedFile = new List<AttachedMessageFile>();

            foreach (var requestDocument in documents)
            {
                BlockMessageFileAttachedMessageFileFile fileFile = null;
                if (requestDocument.Document.AdditionalAttachments.Any())
                {
                    var att = requestDocument.Document.AdditionalAttachments.First();

                    var file = Convert.ToBase64String(await _fileStorage.GetAsync(
                        att.BucketName,
                        att.OriginalName));
                    fileFile = new BlockMessageFileAttachedMessageFileFile
                    {
                        Name = att.ValidName,
                        Content = file
                    };
                }

                var attach = new AttachedMessageFile
                {
                    File = fileFile,
                    MessageInfo = new BlockMessageFileAttachedMessageFileMessageInfo
                    {
                        Id = requestDocument.Document.Id.ToString(),
                        DocumentID = requestDocument.Document.Barcode.ToString(),
                        DocDate = requestDocument.Document.DateCreate.Date.ToString(CultureInfo.InvariantCulture),
                        DocNumber = requestDocument.Document.OutgoingNumber,
                        Direction = "from_niip"
                    }
                };
                attachedFile.Add(attach);
            }

            argument.BlockMessageFile = new BlockMessageFile();
            argument.BlockMessageFile.AddRange(attachedFile);
        }

        private async Task FillBlockFile(Request request, Argument argument)
        {
            var documents = request.Documents.Where(d => _allCodes.Contains(d.Document.Type.Code) && d.Document.AdditionalAttachments.Any());
            var attachedFile = new List<AttachedFile>();

            foreach (var requestDocument in documents)
            {
                BlockFileAttachedFileFile fileFile = null;
                if (requestDocument.Document.AdditionalAttachments.Any())
                {
                    var att = requestDocument.Document.AdditionalAttachments.First();

                    var file = Convert.ToBase64String(await _fileStorage.GetAsync(
                        att.BucketName,
                        att.OriginalName));
                    fileFile = new BlockFileAttachedFileFile
                    {
                        Name = att.ValidName,
                        Content = file,
                        ShepFile = null
                    };
                }

                var attach = new AttachedFile
                {
                    Type = new BlockFileAttachedFileType
                    {
                        UID = (requestDocument.Document.Type.ExternalId ?? requestDocument.Document.Type.Id).ToString(),
                        Note = requestDocument.Document.Type.NameRu
                    },
                    PageCount = requestDocument.Document.PageCount.ToString(),
                    File = fileFile
                };
                attachedFile.Add(attach);
            }

            argument.BlockFile = new BlockFile();
            argument.BlockFile.AddRange(attachedFile);
        }

        private static void FillBlockCustomer(Request request, Argument argument)
        {
            var customers = new List<Customer>();

            foreach (var d in request.RequestCustomers)
            {
                var customer = new Customer
                {
                    NameRu = d.Customer.NameRu,
                    NameKz = d.Customer.NameKz,
                    ShortDocContent = d.Customer.ShortDocContent,
                    NotaryName = d.Customer.NotaryName,
                    PowerAttorneyDateIssue = d.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney
                        ? d.Customer.CustomerAttorneyInfos.Select(i => i.CertDate).FirstOrDefault()
                            .ToString(CultureInfo.InvariantCulture)
                        : string.Empty,
                    PowerAttorneyFullNum = d.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney
                        ? d.Customer.CustomerAttorneyInfos.Select(i => i.CertNum).FirstOrDefault()
                        : string.Empty,
                    ApplicantsInfo = d.Customer.ApplicantsInfo,
                    AttorneyCertDate = d.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney
                        ? d.Customer.CustomerAttorneyInfos.Select(i => i.CertDate).FirstOrDefault()
                            .ToString(CultureInfo.InvariantCulture)
                        : string.Empty,
                    IndustrialProperty = string.Empty,
                    AttorneyCertNum = d.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney
                        ? d.Customer.CustomerAttorneyInfos.Select(i => i.CertDate).FirstOrDefault()
                            .ToString(CultureInfo.InvariantCulture)
                        : string.Empty,
                    PatentLinkType = (d.CustomerRole.ExternalId ?? d.CustomerRole.Id).ToString(),
                    Mention = string.Empty,
                    Type = new BlockCustomerCustomerType
                    {
                        UID = (d.Customer.Type.ExternalId ?? d.Customer.Type.Id).ToString(),
                        Note = d.Customer.Type.NameRu
                    },
                    Xin = d.Customer.Xin,
                    Opf = d.Customer.Opf,
                    NameEn = d.Customer.NameEn,
                    CertificateNumber = d.Customer.CertificateNumber,
                    CertificateSeries = d.Customer.CertificateSeries,
                    RegDate = d.Customer.RegDate?.DateTime,
                    AdrCountry = (d.Customer.Country?.ExternalId ?? d.Customer.Country?.Id).ToString(),
                    AdrObl = d.Customer.Oblast,
                    AdrStreet = d.Customer.Street,
                    AdrPostCode = string.Empty,
                    Country = (d.Customer.Country?.ExternalId ?? d.Customer.Country?.Id).ToString(),
                    Email = string.Join(", ",
                        d.Customer.ContactInfos.Where(c => c.Type.Code == DicContactInfoType.Codes.Email)
                            .Select(c => c.Info)),
                    Fax = string.Join(", ",
                        d.Customer.ContactInfos.Where(c => c.Type.Code == DicContactInfoType.Codes.Fax)
                            .Select(c => c.Info)),
                    Phone = string.Join(", ",
                        d.Customer.ContactInfos.Where(c =>
                            c.Type.Code == DicContactInfoType.Codes.MobilePhone ||
                            c.Type.Code == DicContactInfoType.Codes.Phone).Select(c => c.Info))
                };
                customers.Add(customer);
            }

            argument.BlockCustomer = new BlockCustomer();
            argument.BlockCustomer.AddRange(customers);
        }

        private static void FillBlockEarlyReg(Request request, Argument argument)
        {
            var rarlyReg = request.EarlyRegs.Select(d => new EarlyReg
            {
                Country = new BlockEarlyRegEarlyRegCountry
                {
                    UID = (d.RegCountry?.ExternalId ?? d.RegCountry?.Id).ToString(),
                    Note = d.RegCountry?.NameRu
                },
                Date = d.RegDate.GetValueOrDefault().DateTime.ToString(CultureInfo.InvariantCulture),
                Name = d.NameSD,
                Num = d.RegNumber,
                StageReview = d.StageSD,
                EarlyTypeId = new BlockEarlyRegEarlyRegEarlyTypeId
                {
                    UID = (d.EarlyRegType?.ExternalId ?? d.EarlyRegType?.Id).ToString(),
                    Note = d.EarlyRegType?.NameKz
                }
            }).ToList();

            argument.BlockEarlyReg = new BlockEarlyReg();
            argument.BlockEarlyReg.AddRange(rarlyReg);
        }

        private static void FillBlockClassification(Request request, Argument argument)
        {
            var items = request.ICGSRequests.Select(d => new RefKey
            {
                UID = (d.Icgs.ExternalId ?? d.Icgs.Id).ToString(),
                Note = d.Icgs.NameRu
            }).ToList();

            argument.BlockClassification = new BlockClassification();
            argument.BlockClassification.AddRange(items);
        }

        private static void FillBlockColor(Request request, Argument argument)
        {
            var colorItems = request.ColorTzs.Select(d => new RefKey
            {
                UID = (d.ColorTz.ExternalId ?? d.ColorTz.Id).ToString(),
                Note = d.ColorTz.NameRu
            }).ToList();

            argument.BlockColor = new BlockColor();
            argument.BlockColor.AddRange(colorItems);
        }

        private static void FillData(Request request, Argument argument)
        {
            argument.Genus = request.RequestInfo?.ProductPlace;
            argument.GenusEn = request.RequestInfo?.Genus;

            argument.PatentType = new PatentType
            {
                UID = (request.ProtectionDocType.ExternalId ?? request.ProtectionDocType.Id).ToString(),
                Note = request.ProtectionDocType.NameRu
            };

            argument.Stage = request.CurrentWorkflow.CurrentStage.NameRu;

            argument.RequirementsLaw = false;
            argument.EarlyRegNum = string.Empty;

            argument.LawTTWp2s10 = false;
            argument.AssignmentTPT = false;
            argument.AssignmentAuthorTAT = false;
            argument.HeirshipTN = false;

            argument.NameRu = request.NameRu;
            argument.NameEn = request.NameEn;
            argument.NameKz = request.NameKz;
            argument.PageCount = request.PageCount?.ToString();
            argument.CopyCount = request.CopyCount?.ToString();

            argument.Pay = null;
        }

        private async Task FillRequisition(Request request, Argument argument)
        {
            var doc = request.Documents.FirstOrDefault(d => DocTypeCodes.Contains(d.Document.Type.Code));
            if (doc?.Document.AdditionalAttachments.Count > 0)
            {
                var attach = doc.Document.AdditionalAttachments.First();
                var file = await _fileStorage.GetAsync(attach.BucketName, attach.OriginalName);
                argument.RequisitionFile = new RequisitionFile
                {
                    Name = attach.ValidName,
                    Content = Convert.ToBase64String(file, Base64FormattingOptions.InsertLineBreaks),
                    ShepFile = null
                };
            }
        }

        private static void FillCorrespondence(Request request, Argument argument)
        {
            var correspondence = request.RequestCustomers.FirstOrDefault(d => d.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

            argument.AdrCustomerName = correspondence?.Customer.NameRu ?? correspondence?.Customer.NameKz ?? correspondence?.Customer.NameEn;
            argument.AdrCountry = new AdrCountry
            {
                UID = (correspondence?.Customer.Country?.ExternalId ?? correspondence?.Customer.Country?.Id).ToString(),
                Note = correspondence?.Customer.Country?.NameRu
            };
            argument.AdrPostCode = string.Empty;
            argument.AdrStreet = correspondence?.Customer.Street;
            argument.AdrRegion = correspondence?.Customer.Region;
            argument.AdrPhone = correspondence != null
                ? string.Join(", ",
                    correspondence.Customer.ContactInfos.Where(d =>
                            d.Type.Code == DicContactInfoType.Codes.Phone ||
                            d.Type.Code == DicContactInfoType.Codes.MobilePhone)
                        .Select(d => d.Info))
                : string.Empty;
            argument.AdrFax = correspondence != null
                ? string.Join(", ",
                    correspondence.Customer.ContactInfos.Where(d => d.Type.Code == DicContactInfoType.Codes.Fax)
                        .Select(d => d.Info))
                : string.Empty;
            argument.AdrEmail = correspondence != null
                ? string.Join(", ",
                    correspondence.Customer.ContactInfos.Where(d => d.Type.Code == DicContactInfoType.Codes.Email)
                        .Select(d => d.Info))
                : string.Empty;
        }

        private static void FillCustomer(Request request, Argument argument)
        {
            var declarent = request.RequestCustomers.FirstOrDefault(d => d.CustomerRole.Code == DicCustomerRoleCodes.Declarant);

            argument.CustomerType = new CustomerType
            {
                UID = (declarent?.Customer.Type.ExternalId ?? declarent?.Customer.TypeId).ToString(),
                Note = declarent?.Customer.NameRu ?? declarent?.Customer.NameKz ?? declarent?.Customer.NameEn
            };
            argument.CustomerName = declarent?.Customer.NameRu ?? declarent?.Customer.NameKz ?? declarent?.Customer.NameEn;
            argument.Login = declarent?.Customer.Xin;
            argument.Xin = declarent?.Customer.Xin;
            argument.Phone = declarent != null
                ? string.Join(", ",
                    declarent.Customer.ContactInfos.Where(d =>
                            d.Type.Code == DicContactInfoType.Codes.Phone ||
                            d.Type.Code == DicContactInfoType.Codes.MobilePhone)
                        .Select(d => d.Info))
                : string.Empty;
            argument.Fax = declarent != null
                ? string.Join(", ",
                    declarent.Customer.ContactInfos.Where(d => d.Type.Code == DicContactInfoType.Codes.Fax).Select(d => d.Info))
                : string.Empty;
            argument.Email = declarent != null
                ? string.Join(", ",
                    declarent.Customer.ContactInfos.Where(d => d.Type.Code == DicContactInfoType.Codes.Email)
                        .Select(d => d.Info))
                : string.Empty;
        }

        private static void FillTrademark(Request request, Argument argument)
        {
            argument.IsCollectiveTradeMark = request.RequestType?.Code == DicProtectionDocSubtypeCodes.CollectiveTrademark;
            argument.IsTMInStandartFont = request.TypeTrademark?.Code == DicTypeTrademark.Codes.Literal;
            argument.IsTMVolume = request.TypeTrademark?.Code == DicTypeTrademark.Codes.Volume;

            argument.Translation = request.Translation;
            argument.Transliteration = request.Transliteration;
        }

        private static void FillProduct(Request request, Argument argument)
        {
            argument.ProductType = request.RequestInfo?.ProductType;
            argument.ProductSpecialProp = request.RequestInfo?.ProductSpecialProp;
            argument.ProductPalce = request.RequestInfo?.ProductPlace;
        }

        private static void FillBreeding(Request request, Argument argument)
        {
            argument.BreedingNumber = request.RequestInfo?.BreedingNumber;
            argument.Breed = request.RequestInfo?.Breed;

            if (request.RequestInfo?.BreedCountry != null)
                argument.BreedCountry = new BreedCountry
                {
                    UID = (request.RequestInfo?.BreedCountry?.ExternalId ?? request.RequestInfo?.BreedCountry?.Id).ToString(),
                    Note = request.RequestInfo?.BreedCountry?.NameRu
                };
        }

        private static void FillDocInfo(Argument argument, Request request)
        {
            argument.DocInfo = new DocInfo
            {
                RegNumber = request.RequestNum,
                DocumentID = request.Barcode.ToString(),
                InNumber = request.IncomingNumber,
                DateNIIP = request.DateCreate.DateTime,
                ApplicationDate = request.RequestDate.GetValueOrDefault().Date
            };
        }

        #endregion

        #region Validate

        private ServerStatus ValidateRequest(Request request)
        {
            var result = new ServerStatus();

            var customerErrors = new List<string>();
            var documentErrors = new List<string>();

            if (request.CurrentWorkflow?.CurrentStage.IsFirst == true)
            {
                result.Message = "Заявка не должна быть на первоначальном этапе";
                return result;
            }

            if (request.RequestCustomers.All(d => d.CustomerRole.Code != DicCustomerRoleCodes.Declarant))
                customerErrors.Add("Заявитель");
            else  if (!_codes.Contains(request.ProtectionDocType.Code) && request.RequestCustomers.All(d => d.CustomerRole.Code != DicCustomerRoleCodes.Author))
                customerErrors.Add("Автор");
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeTrademarkCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementTrademark))
                    documentErrors.Add("документ \"Заявление ТЗ\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => !Images.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Изображение\"");
            }
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementNameOfOrigin))
                    documentErrors.Add("документ \"Заявление НМПТ\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => !Images.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Изображение\"");
            }
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeInventionCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementInventions))
                    documentErrors.Add("документ \"Заявление ИЗ\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.InventionDescription))
                    documentErrors.Add("документ \"Описание изобретения\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.Essay))
                    documentErrors.Add("документ \"Реферат\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.FormulaInvention))
                    documentErrors.Add("документ \"Формула ИЗ\"");
            }
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementSelectiveAchievs))
                    documentErrors.Add("документ \"Заявление СД\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.SAQuestionnaire))
                    documentErrors.Add("документ \"Анкета СД\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.AttributesTable))
                    documentErrors.Add("документ \"Таблица признаков\"");
            }
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementIndustrialDesigns))
                    documentErrors.Add("документ \"Заявление ПО\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.IndustrialModelDescription))
                    documentErrors.Add("документ \"Описание промышленного образца\"");
                else if (request.Documents.All(rd => !PoImages.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Изображение\"");
            }
            else if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeUsefulModelCode)
            {
                if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.StatementUsefulModels))
                    documentErrors.Add("документ \"Заявление ПМ\"");
                else if (request.Documents.All(rd => !Payments.Contains(rd.Document.Type.Code)))
                    documentErrors.Add("документ \"Платежный документ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.UsefulModelDescription))
                    documentErrors.Add("документ \"Описание ПМ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.FormulaUsefulModel))
                    documentErrors.Add("документ \"Формула ПМ\"");
                else if (request.Documents.All(rd => rd.Document.Type.Code != DicDocumentTypeCodes.Essay))
                    documentErrors.Add("документ \"Реферат\"");
            }
            
            if (customerErrors.Any())
                result.Message = $"В заявке отсутствует {string.Join(", ", customerErrors)}. ";

            if (documentErrors.Any())
                result.Message += $"В заявке отсутствует {string.Join(", ", documentErrors)}.";

            return result;
        }

        #endregion
    }
}