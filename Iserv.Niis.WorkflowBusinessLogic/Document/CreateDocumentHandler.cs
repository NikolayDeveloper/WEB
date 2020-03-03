using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Common;
using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateDocumentHandler: BaseHandler
    {
        public int Execute(int ownerId, Owner.Type ownerType, string documentTypeCode, DocumentType type, UserInputDto userInputDto, int? bulletinId = null, int? protectionDocTypeId = null)
        {
            var documentType = Executor.GetQuery<GetDicDocumentTypeByCodeQuery>()
                                .Process(q => q.Execute(documentTypeCode ?? string.Empty));
            if (documentType == null)
                throw new DataNotFoundException(nameof(DicDocumentType), DataNotFoundException.OperationType.Read,
                    documentTypeCode);

            int? currentUserId;
            int? addresseeId;


            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(ownerId));
                    if (request == null)
                        throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = request.RequestCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    currentUserId = request.CurrentWorkflow.CurrentUserId;
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute(ownerId));
                    if (protectionDoc == null)
                        throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = protectionDoc.ProtectionDocCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    currentUserId = protectionDoc.CurrentWorkflow.CurrentUserId;
                    break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute(ownerId));
                    if (contract == null)
                        throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = contract.ContractCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    currentUserId = contract.CurrentWorkflow.CurrentUserId;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork));

            var patent = new Domain.Entities.Document.Document
            {
                TypeId = documentType.Id,
                AddresseeId = addresseeId,
                DocumentType = type,
                DateCreate = DateTimeOffset.Now,
                BulletinId = bulletinId,
                ProtectionDocTypeId = protectionDocTypeId,
                StatusId = workStatus.Id
            };
            Executor.GetHandler<GenerateBarcodeHandler>().Process<Domain.Entities.Document.Document>(c => c.Execute(patent));
            var documentId = Executor.GetCommand<CreateDocumentCommand>().Process(c => c.Execute(patent));

            switch (ownerType)
            {
                case Owner.Type.Request:
                    Executor.GetCommand<CreateRequestDocumentCommand>().Process(c =>
                        c.Execute(
                            new RequestDocument
                            {
                                RequestId = ownerId,
                                DocumentId = documentId

                            }));
                    break;
                case Owner.Type.ProtectionDoc:
                    Executor.GetCommand<CreateProtectionDocDocumentCommand>().Process(c =>
                        c.Execute(
                            new ProtectionDocDocument
                            {
                                ProtectionDocId = ownerId,
                                DocumentId = documentId
                            }));
                    break;
                case Owner.Type.Contract:
                    Executor.GetCommand<CreateContractDocumentCommand>().Process(c =>
                        c.Execute(
                            new ContractDocument
                            {
                                ContractId = ownerId,
                                DocumentId = documentId
                            }));
                    break;
                default:
                    throw new NotImplementedException();
            }

            var initialPatentWorkflow = Executor.GetQuery<GetInitialDocumentWorkflowQuery>().Process(q =>
                q.Execute(documentId,
                    currentUserId ?? throw new DataNotFoundException(nameof(DicCustomer),
                        DataNotFoundException.OperationType.Read, 0)));
            initialPatentWorkflow.IsCurent = true;
            Executor.GetCommand<CreateDocumentWorkflowCommand>().Process(c => c.Execute(initialPatentWorkflow));
            //patent.CurrentWorkflowId = initialPatentWorkflowId;
            //Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(patent));
            Executor.GetHandler<GenerateRegisterDocumentNumberHandler>().Process<Domain.Entities.Document.Document>(c => c.ExecuteAsync(documentId));
            Executor.GetCommand<CreateUserInputCommand>().Process(c => c.Execute(documentId, userInputDto));

            return documentId;
        }
    }
}
