using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    /// <summary>
    /// Прием материала(переписка)
    /// </summary>
    public class MessageSendService : IMessageSendService
    {
        private const int PaymentDocumentTypeId = 619;
        private readonly NiisWebContext _niisContext;
        private readonly IntegrationDictionaryHelper _integrationDictionaryHelper;
        private readonly IntegrationDocumentHelper _documentHelper;
        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly IntegrationEgovPayHelper _egovPayHelper;
        private readonly DictionaryHelper _dictionaryHelper;

        public MessageSendService(
            NiisWebContext niisContext, 
            IntegrationDictionaryHelper integrationDictionaryHelper, 
            IntegrationDocumentHelper integrationDocumentHelper, 
            IntegrationAttachFileHelper attachFileHelper, 
            IntegrationEgovPayHelper egovPayHelper, 
            DictionaryHelper dictionaryHelper)
        {
            _niisContext = niisContext;
            _integrationDictionaryHelper = integrationDictionaryHelper;
            _documentHelper = integrationDocumentHelper;
            _attachFileHelper = attachFileHelper;
            _egovPayHelper = egovPayHelper;
            _dictionaryHelper = dictionaryHelper;
        }

        /// <summary>
        /// Проверяет наличие платежного материала и что документ не платежный документ
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public bool HasFileAndNotPayment(MessageSendArgument argument)
        {
            return argument.PaymentFile.Content != null && argument.DocumentType.UID != PaymentDocumentTypeId;
        }

        /// <summary>
        /// Принятие материала
        /// </summary>
        /// <param name="argument">метаданные материала</param>
        /// <returns>Материал</returns>
        public Document CorrespondenceAdd(MessageSendArgument argument)
        {
            var document = DocumentAdd(argument);
            _egovPayHelper.CreatePay(argument.Pay);
            return document;
        }

        /// <summary>
        /// Принятие материала
        /// </summary>
        /// <param name="argument">метаданные материала</param>
        /// <param name="documentId">Id родительского документа</param>
        /// <returns>Материал</returns>
        public Document PaymentDocumentAdd(MessageSendArgument argument, int documentId = 0)
        {
            return HasFileAndNotPayment(argument)
                ? DocumentAdd(argument, true, documentId)
                : null;
        }

        /// <summary>
        /// Создание документа
        /// </summary>
        /// <param name="argument">Метаданные документа</param>
        /// <param name="isPayment">Наличие платежа</param>
        /// <param name="documentId">Id родительского документа</param>
        /// <returns>Материал</returns>
        private Document DocumentAdd(MessageSendArgument argument, bool isPayment = false, int documentId = 0)
        {
            var customer = _integrationDictionaryHelper.GetCustomerIdOrCreateNew(
                Mapper.Map<DicCustomer>(argument, el => el.Items[nameof(DicCustomer.TypeId)] = _dictionaryHelper.GetDictionaryEntityByExternalId(nameof(DicCustomerType), argument.CustomerType.UID).Id));

            var typeId = isPayment
                ? PaymentDocumentTypeId
                : argument.DocumentType.UID;

            var document = new Document(_dictionaryHelper.GetDocumentType(typeId).type)
            {
                TypeId = _dictionaryHelper.GetDictionaryIdByExternalId(nameof(DicDocumentType), typeId),
                AddresseeId = customer.Id,
                DateCreate = DateTimeOffset.Now,
                StatusId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus),DicDocumentStatusCodes.InWork),
                ReceiveTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed)
            };
            _documentHelper.CreateDocument(document);
            
            int? requestBarcode = null;
            if (int.TryParse(argument.OwnerDocumentUId, out var result))
            {
                requestBarcode = result;
            } 
            var requestId = GetRequestId(requestBarcode);

            if (requestId.HasValue)
            {
                _niisContext.RequestsDocuments.Add(new RequestDocument
                {
                    DocumentId = document.Id,
                    RequestId = requestId.Value
                });
                _niisContext.SaveChanges();
            }

            if (isPayment && documentId != 0)
            {
                var parentDocument = _niisContext.Documents.FirstOrDefault(d => d.Id == documentId);
                parentDocument.IsHasPaymentDocument = true;
                _niisContext.DocumentLinks.Add(new DocumentLink
                {
                    ParentDocumentId = documentId,
                    ChildDocumentId = document.Id
                });
                _niisContext.SaveChanges();

                _attachFileHelper.AttachFile(new AttachedFileModel
                    {
                        File = argument.PaymentFile.Content,
                        Length = argument.PaymentFile.Content.Length,
                        IsMain = true,
                        Name = argument.PaymentFile.Name
                    },
                    document);
            }
            else
            { 
                _attachFileHelper.AttachFile(new AttachedFileModel
                {
                    File = argument.File.Content,
                    Length = argument.File.Content.Length,
                    IsMain = true,
                    Name = argument.File.Name
                },
                document);
            }

            return document;
        }

        /// <summary>
        /// Подучить заявку по баркоду
        /// </summary>
        /// <param name="requestBarcode">Баркод</param>
        /// <returns>Идентефикатор</returns>
        private int? GetRequestId(int? requestBarcode)
        {
            var request = _niisContext.Requests.FirstOrDefault(r => r.Barcode == requestBarcode);
            return request?.Id;
        }
    }
}