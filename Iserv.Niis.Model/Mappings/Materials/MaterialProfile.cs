using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.DocumentRequest;
using Iserv.Niis.Model.Models.Material.Incoming;
using Iserv.Niis.Model.Models.Material.Internal;
using Iserv.Niis.Model.Models.Material.Outgoing;
using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;

namespace Iserv.Niis.Model.Mappings.Materials
{
    public class MaterialProfile : Profile
    {
        private static readonly string[] InternalReportCodes =
        {
            "IZ_OTCHET_POISK_IZ",
            "IZ_OTCHET_POISK_P"
        };

        public MaterialProfile()
        {

            CreateMap<AttachmentDto, Document>();

            CreateMap<Attachment, AttachmentDto>();

            CreateMap<RequestDocument, MaterialOwnerDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.Request.ProtectionDocTypeId))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Request));

            CreateMap<Domain.Entities.Contract.ContractDocument, MaterialOwnerDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.ContractId))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.Contract.ProtectionDocTypeId))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Contract));

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDocDocument, MaterialOwnerDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.ProtectionDocId))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.ProtectionDoc.TypeId))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.ProtectionDoc));

            CreateMap<Document, MaterialItemDto>()
                .ForMember(dest => dest.Initiator,
                    opt => opt.MapFrom(src =>
                        src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst) != null
                            ? src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst).CurrentUser.NameRu
                            : string.Empty))
                .ForMember(dest => dest.Executor,
                    //Заменил на всех текущих исполнителей
                    opt => opt.MapFrom(src => src.CurrentWorkflows.Any()
                        ? string.Join(", ", src.CurrentWorkflows.Where(d => d.CurrentUserId.HasValue).Select(d => d.CurrentUser.NameRu))
                        : string.Empty))
                .ForMember(dest => dest.CanDownload,
                    opt => opt.MapFrom(src =>
                        src.DocumentType == DocumentType.Incoming
                            ? src.MainAttachmentId.HasValue
                            : src.DocumentType == DocumentType.Outgoing || src.DocumentType == DocumentType.Internal || src.DocumentType == DocumentType.DocumentRequest && (src.Type.TemplateFileId.HasValue || src.MainAttachmentId.HasValue)))
                .ForMember(dest => dest.HasTemplate,
                    opt => opt.MapFrom(src =>
                        src.DocumentType != DocumentType.Incoming &&
                        (src.DocumentType == DocumentType.Outgoing || src.DocumentType == DocumentType.DocumentRequest || src.DocumentType == DocumentType.Internal && src.Type.TemplateFileId.HasValue)))
                //Заменил на все текущие WF
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.CurrentWorkflows.Any()
                    ? string.Join(", ", src.CurrentWorkflows.Where(d => d.CurrentStageId.HasValue).Select(d => d.CurrentStage.NameRu))
                    : string.Empty));

            CreateMap<Domain.Entities.Request.Request, MaterialItemDto>()
                .ForMember(dest => dest.Initiator,
                    opt => opt.MapFrom(src =>
                        src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst) != null
                            ? src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst).CurrentUser.NameRu
                            : string.Empty))
                .ForMember(dest => dest.Executor,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                        ? src.CurrentWorkflow.CurrentUserId.HasValue
                            ? src.CurrentWorkflow.CurrentUser.NameRu
                            : string.Empty
                        : string.Empty))
                .ForMember(dest => dest.CanDownload, opt => opt.UseValue(false))
                .ForMember(dest => dest.HasTemplate, opt => opt.UseValue(false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                    ? src.CurrentWorkflow.CurrentStageId.HasValue
                        ? src.CurrentWorkflow.CurrentStage.NameRu
                        : string.Empty
                    : string.Empty))
                .ForMember(dest => dest.DocumentNum, opt => opt.MapFrom(src => src.RequestNum))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.ProtectionDocType.NameRu))
                .ForMember(dest => dest.DocumentType, opt => opt.UseValue(DocumentType.Request));

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, MaterialItemDto>()
                .ForMember(dest => dest.Initiator,
                    opt => opt.MapFrom(src =>
                        src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst) != null
                            ? src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst).CurrentUser.NameRu
                            : string.Empty))
                .ForMember(dest => dest.Executor,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                        ? src.CurrentWorkflow.CurrentUserId.HasValue
                            ? src.CurrentWorkflow.CurrentUser.NameRu
                            : string.Empty
                        : string.Empty))
                .ForMember(dest => dest.CanDownload, opt => opt.UseValue(false))
                .ForMember(dest => dest.HasTemplate, opt => opt.UseValue(false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                    ? src.CurrentWorkflow.CurrentStageId.HasValue
                        ? src.CurrentWorkflow.CurrentStage.NameRu
                        : string.Empty
                    : string.Empty))
                .ForMember(dest => dest.DocumentNum, opt => opt.MapFrom(src => src.GosNumber))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Type.NameRu))
                .ForMember(dest => dest.DocumentType, opt => opt.UseValue(DocumentType.ProtectionDoc));

            CreateMap<Document, MaterialDetailDto>()
                .ForMember(dest => dest.HasSecondaryAttachment, opt => opt.MapFrom(src => src.AdditionalAttachments.Any(a => a.IsMain == false)))
                .ForMember(dest => dest.Attachment, opt => opt.MapFrom(src => src.MainAttachment))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Type.Code));

            CreateMap<MaterialDetailDto, Document>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? 0))
                .ForMember(dest => dest.Requests,
                    opt => opt.MapFrom(src =>
                        src.Owners.Where(o => o.OwnerType == Owner.Type.Request)
                            .Select(o => new RequestDocument { RequestId = o.OwnerId })))
                .ForMember(dest => dest.Contracts,
                    opt => opt.MapFrom(src =>
                        src.Owners.Where(o => o.OwnerType == Owner.Type.Contract)
                            .Select(o => new ContractDocument { ContractId = o.OwnerId })))
                .ForMember(dest => dest.ProtectionDocs,
                    opt => opt.MapFrom(src =>
                        src.Owners.Where(o => o.OwnerType == Owner.Type.ProtectionDoc)
                            .Select(o => new ProtectionDocDocument { ProtectionDocId = o.OwnerId })));

            CreateMap<MaterialIncomingDetailDto, Document>()
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.DocumentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentParentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflows, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore());

            CreateMap<MaterialOutgoingDetailDto, Document>()
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.DocumentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentParentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflows, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore());

            CreateMap<MaterialInternalDetailDto, Document>()
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.DocumentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentParentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflows, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore());

            CreateMap<MaterialDocumentRequestDetailDto, Document>()
                .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.DocumentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentParentLinks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflows, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore());

            CreateMap<DocumentCommentDto, DocumentComment>();

            CreateMap<DocumentComment, DocumentCommentDto>()
                .ForMember(dest => dest.AuthorInitials,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.NameRu : string.Empty));

            CreateMap<DocumentLinkDto, DocumentLink>();

            CreateMap<DocumentLink, DocumentLinkDto>()
                .ForMember(dest => dest.ParentDocumentTypeName,
                    opt => opt.MapFrom(src => src.ParentDocument.Type.NameRu))
                .ForMember(dest => dest.ParentDocumentType,
                    opt => opt.MapFrom(src => src.ParentDocument.DocumentType))
                .ForMember(dest => dest.ChildDocumentType,
                    opt => opt.MapFrom(src => src.ChildDocument.DocumentType))
                .ForMember(dest => dest.ParentDocumentNumber,
                    opt => opt.MapFrom(src => src.ParentDocument.DocumentType == DocumentType.Incoming
                    ? src.ParentDocument.IncomingNumber
                    : src.ParentDocument.DocumentType == DocumentType.Outgoing
                        ? src.ParentDocument.OutgoingNumber
                        : src.ParentDocument.Barcode.ToString()))
                .ForMember(dest => dest.ChildDocumentTypeName,
                    opt => opt.MapFrom(src => src.ChildDocument.Type.NameRu))
                .ForMember(dest => dest.ChildDocumentNumber,
                    opt => opt.MapFrom(src => src.ChildDocument.DocumentType == DocumentType.Incoming
                    ? src.ChildDocument.IncomingNumber
                    : src.ChildDocument.DocumentType == DocumentType.Outgoing
                        ? src.ChildDocument.OutgoingNumber
                        : src.ChildDocument.Barcode.ToString()));

            CreateMap<Document, MaterialOutgoingDetailDto>()
                .ForMember(dest => dest.WorkflowDtos,
                    opt => opt.MapFrom(src => src.Workflows.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.IncomingAnswerNumber,
                    opt => opt.MapFrom(src => src.IncomingAnswer != null ? src.IncomingAnswer.IncomingNumber + " " + src.IncomingAnswer.Type.NameRu : string.Empty))
                .ForMember(dest => dest.StatusCode,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.NameRu : string.Empty))
                .ForMember(dest => dest.DocumentDate,
                    opt => opt.MapFrom(src => src.SendingDate))
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId != null))
                .ForMember(dest => dest.AddresseeXin,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Xin : string.Empty))
                .ForMember(dest => dest.AddresseeAddress,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Address : string.Empty))
                .ForMember(dest => dest.AddresseeCity,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.City : string.Empty))
                .ForMember(dest => dest.AddresseeOblast,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Oblast : string.Empty))
                .ForMember(dest => dest.AddresseeRepublic,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Republic : string.Empty))
                .ForMember(dest => dest.AddresseeRegion,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Region : string.Empty))
                .ForMember(dest => dest.AddresseeStreet,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Street : string.Empty))
                .ForMember(dest => dest.AddresseeNameRu,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.NameRu : string.Empty))
                .ForMember(dest => dest.PaymentInvoiceCode,
                    opt => opt.MapFrom(src => src.PaymentInvoice != null ? src.PaymentInvoice.Tariff.Code : string.Empty))
                .ForMember(dest => dest.AddresseeEmail,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Email : string.Empty))
                .ForMember(dest => dest.Apartment,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Apartment : string.Empty))
                .ForMember(dest => dest.Attachment,
                    opt => opt.MapFrom(src => src.MainAttachment))
                .ForMember(dest => dest.CommentDtos,
                    opt => opt.MapFrom(src => src.Comments.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentParentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentParentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Type.Code))
                .ForMember(dest => dest.OwnerType,
                    opt => opt.MapFrom(src =>
                        src.Requests.Any()
                            ? Owner.Type.Request
                            : src.Contracts.Any()
                                ? Owner.Type.Contract
                                : src.ProtectionDocs.Any()
                                    ? Owner.Type.ProtectionDoc
                                    : Owner.Type.None));

            CreateMap<Document, MaterialInternalDetailDto>()
                .ForMember(dest => dest.WorkflowDtos,
                    opt => opt.MapFrom(src => src.Workflows.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.CommentDtos,
                    opt => opt.MapFrom(src => src.Comments.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentParentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentParentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Type.Code))
                .ForMember(dest => dest.DocumentDate,
                    opt => opt.MapFrom(src => src.SendingDate))
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId != null))
                .ForMember(dest => dest.StatusNameRu,
                    opt => opt.MapFrom(src => src.Status != null ? src.Status.NameRu : string.Empty))
                .ForMember(dest => dest.HasSecondaryAttachment,
                    opt => opt.MapFrom(src => src.AdditionalAttachments.Any(a => a.IsMain == false)))
                .ForMember(dest => dest.Attachment,
                    opt => opt.MapFrom(src => src.MainAttachment))
                .ForMember(dest => dest.HasTemplate,
                    opt => opt.MapFrom(src => src.Type.TemplateFileId.HasValue))
                .ForMember(dest => dest.OwnerType,
                    opt => opt.MapFrom(src =>
                        src.Requests.Any()
                            ? Owner.Type.Request
                            : src.Contracts.Any()
                                ? Owner.Type.Contract
                                : src.ProtectionDocs.Any()
                                    ? Owner.Type.ProtectionDoc
                                    : Owner.Type.None));

            CreateMap<Document, MaterialDocumentRequestDetailDto>()
                .ForMember(dest => dest.WorkflowDtos,
                    opt => opt.MapFrom(src => src.Workflows.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.CommentDtos,
                    opt => opt.MapFrom(src => src.Comments.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentParentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentParentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Type.Code))
                .ForMember(dest => dest.DocumentDate,
                    opt => opt.MapFrom(src => src.SendingDate))
                .ForMember(dest => dest.HasSecondaryAttachment,
                    opt => opt.MapFrom(src => src.AdditionalAttachments.Any(a => a.IsMain == false)))
                .ForMember(dest => dest.Attachment,
                    opt => opt.MapFrom(src => src.MainAttachment))
                .ForMember(dest => dest.HasTemplate,
                    opt => opt.MapFrom(src => src.Type.TemplateFileId.HasValue))
                .ForMember(dest => dest.OwnerType,
                    opt => opt.MapFrom(src =>
                        src.Requests.Any()
                            ? Owner.Type.Request
                            : src.Contracts.Any()
                                ? Owner.Type.Contract
                                : src.ProtectionDocs.Any()
                                    ? Owner.Type.ProtectionDoc
                                    : Owner.Type.None));

            CreateMap<Document, MaterialIncomingDetailDto>()
                .ForMember(dest => dest.AddresseeXin,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Xin : string.Empty))
                .ForMember(dest => dest.AddresseeCity,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.City : string.Empty))
                .ForMember(dest => dest.AddresseeOblast,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Oblast : string.Empty))
                .ForMember(dest => dest.AddresseeAddress,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Address : string.Empty))
                .ForMember(dest => dest.AddresseeRepublic,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Republic : string.Empty))
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId != null))
                .ForMember(dest => dest.AddresseeRegion,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Region : string.Empty))
                .ForMember(dest => dest.AddresseeStreet,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Street : string.Empty))
                .ForMember(dest => dest.AddresseeNameRu,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.NameRu : string.Empty))
                .ForMember(dest => dest.Apartment,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Apartment : string.Empty))
                .ForMember(dest => dest.WorkflowDtos,
                    opt => opt.MapFrom(src => src.Workflows.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.CommentDtos,
                    opt => opt.MapFrom(src => src.Comments.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.DocumentParentLinkDtos,
                    opt => opt.MapFrom(src => src.DocumentParentLinks.OrderByDescending(w => w.DateCreate)))
                .ForMember(dest => dest.Attachment,
                    opt => opt.MapFrom(src => src.MainAttachment))
                .ForMember(dest => dest.DocumentDate,
                    opt => opt.MapFrom(src => src.SendingDate))
                .ForMember(dest => dest.OwnerType,
                    opt => opt.MapFrom(src =>
                        src.Requests.Any()
                            ? Owner.Type.Request
                            : src.Contracts.Any()
                                ? Owner.Type.Contract
                                : src.ProtectionDocs.Any()
                                    ? Owner.Type.ProtectionDoc
                                    : Owner.Type.None));

            CreateMap<Document, MaterialTaskDto>()
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Type.NameRu))
                .ForMember(dest => dest.WasScanned, opt => opt.MapFrom(src => src.MainAttachmentId != null))

                 //Перенеслось в MaterialsController для разделения WF именно для текущего пользователя
                 //.ForMember(dest => dest.CurrentStageUser,
                 //    opt => opt.MapFrom(src =>
                 //        src.CurrentWorkflowId != null
                 //            ? (src.CurrentWorkflow.CurrentUserId != null
                 //                ? src.CurrentWorkflow.CurrentUser.NameRu
                 //                : null)
                 //            : null))
                 .ForMember(dest => dest.DisplayNumber,
                    opt => opt.MapFrom(src => src.DocumentType == DocumentType.Incoming
                    ? src.IncomingNumber
                    : src.DocumentType == DocumentType.Outgoing
                        ? src.OutgoingNumber
                        : src.Barcode.ToString()))

                .ForMember(dest => dest.Creator,
                    opt => opt.MapFrom(src =>
                        src.Workflows.Any()
                            ? src.Workflows.FirstOrDefault(w => w.CurrentStage.IsFirst).CurrentUser.NameRu
                            : null))
                .ForMember(dest => dest.CanDownload,
                    opt => opt.MapFrom(src =>
                        src.MainAttachmentId.HasValue
                        && (src.DocumentType == DocumentType.Incoming || src.DocumentType == DocumentType.Internal || src.DocumentType == DocumentType.DocumentRequest) || src.DocumentType == DocumentType.Outgoing
                    ))
                .ForMember(dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId != null ? src.Status.NameRu : string.Empty));

            CreateMap<DocumentWorkflow, MaterialWorkflowDto>()
                .ForMember(dest => dest.IsSigned, opt => opt.MapFrom(src => src.DocumentUserSignature != null))
                .ForMember(dest => dest.CurrentUserDepartmentId, opt => opt.MapFrom(src => src.CurrentUser != null ? src.CurrentUser.DepartmentId : 0));
            CreateMap<MaterialWorkflowDto, DocumentWorkflow>();

            CreateMap<Document, Document>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Workflows, opt => opt.Ignore())
                .ForMember(dest => dest.Addressee, opt => opt.Ignore())
                .ForMember(dest => dest.Division, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiveType, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflows, opt => opt.Ignore())
                .ForMember(dest => dest.Requests, opt => opt.Ignore());

            CreateMap<DocumentWorkflow, DocumentWorkflow>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentUserSignature, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentStage, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.FromStage, opt => opt.Ignore())
                .ForMember(dest => dest.FromUser, opt => opt.Ignore())
                .ForMember(dest => dest.Route, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());
        }

        private IQueryable<MaterialOwnerDto> MapOwners(IQueryable<object> owners)
        {
            return owners.ProjectTo<MaterialOwnerDto>();
        }
    }
}