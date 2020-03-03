using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Implementations
{
    public class ContractApplicationSendService:IContractApplicationSendService
    {
        private readonly NiisWebContext _niisWebContext;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly IntegrationDictionaryHelper _integrationDictionaryHelper;
        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly IntegrationDocumentHelper _documentHelper;
        private readonly INumberGenerator _numberGenerator;
        private readonly IIntegrationStatusUpdater _integrationStatusUpdater;
        private readonly AppConfiguration _configuration;

        public ContractApplicationSendService(
            NiisWebContext niisWebContext, 
            DictionaryHelper dictionaryHelper, 
            IntegrationDictionaryHelper integrationDictionaryHelper,
            IntegrationAttachFileHelper attachFileHelper,
            IntegrationDocumentHelper documentHelper,
            INumberGenerator numberGenerator,
            IIntegrationStatusUpdater integrationStatusUpdater,
            AppConfiguration configuration)
        {
            _niisWebContext = niisWebContext;
            _dictionaryHelper = dictionaryHelper;
            _integrationDictionaryHelper = integrationDictionaryHelper;
            _attachFileHelper = attachFileHelper;
            _documentHelper = documentHelper;
            _numberGenerator = numberGenerator;
            _integrationStatusUpdater = integrationStatusUpdater;
            _configuration = configuration;
        }

        /// <summary>
        /// Добавление договора
        /// </summary>
        /// <param name="request">Входные параметры</param>
        /// <param name="response">Данные для ответа</param>
        /// <returns></returns>
        public ContractResponse AddContract(ContractRequest request, ContractResponse response)
        {
            var typeId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicDocumentType),request.DocumentType.Id);
            DicCustomer customer;
            customer = Mapper.Map<DicCustomer>(
                request,
                el => el.Items[nameof(customer.TypeId)] = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerType), request.Addressee.CustomerType.Id).Id);

            customer.CountryId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicCountry), request.Addressee.CountryInfo.Id);

            var addressee = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(customer);


            var correspondence = request.CorrespondenceAddress;
            var protectionDocTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType),
                DicProtectionDocTypeCodes.ProtectionDocTypeContractCode);

            #region Добавление договора
            var contract = new Contract
            {
                ProtectionDocTypeId = protectionDocTypeId,
                Description = request.DocumentDescriptionRu,
                ReceiveTypeId = int.Parse(DicReceiveTypeCodes.ElectronicFeed),
                DivisionId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDivision), DicDivisionCodes.RGP_NIIS),
                AddresseeId = addressee.Id,
                AddresseeAddress = addressee.Address,
                StatusId = 1

            };

            CreateContract(Mapper.Map(request, contract));
            #endregion

            #region Добавление адресата для переписки
            if (correspondence != null)
            {
                var correspondenceCustomer = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(new DicCustomer
                {
                    TypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), DicCustomerTypeCodes.Undefined),
                    NameRu = correspondence.CustomerName,
                    Phone = correspondence.Phone,
                    PhoneFax = correspondence.Fax,
                    Email = correspondence.Email
                });

                _niisWebContext.ContractCustomers.Add(new ContractCustomer
                {
                    CustomerId = correspondenceCustomer.Id,
                    ContractId = contract.Id,
                    DateCreate = DateTimeOffset.Now,
                    CustomerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), DicCustomerRole.Codes.Correspondence)
                });
                _niisWebContext.SaveChanges();
            }
            #endregion

            #region Добавление контрагентов

            foreach (var item in request.BlockCustomers)
            {
                if (item.CustomerType == null)
                    continue;

                if (item.CustomerType.Id == 0)
                    item.CustomerType.Id =
                        _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType),
                            DicCustomerTypeCodes.Undefined);

                var dicCustomer = new DicCustomer
                {
                    TypeId = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerType), item.CustomerType.Id).Id,
                    NameRu = item.NameRu,
                    NameKz = item.NameKz,
                    NameEn = item.NameEn,
                    Phone = item.Phone,
                    PhoneFax = item.Fax,
                    Email = item.Email,
                    Address = $"{item.Region}, {item.Street}, {item.Index}",
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

                if (item.RegistrationDate != DateTime.MinValue)
                    dicCustomer.RegDate = new DateTimeOffset(item.RegistrationDate.GetValueOrDefault());

                var contractCustomer = new ContractCustomer
                {
                    CustomerId = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(dicCustomer).Id,
                    ContractId = contract.Id,
                    DateCreate = DateTimeOffset.Now,
                    CustomerRoleId = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerRole), item.CustomerRole.Id)?.Id
                };

                _niisWebContext.ContractCustomers.Add(contractCustomer);
                _niisWebContext.SaveChanges();

            }

            #endregion

            #region Добавление прикрепленного документа заявления
            var document = new Document(_dictionaryHelper.GetDocumentType(typeId).type)
            {
                TypeId = typeId,
                DateCreate = DateTimeOffset.Now,
                AddresseeId = addressee.Id,
                StatusId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus), DicDocumentStatusCodes.InWork),
                ReceiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed)
            };
            _documentHelper.CreateDocument(document);

            _attachFileHelper.AttachFile(new AttachedFileModel
            {
                PageCount = request.RequisitionFile.PageCount.GetValueOrDefault(),
                CopyCount = request.RequisitionFile.CopyCount.GetValueOrDefault(),
                File = request.RequisitionFile.Content,
                Length = request.RequisitionFile.Content.Length,
                IsMain = true,
                Name = request.RequisitionFile.Name
            }, document);

            _niisWebContext.Documents.Update(document);
            _niisWebContext.SaveChanges();

            _niisWebContext.ContractsDocuments.Add(new ContractDocument
            {
                DocumentId = document.Id,
                ContractId = contract.Id
            });
            _niisWebContext.SaveChanges();
            #endregion

            #region Добавление документов
            foreach (var attachedFile in request.AttachmentFiles)
            {
                if (attachedFile.File == null)
                    continue;

                var documentType = _dictionaryHelper.GetDocumentType(attachedFile.FileType.Id);
                var inWorkStatusId =
                    _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus), DicDocumentStatusCodes.InWork);
                var completedStatusId =
                    _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus), DicDocumentStatusCodes.Completed);

                var attachmentDocument = new Document(documentType.type)
                {
                    TypeId = documentType.typeId,
                    DateCreate = DateTimeOffset.Now,
                    StatusId = documentType.type == DocumentType.DocumentRequest ? completedStatusId : inWorkStatusId,
                    AddresseeId = addressee.Id,
                    ReceiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed)
                };
                _documentHelper.CreateDocument(attachmentDocument);
                var attached = new AttachedFileModel
                {
                    Length = attachedFile.File.Content.Length,
                    PageCount = attachedFile.File.PageCount.GetValueOrDefault(),
                    File = attachedFile.File?.Content,
                    Name = attachedFile.File.Name,
                    IsMain = true,
                    CopyCount = 1
                };

                _attachFileHelper.AttachFile(attached, document);
                _niisWebContext.ContractsDocuments.Add(new ContractDocument
                {
                    DocumentId = attachmentDocument.Id,
                    ContractId = contract.Id
                });
                _niisWebContext.SaveChanges();
            }
            #endregion

            #region Добавление связи с ОД
            foreach (var contractPatent in request.PatentNumbers)
            {
                //Получаем типа патента
                if (!(_dictionaryHelper.GetDictionaryByExternalId(nameof(DicProtectionDocType), contractPatent.PatentType.Id) is DicProtectionDocType type))
                    throw new NotSupportedException($"Тип с идентефикатором {contractPatent.PatentType.Id} не найден");

                var patentId = _niisWebContext
                    .ProtectionDocs
                    .Where(d => d.GosNumber == contractPatent.PatentNumber && d.Request.ProtectionDocTypeId == type.Id)
                    .Select(d => d.Id).FirstOrDefault();

                _niisWebContext.ContractProtectionDocRelations.Add(new ContractProtectionDocRelation
                {
                    ProtectionDocId = patentId,
                    ContractId = contract.Id
                });
                _niisWebContext.SaveChanges();
            }
            #endregion

            #region Добавление этапа обработки
            var routeId = _integrationDictionaryHelper.GetRouteIdByProtectionDocType(protectionDocTypeId);
            var stage = _integrationDictionaryHelper.GetRouteStage(routeId);

            var contractWorkflow = new ContractWorkflow
            {
                OwnerId = contract.Id,
                DateCreate = DateTimeOffset.Now,
                RouteId = routeId,
                CurrentStageId = stage.Id,
                CurrentUserId = _configuration.AuthorAttachmentDocumentId,
                IsComplete = stage.IsLast,
                IsSystem = stage.IsSystem,
                IsMain = stage.IsMain
            };
            _niisWebContext.ContractWorkflows.Add(contractWorkflow);
            _niisWebContext.SaveChanges();

            contract.CurrentWorkflowId = contractWorkflow.Id;
            _niisWebContext.Contracts.Update(contract);
            _niisWebContext.SaveChanges();
            #endregion


            _integrationStatusUpdater.Add(contractWorkflow.Id);

            var onlineStatusId = 0;
            if (contractWorkflow.CurrentStageId != null)
                onlineStatusId = _integrationDictionaryHelper.GetOnlineStatus(contractWorkflow.CurrentStageId.Value);

            response.DocumentId = contract.Barcode;
            response.DocumentNumber = contract.IncomingNumber;
            response.RequisitionStatus = onlineStatusId;

           
            return response;
        }

        private void CreateContract(Contract contract)
        {
            _numberGenerator.GenerateBarcode(contract);
            _numberGenerator.GenerateIncomingNum(contract);
            _niisWebContext.Contracts.Add(contract);
            _niisWebContext.SaveChanges();
        }
    }
}
