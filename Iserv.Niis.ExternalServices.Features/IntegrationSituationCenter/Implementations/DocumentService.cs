using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IntegrationDictionaryHelper _integrationDictionaryHelper;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly NiisWebContext _niisWebContext;

        public DocumentService(
            NiisWebContext niisContext,
            IntegrationDictionaryHelper integrationDictionaryHelper,
            DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisContext;
            _integrationDictionaryHelper = integrationDictionaryHelper;
            _dictionaryHelper = dictionaryHelper;
        }

        public void GetRequests(GetDocumentListArgument argument, GetDocumentListResult result)
        {
            if (!argument.DateBegin.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateBegin));
            }
            if (!argument.DateEnd.HasValue)
            {
                throw new ArgumentNullException(nameof(argument.DateEnd));
            }
            var allDocuments = GetRequests(argument.DateBegin.Value, argument.DateEnd.Value).Union(
                GetContracts(argument.DateBegin.Value, argument.DateEnd.Value));
            result.List = allDocuments.ToArray();
        }

        #region PrivateMethods

        private IEnumerable<Document> GetRequests(DateTimeOffset dateBegin, DateTimeOffset dateEnd)
        {
            var documentList = new List<Document>();
            var requests = _niisWebContext.ProtectionDocs
                .Where(x => x.Request.DateUpdate.Date >= dateBegin.Date &&
                            x.Request.DateUpdate.Date <= dateEnd.Date)
                .Select(x => new
                {
                    x.Request.Barcode,
                    x.Request.DateCreate,
                    x.Request.RequestNum,
                    TypeName = x.Request.ProtectionDocType.NameRu,
                    TypeId = x.Request.ProtectionDocTypeId,
                    OnlineStatusName = x.Request.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.NameRu,
                    OnlineStatusId = x.Request.CurrentWorkflow.CurrentStage.OnlineRequisitionStatusId,
                    ProtectionDocId = x.Id,
                    PlanProvDate = DateTime.MinValue, //TODO позже узнать какое поле подставлять
                    UserName = x.Request.CurrentWorkflow.CurrentUser.NameRu
                });
            foreach (var request in requests)
            {
                var resultStatus = string.Empty;
                if (request.OnlineStatusId.HasValue)
                {
                    var onlineStatusCode = _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicOnlineRequisitionStatus), request.OnlineStatusId.Value);
                    resultStatus = _integrationDictionaryHelper.GetResultStatus(onlineStatusCode);
                }

                documentList.Add(new Document
                {
                    UID = request.Barcode,
                    Status = request.OnlineStatusName,
                    User = request.UserName,
                    DocTypeName = request.TypeName,
                    DocDate = request.DateCreate.DateTime,
                    DocNum = request.RequestNum,
                    DocTypeID = request.TypeId,
                    PatentUID = request.ProtectionDocId,
                    PlanProvDate = request.PlanProvDate,
                    ResultStatus = resultStatus
                });
            }
            return documentList;
        }

        private IEnumerable<Document> GetContracts(DateTimeOffset dateBegin, DateTimeOffset dateEnd)
        {
            var documentList = new List<Document>();

            var contracts = _niisWebContext.Contracts
                .Where(x => x.DateUpdate.Date >= dateBegin.Date &&
                            x.DateUpdate.Date <= dateEnd.Date)
                .Select(x => new
                {
                    x.Barcode,
                    x.ContractNum,
                    x.DateCreate,
                    TypeName = x.Type.NameRu,
                    x.TypeId,
                    OnlineStatusName = x.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.NameRu,
                    OnlineStatusId = x.CurrentWorkflow.CurrentStage.OnlineRequisitionStatusId,
                    x.ProtectionDocTypeId,
                    PlanProvDate = DateTime.MinValue, //TODO позже узнать какое поле подставлять
                    UserName = x.CurrentWorkflow.CurrentUser.NameRu
                });
            foreach (var contract in contracts)
            {
                var resultStatus = string.Empty;
                if (contract.OnlineStatusId.HasValue)
                {
                    var onlineStatusCode = _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicOnlineRequisitionStatus), contract.OnlineStatusId.Value);
                    resultStatus = _integrationDictionaryHelper.GetResultStatus(onlineStatusCode);
                }
                documentList.Add(new Document
                {
                    UID = contract.Barcode,
                    Status = contract.OnlineStatusName,
                    User = contract.UserName,
                    DocTypeID = contract.TypeId ?? -1,
                    DocTypeName = contract.TypeName,
                    DocDate = contract.DateCreate.DateTime,
                    DocNum = contract.ContractNum,
                    PatentUID = contract.ProtectionDocTypeId,
                    ResultStatus = resultStatus,
                    PlanProvDate = contract.PlanProvDate
                });
            }
            return documentList;
        }

        #endregion
    }
}