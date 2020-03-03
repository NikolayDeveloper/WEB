using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.ContractCustomers;
using Iserv.Niis.BusinessLogic.ContractRequestRelations;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Workflows.Contracts;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Domain.OldNiisEntities;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Services.Dapper;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Utils.Helpers;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Services.Implementations
{
    public class ImportContractsHelper : BaseImportHelper, IImportContractsHelper
    {
        #region Constructor

        private readonly IGenerateHash _generateHash;
        private readonly IFileStorage _fileStorage;
        private readonly IImportDocumentsHelper _importDocumentsHelper;

        public ImportContractsHelper(
            IConfiguration configuration,
            DictionaryHelper dictionaryHelper,
            IFileStorage fileStorage,
            IGenerateHash generateHash, 
            IImportDocumentsHelper importDocumentsHelper) : base(configuration, dictionaryHelper)
        {
            _fileStorage = fileStorage;
            _generateHash = generateHash;
            _importDocumentsHelper = importDocumentsHelper;

            SavedContracts = new List<Contract>();
        }

        public const int DeveloperUserId = 1;

        private List<Contract> SavedContracts { get; }

        #endregion

        public async Task ImportFromDb(string number, int requestId)
        {
            var refSqlQuey = string.Format(ImportSqlQueryHelper.ContractRefSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), string.Join(", ", ObjectType.GetContractRouteIds()), number);
            var oldRefContracts = await SqlDapperConnection.QueryAsync<RfMessageDocument>(refSqlQuey, TargetConnectionString);

            foreach (var oldRefContract in oldRefContracts)
            {
                var refContract = CreateRefContract(oldRefContract, requestId);
                if (refContract == null) continue;

                var oldContractId = refContract.ContractId;

                //Contract
                var newContractId = await FillContract(refContract.ContractId);
                if (newContractId == 0) continue;

                refContract.ContractId = newContractId;
                Executor.GetCommand<CreateContractRequestRelationsCommand>().Process(d => d.Execute(refContract));
                
                refContract.DateCreate = new DateTimeOffset(oldRefContract.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateContractRequestRelationsCommand>().Process(d => d.Execute(refContract));

                //ContractWorkflow
                await FillContractWorkflows(oldContractId, newContractId);
                
                //ContractWorkflow
                await FillContractCustomers(oldContractId, newContractId);

                //attachments
                await FillAttachments(oldContractId, newContractId);

                try
                {
                    //CurrentWorkflow
                    var currentWorkflowSqlQuery = string.Format(ImportSqlQueryHelper.ContractsCurrentWorkflowSqlQuery, newContractId);
                    await SqlDapperConnection.ExecuteAsync(currentWorkflowSqlQuery, SourceConnectionString);

                    //MainAttachment
                    var setMainAttachmentSqlQuery = string.Format(ImportSqlQueryHelper.SetContractsMainAttachmentSqlQuery, newContractId);
                    await SqlDapperConnection.ExecuteAsync(setMainAttachmentSqlQuery, SourceConnectionString);
                }
                catch (Exception)
                {
                    // ignored
                }

                //Documents
                await _importDocumentsHelper.ImportContractFromDb(oldContractId, newContractId);
            }
        }

        #region RefContract

        private ContractRequestRelation CreateRefContract(RfMessageDocument oldRefContract, int requestId)
        {
            try
            {
                var requestContract = new ContractRequestRelation
                {
                    DateCreate = new DateTimeOffset(oldRefContract.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldRefContract.DateCreate.GetValueOrDefault(DateTime.Now)),
                    ContractId = oldRefContract.RefdocumentId,
                    ExternalId = oldRefContract.Id,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return requestContract;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Contract

        private async Task<int> FillContract(int refContractId)
        {
            try
            {
                var sqlQuery = string.Format(ImportSqlQueryHelper.ContractSqlQuery, string.Join(", ", ObjectType.GetContractRouteIds()), refContractId);
                var oldContacts = await SqlDapperConnection.QueryAsync<DdDocument>(sqlQuery, TargetConnectionString);
                var ddContracts = oldContacts.ToList();
                if (!ddContracts.Any()) return 0;
                var contract = await CreateContract(ddContracts.FirstOrDefault());
                var newContractId = await Executor.GetCommand<CreateContractCommand>().Process(d => d.ExecuteFullObjAsync(contract));

                contract.Id = newContractId;

                contract.DateCreate = new DateTimeOffset(ddContracts.First().DateCreate.GetValueOrDefault(DateTime.Now));
                Executor.GetCommand<UpdateContractCommand>().Process(d => d.Execute(contract));

                SavedContracts.Add(contract);

                return newContractId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async Task<Contract> CreateContract(DdDocument oldContract)
        {
            var customerId = GetObjectId<DicCustomer>(oldContract.CustomerId);
            if ((customerId == null || customerId == 0) && oldContract.CustomerId.HasValue)
                customerId = await GetCustomer(oldContract.CustomerId.Value);

            var contract = new Contract
            {
                AddresseeId = customerId,
                ApplicationNum = oldContract.ReqNumber21,
                Barcode = oldContract.Id,
                BulletinDate = GetNullableDate(oldContract.Dby),
                ContractNum = oldContract.ReqNumber21,
                CopyCount = oldContract.CopyCount,
                DateCreate = new DateTimeOffset(oldContract.DateCreate.GetValueOrDefault(DateTime.Now)),
                DateUpdate = new DateTimeOffset(oldContract.Stamp.GetValueOrDefault(DateTime.Now)),
                DepartmentId = GetObjectId<DicDepartment>(oldContract.DepartmentId),
                Description = $"{oldContract.DescriptionMlRu} {oldContract.DescriptionMlKz} {oldContract.DescriptionMlEn}",
                DivisionId = GetObjectId<DicDivision>(oldContract.DivisionId),
                ExternalId = oldContract.Id,
                GosDate = GetNullableDate(oldContract.GosDate11),
                GosNumber = oldContract.GosNumber11,
                IsRead = true,
                NameEn = oldContract.DescriptionMlEn,
                NameKz = oldContract.DescriptionMlKz,
                NameRu = oldContract.DescriptionMlRu,
                NumberBulletin = oldContract.Nby,
                OutgoingNumber = oldContract.Outnum,
                PageCount = oldContract.PageCount,
                ProtectionDocTypeId = DictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocType), DicProtectionDocTypeCodes.ProtectionDocTypeContractCode),
                ReceiveTypeId = GetObjectId<DicReceiveType>(oldContract.SendType),
                RegDate = GetNullableDate(oldContract.ReqDate22),
                StatusId = GetObjectId<DicContractStatus>(oldContract.StatusId),
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                ValidDate = oldContract.Stz17?.ToString("dd.MM.yyyy"),
                TypeId = GetObjectId<DicProtectionDocSubType>(oldContract.SubtypeId)
            };

            return contract;
        }

        #endregion

        #region Workflows

        private async Task FillContractWorkflows(int oldContractId, int newContractId)
        {
            var wfSqlQuery = string.Format(ImportSqlQueryHelper.ContractsWfSqlQuery, string.Join(", ", ObjectType.GetContractRouteIds()), oldContractId);
            var oldWorkflows = await SqlDapperConnection.QueryAsync<WtPtWorkoffice>(wfSqlQuery, TargetConnectionString);
            foreach (var oldWorkflow in oldWorkflows)
            {
                var contractWorkflow = CreateWorkflow(oldWorkflow, newContractId);
                if (contractWorkflow == null) continue;
                await Executor.GetCommand<CreateContractWorkflowCommand>().Process(d => d.ExecuteAsync(contractWorkflow));

                contractWorkflow.DateCreate = new DateTimeOffset(oldWorkflow.DateCreate.GetValueOrDefault(DateTime.Now));
                Executor.GetCommand<UpdateContractWorkflowCommand>().Process(d => d.ExecuteAsync(contractWorkflow));
            }
        }

        private ContractWorkflow CreateWorkflow(WtPtWorkoffice oldWorkoffice, int newContractId)
        {
            try
            {
                var wf = new ContractWorkflow
                {
                    ControlDate = GetNullableDate(oldWorkoffice.ControlDate),
                    CurrentStageId = GetObjectId<DicRouteStage>(oldWorkoffice.ToStageId),
                    CurrentUserId = GetUserId(oldWorkoffice.ToUserId),
                    DateCreate = new DateTimeOffset(oldWorkoffice.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldWorkoffice.Stamp.GetValueOrDefault(DateTime.Now)),
                    Description = oldWorkoffice.Description,
                    ExternalId = oldWorkoffice.Id,
                    FromStageId = GetObjectId<DicRouteStage>(oldWorkoffice.FromStageId),
                    FromUserId = GetUserId(oldWorkoffice.FromUserId),
                    IsComplete = GenerateHelper.StringToNullableBool(oldWorkoffice.IsComplete),
                    IsMain = false,
                    IsSystem = GenerateHelper.StringToNullableBool(oldWorkoffice.IsSystem),
                    OwnerId = newContractId,
                    RouteId = GetObjectId<DicRoute>(oldWorkoffice.TypeId),
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                };

                return wf;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Customers

        private async Task FillContractCustomers(int oldContractId, int newContractId)
        {
            var customersSqlQuery = string.Format(ImportSqlQueryHelper.ContractCustomersSqlQuery, string.Join(", ", ObjectType.GetContractRouteIds()), oldContractId);
            var customers = await SqlDapperConnection.QueryAsync<RfCustomerAddressExtention>(customersSqlQuery, TargetConnectionString);
            foreach (var customer in customers)
            {
                var contractCustomer = await CreateCustomer(customer, newContractId);
                if (contractCustomer == null) continue;
                await Executor.GetCommand<CreateContractCustomerCommand>().Process(d => d.ExecuteAsync(contractCustomer));

                contractCustomer.DateCreate = new DateTimeOffset(customer.DateCreate.GetValueOrDefault(DateTime.Now));
                await Executor.GetCommand<UpdateContractCustomerCommand>().Process(d => d.ExecuteAsync(contractCustomer));
            }
        }
        
        private async Task<ContractCustomer> CreateCustomer(RfCustomerAddressExtention oldCustomer, int contractId)
        {
            try
            {
                var customerId = GetObjectId<DicCustomer>(oldCustomer.CustomerId);
                
                if ((customerId == null || customerId == 0) && oldCustomer.CustomerId.HasValue)
                    customerId = await GetCustomer(oldCustomer.CustomerId.Value);

                if (customerId == null || customerId == 0)
                    return null;

                var customer = new ContractCustomer
                {
                    Address = oldCustomer.AddresNameRu,
                    CustomerId = customerId,
                    CustomerRoleId = GetObjectId<DicCustomerRole>(oldCustomer.CType),
                    DateCreate = new DateTimeOffset(oldCustomer.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldCustomer.Stamp.GetValueOrDefault(DateTime.Now)),
                    DateBegin = GetNullableDate(oldCustomer.DateBegin),
                    DateEnd = GetNullableDate(oldCustomer.DateEnd),
                    ExternalId = oldCustomer.Id,
                    ContractId = contractId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    AddressEn = oldCustomer.AddresNameEn,
                    AddressKz = oldCustomer.AddresNameKz
                };

                return customer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Attachments

        private async Task FillAttachments(int oldContractId, int newContractId)
        {
            var attachmentsSqlQuery = string.Format(ImportSqlQueryHelper.DocumentsAttachmentsSqlQuery, oldContractId);
            var oldAttachments = await SqlDapperConnection.QueryAsync<DocumentData>(attachmentsSqlQuery, TargetAttachmentConnectionString);
            foreach (var oldAttachment in oldAttachments)
            {
                var attachment = await CreateAttachment(oldAttachment, newContractId);
                if (attachment == null) continue;
                await Executor.GetCommand<CreateAttachmentCommand>().Process(d => d.ExecuteAsync(attachment));
            }
        }

        private async Task<Attachment> CreateAttachment(DocumentData oldAttachment, int newContractId)
        {
            try
            {
                var fileName = oldAttachment.FileName;
                var file = oldAttachment.File;

                var contractInfo = SavedContracts.FirstOrDefault(d => d.Id == newContractId);
                if (contractInfo == null) return null;

                var bucketName = GetBucketName(null, null, newContractId);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"{Guid.NewGuid()}.{FileTypes.Pdf}";
                }

                var originalName = GetFolderWithOriginalName(null, null, newContractId, fileName, null);
                var validName = fileName.MakeValidFileName();
                var contentType = FileTypeHelper.GetContentType(fileName);

                await _fileStorage.AddAsync(bucketName, originalName, file, contentType);

                var attachment = new Attachment
                {
                    AuthorId = DeveloperUserId,
                    ContentType = contentType,
                    BucketName = bucketName,
                    IsMain = true,
                    CopyCount = contractInfo.CopyCount,
                    PageCount = contractInfo.PageCount,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now,
                    OriginalName = originalName,
                    Length = file.Length,
                    ValidName = validName,
                    Hash = _generateHash.GenerateFileHash(file),
                    ExternalId = contractInfo.ExternalId,
                    IsDeleted = false
                };

                return attachment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}