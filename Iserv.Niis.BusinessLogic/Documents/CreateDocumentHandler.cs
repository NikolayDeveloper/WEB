using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.BusinessLogic.Documents.UserInput;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class CreateDocumentHandler: BaseHandler
    {
        public async Task<int> ExecuteAsync(int ownerId, Owner.Type ownerType, string documentTypeCode, DocumentType type, UserInputDto userInputDto, int? specialUserId = null)
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
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    if (request == null)
                        throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = request.RequestCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    currentUserId = request.CurrentWorkflow.CurrentUserId;
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    if (protectionDoc == null)
                        throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = protectionDoc.ProtectionDocCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    ProtectionDocWorkflow userSpecificWorkflow = null;
                    if (specialUserId.HasValue
                        && Executor
                        .GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>()
                        .Process(q => q.Execute(protectionDoc.Id, specialUserId.Value, out userSpecificWorkflow)) 
                    && userSpecificWorkflow != null)
                    {
                        currentUserId = userSpecificWorkflow.CurrentUserId;
                    }
                    else
                        currentUserId = protectionDoc.CurrentWorkflow.CurrentUserId;

                    break;
                case Owner.Type.Contract:
                    var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    if (contract == null)
                        throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Read, ownerId);
                    addresseeId = contract.ContractCustomers.FirstOrDefault(c =>
                        c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    currentUserId = contract.CurrentWorkflow.CurrentUserId;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var document = new Document
            {
                TypeId = documentType.Id,
                AddresseeId = addresseeId,
                DocumentType = type,
                DateCreate = DateTimeOffset.Now
            };
            var documentId = await Executor.GetCommand<CreateDocumentCommand>().Process(c => c.ExecuteAsync(document));

            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<AddRequestDocumentsCommand>().Process(c =>
                        c.ExecuteAsync(new List<RequestDocument>
                        {
                            new RequestDocument
                            {
                                RequestId = ownerId,
                                DocumentId = documentId
                            }
                        }));
                    break;
                case Owner.Type.ProtectionDoc:
                    await Executor.GetCommand<AddProtectionDocDocumentsCommand>().Process(c =>
                        c.ExecuteAsync(new List<ProtectionDocDocument>
                        {
                            new ProtectionDocDocument
                            {
                                ProtectionDocId = ownerId,
                                DocumentId = documentId
                            }
                        }));
                    break;
                case Owner.Type.Contract:
                    await Executor.GetCommand<AddContractDocumentsCommand>().Process(c =>
                        c.ExecuteAsync(new List<ContractDocument>
                        {
                            new ContractDocument
                            {
                                ContractId = ownerId,
                                DocumentId = documentId
                            }
                        }));
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            var documentWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>().Process(q =>
                q.ExecuteAsync(documentId,
                    currentUserId ?? throw new DataNotFoundException(nameof(DicCustomer),
                        DataNotFoundException.OperationType.Read, 0)));
            await Executor.GetCommand<ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(documentWorkflow));
            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
            await Executor.GetHandler<GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));
            await Executor.GetHandler<GenerateRegisterDocumentNumberHandler>().Process(c => c.ExecuteAsync(documentId));
            await Executor.GetCommand<CreateUserInputCommand>().Process(c => c.ExecuteAsync(documentId, userInputDto));
            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(documentId, specialUserId));

            return documentId;
        }
    }
}
