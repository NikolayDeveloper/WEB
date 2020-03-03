using System;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Enums;
using NetCoreDataAccess.Repository;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    public class GetDocumentTypeEnumByTypeIdQuery : BaseQuery
    {
        public DocumentType Execute(int typeId)
        {
            var typeRepo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();
            var result = typeRepo.GetById(typeId);

            if (result?.ClassificationId == null)
            {
                throw new Exception("Document type enum define error!");
            }

            var classRepo = Uow.GetRepository<DicDocumentClassification>();
            
            var rootClassification = GetRootClassification((int) result.ClassificationId, classRepo);

            switch (rootClassification.Code)
            {
                case DicDocumentClassificationCodes.Incoming:
                    return DocumentType.Incoming;
                case DicDocumentClassificationCodes.Outgoing:
                    return DocumentType.Outgoing;
                case DicDocumentClassificationCodes.Internal:
                    return DocumentType.Internal;
                case DicDocumentClassificationCodes.DocumentRequest:
                    return DocumentType.DocumentRequest;
                default:
                    throw new Exception("Document type enum define error!");
            }
        }

        private DicDocumentClassification GetRootClassification(int classificationId, Repository<DicDocumentClassification> classRepo)
        {
            var dicDocumentClassification = classRepo.GetById(classificationId);

            return dicDocumentClassification.ParentId == null 
                ? dicDocumentClassification 
                : GetRootClassification((int) dicDocumentClassification.ParentId, classRepo);
        }
    }
}
