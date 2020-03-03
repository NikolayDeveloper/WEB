using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using DocumentType = Iserv.Niis.Domain.Enums.DocumentType;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class RequisitionSendService : IRequisitionSendService
    {
        private readonly AppConfiguration _configuration;
        private readonly IntegrationDictionaryHelper _integrationDictionaryHelper;
        private readonly IntegrationEnumMapper _integrationEnumMapper;
        private readonly NiisWebContext _niisContext;
        private readonly IntegrationValidationHelper _validationHelper;
        private readonly INumberGenerator _numberGenerator;
        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly IntegrationDocumentHelper _documentHelper;
        private readonly IntegrationEgovPayHelper _egovPayHelper;
        private readonly IIntegrationStatusUpdater _integrationStatusUpdater;
        private readonly DictionaryHelper _dictionaryHelper;

        public RequisitionSendService(
            NiisWebContext context,
            AppConfiguration configuration,
            IntegrationValidationHelper validationHelper, IntegrationDictionaryHelper integrationDictionaryHelper,
            IntegrationEnumMapper integrationEnumMapper, INumberGenerator numberGenerator, IntegrationAttachFileHelper attachFileHelper,
            IntegrationDocumentHelper documentHelper, IntegrationEgovPayHelper egovPayHelper,
            IIntegrationStatusUpdater integrationStatusUpdater,
            DictionaryHelper dictionaryHelper)
        {
            _niisContext = context;
            _configuration = configuration;
            _validationHelper = validationHelper;
            _integrationDictionaryHelper = integrationDictionaryHelper;
            _integrationEnumMapper = integrationEnumMapper;
            _numberGenerator = numberGenerator;
            _attachFileHelper = attachFileHelper;
            _documentHelper = documentHelper;
            _egovPayHelper = egovPayHelper;
            _integrationStatusUpdater = integrationStatusUpdater;
            _dictionaryHelper = dictionaryHelper;
        }

        public (int requestId, int onlineStatusId, string incomingNum, int barcode) RequisitionDocumentAdd(RequisitionSendArgument argument)
        {
            var protectionDocTypeId = argument.PatentType.UID;
            //var protectionDocTypeCode = _dictionaryHelper.GetDictionaryCodeById(nameof(DicProtectionDocType), protectionDocTypeId);
            var protectionDocTypeCode = _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType), protectionDocTypeId);
            var receiveRypeCode = _integrationDictionaryHelper.GetReceiveTypeCode(argument.SystemInfo.Sender);
            var receiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), receiveRypeCode);
            var departmentCode = _integrationEnumMapper.MapToDepartment(protectionDocTypeCode);
            var departmentId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDepartment) ,departmentCode);
            var speciesTradeMarkId = _dictionaryHelper.GetSpeciesTradeMarkId(argument.IsCollectiveTradeMark);
            var typeTrademarkId = _dictionaryHelper.GetNullableDictionaryIdByCode(nameof(DicTypeTrademark), argument.PatentSubType);
            DicCustomer customer = null;
            customer = Mapper.Map<DicCustomer>(
                    argument,
                    el => el.Items[nameof(customer.TypeId)] = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerType), argument.CustomerType.UID).Id);

            var addressee = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(customer);

            var subTypeCode = GetSubType(protectionDocTypeCode);
            var requestTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocSubType), subTypeCode);

            //if (int.TryParse(argument.PatentSubType, out var patentSubType))
            //{
            //    _dictionaryHelper.ChechDictionaryId(nameof(DicProtectionDocSubType), patentSubType);
            //    requestTypeId = patentSubType;
            //}

            var request = new Request
            {
                ProtectionDocTypeId = protectionDocTypeId, 
                ReceiveTypeId = receiveTypeId,
                DepartmentId = departmentId,
                DivisionId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDivision), DicDivisionCodes.RGP_NIIS),
                FlDivisionId = _integrationDictionaryHelper.GetDivisionId(_configuration.MainExecutorIds[protectionDocTypeCode]),
                IsComplete = false,
                AddresseeId = addressee.Id,
                AddresseeAddress = addressee.Address,
                UserId = _configuration.MainExecutorIds[protectionDocTypeCode],
                SpeciesTradeMarkId = speciesTradeMarkId,
                TypeTrademarkId = typeTrademarkId,
                RequestTypeId = requestTypeId,
                IsFromLk = true
            };

            if (argument.BlockFile.Any(d => d.Type.UID == 292))
            {
                request.Referat = argument.BlockFile.First(d => d.Type.UID == 292).Type.Note;
            }

            if (argument.BlockFile != null)
            {
                var imageFile = _attachFileHelper.GetImageFile(argument.BlockFile,
                    _validationHelper.SenderIsPep(argument.SystemInfo.Sender));
                if (imageFile != null)
                {
                    request.Image = imageFile;
                    request.IsImageFromName = false;
                }
            }

            CreateRequest(Mapper.Map(argument, request));

            if (argument.BlockFile != null)
            {
                var meidaFile = argument.BlockFile.Where(d => d.Type.UID == 4421).ToList();
                if (meidaFile.Any())
                {
                    foreach (var attachedFile in meidaFile)
                    {
                        _attachFileHelper.AttachFile(new AttachedFileModel
                        {
                            PageCount = attachedFile.PageCount,
                            CopyCount = 0,
                            File = attachedFile.File.Content,
                            Length = attachedFile.File.Content.Length,
                            IsMain = false,
                            Name = attachedFile.File.Name
                        }, request.Id);
                    }
                }
            }

            var requestInfo = new RequestInfo
            {
                RequestId = request.Id,
                FlagTth = false,
                IsConventionPriority = false,
                IsExhibitPriority = false,
                IsStandardFont = false,
                IsTransliteration = false,
                IsTranslation = false,
                IsVolumeTZ = false,
                IsColorPerformance = false,
                AcceptAgreement = false,
                BreedCountryId = argument.BreedCountry != null ? (argument.BreedCountry.UID > 0 ? _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicCountry),argument.BreedCountry.UID) : (int?)null) : null
            };

            _niisContext.RequestInfos.Add(Mapper.Map(argument, requestInfo));
            _niisContext.SaveChanges();

            var dicDocTypeCode = _integrationEnumMapper.MapProtectionDocTypeToDocumentType(protectionDocTypeCode);
            var docTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentType), dicDocTypeCode);

            var document = new Document(_dictionaryHelper.GetDocumentType(docTypeId).type)
            {
                TypeId = docTypeId,
                DateCreate = DateTimeOffset.Now,
                AddresseeId = addressee.Id,
                StatusId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus), DicDocumentStatusCodes.InWork),
                ReceiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed)
            };
            _documentHelper.CreateDocument(document);

            _attachFileHelper.AttachFile(new AttachedFileModel
            {
                PageCount = argument.PageCount,
                CopyCount = argument.CopyCount,
                File = argument.RequisitionFile.Content,
                Length = argument.RequisitionFile.Content.Length,
                IsMain = true,
                Name = argument.RequisitionFile.Name
            }, document);


            _niisContext.Documents.Update(document);
            _niisContext.SaveChanges();

            _niisContext.RequestsDocuments.Add(new RequestDocument
            {
                DocumentId = document.Id,
                RequestId = request.Id
            });
            _niisContext.SaveChanges();

            var routeId = _integrationDictionaryHelper.GetRouteIdByProtectionDocType(protectionDocTypeId);
            var stage = _integrationDictionaryHelper.GetRouteStage(routeId);

            var requestWorkflow = new RequestWorkflow
            {
                OwnerId = request.Id,
                DateCreate = DateTimeOffset.Now,
                RouteId = routeId,
                CurrentStageId = stage.Id,
                CurrentUserId = _configuration.MainExecutorIds[protectionDocTypeCode],
                IsComplete = stage.IsLast,
                IsSystem = stage.IsSystem,
                IsMain = stage.IsMain
            };
            _niisContext.RequestWorkflows.Add(requestWorkflow);
            _niisContext.SaveChanges();

            request.CurrentWorkflowId = requestWorkflow.Id;
            _niisContext.Requests.Update(request);
            _niisContext.SaveChanges();

            _integrationStatusUpdater.Add(requestWorkflow.Id);

            var dicCustomer = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(new DicCustomer
            {
                TypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), DicCustomerTypeCodes.Undefined),
                NameRu = argument.AdrCustomerName,
                Phone = argument.AdrPhone,
                PhoneFax = argument.AdrFax,
                Email = argument.AdrEmail,
                Address = $"{argument.AdrRegion}, {argument.AdrStreet}, {argument.AdrPostCode}"
            });

            _niisContext.RequestCustomers.Add(new RequestCustomer
            {
                CustomerId = dicCustomer.Id,
                RequestId = request.Id,
                DateCreate = DateTimeOffset.Now,
                CustomerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), DicCustomerRole.Codes.Correspondence)
            });
            _niisContext.SaveChanges();

            _egovPayHelper.CreatePay(argument.Pay);

            var onlineStatusId = 0;
            if (requestWorkflow.CurrentStageId != null)
                onlineStatusId = _integrationDictionaryHelper.GetOnlineStatus(requestWorkflow.CurrentStageId.Value);

            return (requestId: request.Id, onlineStatusId: onlineStatusId, incomingNum: request.IncomingNumber, barcode:request.Barcode);
        }

        private string GetSubType(string typeCode)
        {
            string subtypeCode;

            switch (typeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalTradeMark;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalInvention;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalUsefulModel;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalIndustrialSample;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalNameOfOrigin;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalSelectionAchieve;
                    break;

                default:
                    subtypeCode = null;
                    break;
            }

            return subtypeCode;
        }

        public void RequisitionMktuAdd(RefKey[] blockClassification, int requestId)
        {
            foreach (var item in blockClassification)
            {
                if (string.IsNullOrEmpty(item.Note))
                {
                    continue;
                }
                _niisContext.ICGSRequests.Add(new ICGSRequest
                {
                    DateCreate = DateTimeOffset.Now,
                    IcgsId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicICGS), item.UID),
                    IsNegative = false,
                    Description = item.Note,
                    RequestId = requestId
                });
                _niisContext.SaveChanges();
            }
        }

        public void RequisitionEarlyRegAdd(EarlyReg[] blockEarlyReg, int requestId)
        {
            foreach (var item in blockEarlyReg)
            {
                var earlyReg = new RequestEarlyReg
                {
                    DateCreate = DateTimeOffset.Now,
                    EarlyRegTypeId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicEarlyRegType), item.EarlyTypeId.UID),
                    RegCountryId = item.Country.UID,
                    RegNumber = item.Num,
                    RequestId = requestId,
                    NameSD = item.Name
                };

                if (item.Date != null)
                    earlyReg.RegDate = new DateTimeOffset(item.Date.Value);

                _niisContext.RequestEarlyRegs.Add(earlyReg);
                _niisContext.SaveChanges();
            }
        }

        public void RequisitionColorAdd(RefKey[] blockColor, int requestId, int protectionDocTypeId)
        {
            var protectionDocTypeCode = _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType), protectionDocTypeId);
            if (protectionDocTypeCode == DicProtectionDocType.Codes.Trademark)
            {
                foreach (var item in blockColor)
                {
                    _niisContext.DicColorTzRequestRelations.Add(new DicColorTZRequestRelation
                    {
                        ColorTzId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicColorTZ), item.UID),
                        RequestId = requestId
                    });
                    _niisContext.SaveChanges();
                }

                return;
            }
            foreach (var item in blockColor)
            {
                _niisContext.DicIcfemRequestRelations.Add(new DicIcfemRequestRelation
                {
                    RequestId = requestId,
                    DicIcfemId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicICFEM), item.UID)
                });
                _niisContext.SaveChanges();
            }

        }

        public void RequisitionBlockCustomerAdd(Customer[] customers, int requestId)
        {
            foreach (var item in customers)
            {
                if (item.Type == null)
                    continue;
                var customer = new DicCustomer
                {
                    TypeId = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerType), item.Type.UID).Id,
                    NameRu = item.NameRu,
                    NameKz = item.NameKz,
                    NameEn = item.NameEn,
                    Phone = item.Phone,
                    PhoneFax = item.Fax,
                    Email = item.Email,
                    Address = $"{item.AdrObl}, {item.AdrStreet}, {item.AdrPostCode}",
                    CertificateNumber = item.CertificateNumber,
                    CertificateSeries = item.CertificateSeries,
                    Opf = item.Opf,
                    ApplicantsInfo = item.ApplicantsInfo,
                    PowerAttorneyFullNum = item.PowerAttorneyFullNum,
                    ShortDocContent = item.ShortDocContent,
                    Xin = item.Xin,
                    NotaryName = item.NotaryName,
                    DateCreate = DateTimeOffset.Now
                };

                if (item.RegDate != DateTime.MinValue)
                    customer.RegDate = new DateTimeOffset(item.RegDate);

                var requestCustomer = new RequestCustomer
                {
                    CustomerId = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(customer).Id,
                    RequestId = requestId,
                    DateCreate = DateTimeOffset.Now,
                    CustomerRoleId = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerRole), item.PatentLinkType.UID)?.Id
                };

                _niisContext.RequestCustomers.Add(requestCustomer);
                _niisContext.SaveChanges();
            }
        }

        public void RequisitionBlockFileAdd(AttachedFile[] blockFile, int requestId, string sender)
        {
            foreach (var attachedFile in blockFile.Where(d => d.Type.UID != 4421))
            {
                if (attachedFile.File == null)
                    continue;

                var documentType = _dictionaryHelper.GetDocumentType(attachedFile.Type.UID);
                var addresseeId = _integrationDictionaryHelper.GetCustomerIdByRequestId(requestId);
                var document = new Document(documentType.type)
                {
                    TypeId = documentType.typeId,
                    DateCreate = DateTimeOffset.Now,
                    StatusId = documentType.type == DocumentType.DocumentRequest? Int32.Parse(DicDocumentStatusCodes.Completed): Int32.Parse(DicDocumentStatusCodes.InWork),
                    AddresseeId = addresseeId,
                    ReceiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed)
                };
                _documentHelper.CreateDocument(document);
                var attached = new AttachedFileModel
                {
                    Length = attachedFile.File.Content.Length,
                    PageCount = attachedFile.PageCount,
                    File = attachedFile.File?.Content,
                    Name = attachedFile.File.Name,
                    IsMain = true,
                    CopyCount = 1
                };
                if (_validationHelper.SenderIsPep(sender))
                {
                    if (string.IsNullOrEmpty(attachedFile.File.ShepFile?.Name))
                        continue; 
                    attached.Name = attachedFile.File.ShepFile.Name;
                    try
                    {
                        attached.File = _attachFileHelper.ShepFileDownload(attachedFile.File.ShepFile);
                    }
                    catch
                    {
                        Log.Warning($"Не удалось загрузить файл с ШЕПа, ID заявки = {requestId}");
                        continue; 
                    }
                }
                else
                {
                    attached.Name = attachedFile.File.Name;
                    attached.File = attachedFile.File?.Content;
                }
                _attachFileHelper.AttachFile(attached, document);
                _niisContext.RequestsDocuments.Add(new RequestDocument
                {
                    DocumentId = document.Id,
                    RequestId = requestId
                });
                _niisContext.SaveChanges();
            }
        }
        #region PrivateMethods
        private void CreateRequest(Request request)
        {
            _numberGenerator.GenerateIncomingNum(request);
            _numberGenerator.GenerateBarcode(request);
            //request.RequestDate = DateTimeOffset.Now;
            _niisContext.Requests.Add(request);
            _niisContext.SaveChanges();
        }
        #endregion
    }
}