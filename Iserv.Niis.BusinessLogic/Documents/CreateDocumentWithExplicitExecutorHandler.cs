using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.BusinessLogic.Documents.UserInput;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.BusinessLogic.Documents
{
    /// <summary>
    /// Обработчик, который создает и возврашает документ по коду 
    /// типа документа для родительской сущности с указанным исполнителем.
    /// </summary>
    public class CreateDocumentWithExplicitExecutorHandler : BaseHandler
    {
        /// <summary>
        /// Выполнение обработчика.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="documentTypeCode">Код типа документа.</param>
        /// <param name="type">Тип документа.</param>
        /// <param name="userInput">Пользовательский ввод.</param>
        /// <param name="executorId">Идентификатор исполнителя.</param>
        /// <returns>Идентификатор созданного документа.</returns>
        public async Task<int> ExecuteAsync(
            int ownerId,
            Owner.Type ownerType,
            string documentTypeCode,
            DocumentType type,
            UserInputDto userInput,
            int executorId)
        {
            DicDocumentType documentType = GetDocumentTypeWithValidation(documentTypeCode);

            int? addresseeId = await GetAddresseeId(ownerId, ownerType);

            Document document = new Document
            {
                TypeId = documentType.Id,
                AddresseeId = addresseeId,
                DocumentType = type,
                DateCreate = DateTimeOffset.Now
            };

            int documentId = await Executor
                .GetCommand<CreateDocumentCommand>()
                .Process(command => command.ExecuteAsync(document));

            await AddDocumentToOwner(ownerId, ownerType, documentId);

            DocumentWorkflow documentWorkflow = await Executor
                .GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(query => query.ExecuteAsync(documentId, executorId));

            await Executor
                .GetCommand<ApplyDocumentWorkflowCommand>()
                .Process(command => command.ExecuteAsync(documentWorkflow));

            await Executor
                .GetCommand<UpdateDocumentCommand>()
                .Process(command => command.Execute(document));

            await Executor
                .GetHandler<GenerateDocumentBarcodeHandler>()
                .Process(handler => handler.ExecuteAsync(documentId));

            await Executor
                .GetHandler<GenerateRegisterDocumentNumberHandler>()
                .Process(handler => handler.ExecuteAsync(documentId));

            await Executor
                .GetCommand<CreateUserInputCommand>()
                .Process(command => command.ExecuteAsync(documentId, userInput));

            await Executor
                .GetHandler<ProcessWorkflowByDocumentIdHandler>()
                .Process(handler => handler.ExecuteAsync(documentId));

            return document.Id;
        }

        /// <summary>
        /// Получает идентификатор адресата для переписки по идентификатору и типу родительской сущности.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <returns>Идентификатор адресата для переписки.</returns>
        private async Task<int?> GetAddresseeId(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    return await GetAddresseeIdByRequestId(ownerId);
                case Owner.Type.ProtectionDoc:
                    return await GetAddresseeIdByProtectionDocId(ownerId);
                case Owner.Type.Contract:
                    return await GetAddresseeIdByContractId(ownerId);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает идентификатор адресата для переписки по идентификатору заявки.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Иденификатор адресата для переписки.</returns>
        private async Task<int?> GetAddresseeIdByRequestId(int requestId)
        {
            Request request = await Executor
                .GetQuery<GetRequestByIdWithCustomersAndCustomerRolesQuery>()
                .Process(query => query.ExecuteAsync(requestId));

            if(request is null)
            {
                throw new DataNotFoundException(nameof(Request),
                    DataNotFoundException.OperationType.Read, requestId);
            }

            RequestCustomer addressee = request.RequestCustomers
                 .FirstOrDefault(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

            return addressee?.CustomerId;
        }

        /// <summary>
        /// Получает идентификатор адресата для переписки по идентификатору охранного документа.
        /// </summary>
        /// <param name="protectionDocId">Идентификатор охранного документа.</param>
        /// <returns>Идентификатор адресата для переписки.</returns>
        private async Task<int?> GetAddresseeIdByProtectionDocId(int protectionDocId)
        {
            ProtectionDoc protectionDoc = await Executor
                .GetQuery<GetProtectionDocByIdWithCustomersAndCustomerRolesQuery>()
                .Process(query => query.ExecuteAsync(protectionDocId));

            if(protectionDoc is null)
            {
                throw new DataNotFoundException(nameof(ProtectionDoc),
                    DataNotFoundException.OperationType.Read, protectionDocId);
            }

            ProtectionDocCustomer addressee = protectionDoc.ProtectionDocCustomers
                .FirstOrDefault(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

            return addressee?.CustomerId;
        }

        /// <summary>
        /// Получает идентификатор адресата для переписки по идентификатору договора.
        /// </summary>
        /// <param name="contractId">Идентификатор договора.</param>
        /// <returns>Идентификатор адресата для переписки.</returns>
        private async Task<int?> GetAddresseeIdByContractId(int contractId)
        {
            Contract contract = await Executor
                .GetQuery<GetContractByIdWithCustomersAndCustomerRolesQuery>()
                .Process(query => query.ExecuteAsync(contractId));

            if(contract is null)
            {
                throw new DataNotFoundException(nameof(Contract),
                    DataNotFoundException.OperationType.Read, contractId);
            }

            ContractCustomer addressee = contract.ContractCustomers
                .FirstOrDefault(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

            return addressee?.CustomerId;
        }


        /// <summary>
        /// Добавляет документ к родительской сущности.
        /// </summary>
        /// <param name="ownerId">Иденификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Асинхронная операция.</returns>
        private async Task AddDocumentToOwner(int ownerId, Owner.Type ownerType, int documentId)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await AddDocumentToRequest(ownerId, documentId);
                    break;

                case Owner.Type.ProtectionDoc:
                    await AddDocumentToProtectionDoc(ownerId, documentId);
                    break;

                case Owner.Type.Contract:
                    await AddDocumentToContract(ownerId, documentId);
                    break;
            }            
        }

        /// <summary>
        /// Добавляет документа к заявке.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Асинхронная операция.</returns>
        private async Task AddDocumentToRequest(int requestId, int documentId)
        {
            List<RequestDocument> documents = new List<RequestDocument>
            {
                new RequestDocument
                {
                    RequestId = requestId,
                    DocumentId = documentId
                }
            };

            await Executor
                .GetCommand<AddRequestDocumentsCommand>()
                .Process(command => command.ExecuteAsync(documents));
        }

        /// <summary>
        /// Добавляет документ к охранному документу.
        /// </summary>
        /// <param name="protectionDocId">Идентификатор охранного документа.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Асинхронная операция.</returns>
        private async Task AddDocumentToProtectionDoc(int protectionDocId, int documentId)
        {
            List<ProtectionDocDocument> documents = new List<ProtectionDocDocument>
            {
                new ProtectionDocDocument
                {
                    ProtectionDocId = protectionDocId,
                    DocumentId = documentId
                }
            };

            await Executor
                .GetCommand<AddProtectionDocDocumentsCommand>()
                .Process(command => command.ExecuteAsync(documents));
        }

        /// <summary>
        /// Добавляет докумеент к договору.
        /// </summary>
        /// <param name="contractId">Идентификатор договора.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Асинхронная операция.</returns>
        private async Task AddDocumentToContract(int contractId, int documentId)
        {
            List<ContractDocument> documents = new List<ContractDocument>
            {
                new ContractDocument
                {
                    ContractId = contractId,
                    DocumentId = documentId
                }
            };

            await Executor
                .GetCommand<AddContractDocumentsCommand>()
                .Process(command => command.ExecuteAsync(documents));
        }


        /// <summary>
        /// Возвращает тип документа по его коду.
        /// </summary>
        /// <param name="code">Код типа документа.</param>
        /// <returns>Тип документа.</returns>
        /// <exception cref="DataNotFoundException">Выкидывается если тип документа не найден.</exception>
        private DicDocumentType GetDocumentTypeWithValidation(string code)
        {
            var documentType = Executor
                .GetQuery<GetDicDocumentTypeByCodeQuery>()
                .Process(query => query.Execute(code));

            if (documentType is null)
            {
                throw new DataNotFoundException(nameof(DicDocumentType),
                    DataNotFoundException.OperationType.Read, code);
            }

            return documentType;
        }
    }
}