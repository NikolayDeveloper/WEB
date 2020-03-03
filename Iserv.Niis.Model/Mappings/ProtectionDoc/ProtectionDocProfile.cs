using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Mappings.ProtectionDoc
{
    public class ProtectionDocProfile : Profile
    {
        public ProtectionDocProfile()
        {
            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, ProtectionDocDetailsDto>()
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId.HasValue))
                .ForMember(dest => dest.TypeCode,
                    opt => opt.MapFrom(src => src.Type != null ? src.Type.Code : string.Empty))
                .ForMember(dest => dest.BulletinId,
                    opt => opt.MapFrom(src => src.Bulletins.Any() ? src.Bulletins.FirstOrDefault().Bulletin.Id : 0))
                .ForMember(dest => dest.Subjects,
                    opt => opt.UseValue(new SubjectDto[0]))
                .ForMember(dest => dest.IcgsRequestDtos,
                    opt => opt.MapFrom(src => src.IcgsProtectionDocs))
                .ForMember(dest => dest.IcisRequestIds,
                    opt => opt.MapFrom(src => src.IcisProtectionDocs.Select(i => i.IcisId)))
                .ForMember(dest => dest.IpcIds,
                    opt => opt.MapFrom(src => src.IpcProtectionDocs.Select(i => i.IpcId)))
                .ForMember(dest => dest.MainIpcId,
                    opt => opt.MapFrom(src => src.IpcProtectionDocs.Where(i => i.IsMain).Select(i => i.IpcId).FirstOrDefault()))
                .ForMember(dest => dest.IcfemIds,
                    opt => opt.MapFrom(src => src.Icfems.Select(i => i.DicIcfemId)))
                .ForMember(dest => dest.ColorTzIds,
                    opt => opt.MapFrom(src => src.ColorTzs.Select(i => i.ColorTzId)))
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.Priority))
                .ForMember(dest => dest.IsExhibitPriority,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.IsExhibitPriority))
                .ForMember(dest => dest.Transliteration,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.Transliteration))
                .ForMember(dest => dest.Translation,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.Translation))
                .ForMember(dest => dest.IsColorPerformance,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.IsColorPerformance))
                .ForMember(dest => dest.ProductSpecialProp,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.ProductSpecialProp))
                .ForMember(dest => dest.ProductPlace,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.ProductPlace))
                .ForMember(dest => dest.Genus,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.Genus))
                .ForMember(dest => dest.BreedingNumber,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.BreedingNumber))
                .ForMember(dest => dest.BreedCountryId,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.BreedCountryId))
                .ForMember(dest => dest.BreedCountryNameRu,
                    opt => opt.MapFrom(src => src.ProtectionDocInfo.BreedCountry.NameRu))
                .ForMember(dest => dest.ProtectionDocTypeCode,
                    opt => opt.MapFrom(src => src.Type != null ? src.Type.Code : null))
                .ForMember(dest => dest.RequestEarlyRegDtos,
                    opt => opt.MapFrom(src => src.EarlyRegs))
                .ForMember(dest => dest.RequestConventionInfoDtos,
                    opt => opt.MapFrom(src => src.ProtectionDocConventionInfos))
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(src => src.SubTypeId))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(src => src.RegNumber))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RegDate))
                .ForMember(dest => dest.YearMaintain,
                    opt => opt.MapFrom(src =>
                        src.MaintainDate.HasValue ? src.MaintainDate.Value.Year.ToString() : string.Empty))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image == null
                    ? string.Empty
                    : $"/api/ProtectionDocs/{src.Id}/image?{DateTimeOffset.Now.Ticks}"))
                .ForMember(dest => dest.HasProxy,
                    opt => opt.MapFrom(src =>
                        src.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.IN_001_063)));

            CreateMap<ProtectionDocDetailsDto, Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .ForMember(dest => dest.CurrentWorkflow, src => src.Ignore())
                .ForMember(dest => dest.Workflows, src => src.Ignore());

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, IntellectualPropertyDto>()
                .ForMember(dest => dest.CurrentStageValue,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                        ? src.CurrentWorkflow.CurrentStageId.HasValue
                            ? src.CurrentWorkflow.CurrentStage.NameRu
                            : string.Empty
                        : string.Empty))
                .ForMember(dest => dest.IncomingNumber, src => src.MapFrom(x => x.Request.IncomingNumber))
                .ForMember(dest => dest.IncomingNumberFilial, src => src.MapFrom(x => x.Request.IncomingNumberFilial))
                .ForMember(dest => dest.Addressee, src => src.MapFrom(x =>
                    x.Request.AddresseeId.HasValue
                        ? $"{x.Request.Addressee.NameEn} {x.Request.Addressee.NameRu} {x.Request.Addressee.NameKz}"
                        : string.Empty))
                .ForMember(dest => dest.ProtectionDocTypeId, src => src.MapFrom(x => x.TypeId))
                .ForMember(dest => dest.ProtectionDocTypeValue, src => src.MapFrom(x =>
                    x.TypeId > 0
                        ? x.Type.NameRu
                        : string.Empty))
                .ForMember(dest => dest.ReviewDaysAll,
                    opt => opt.MapFrom(src => (DateTimeOffset.Now - src.DateCreate).Days))
                .ForMember(dest => dest.ReviewDaysStage,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue
                            ? (DateTimeOffset.Now - src.CurrentWorkflow.DateCreate).Days
                            : 0))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.ProtectionDoc))
                .ForMember(dest => dest.TaskType, opt => opt.UseValue("protectiondoc"))
                .ForMember(dest => dest.CanGenerateGosNumber,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId != null &&
                                              src.CurrentWorkflow.CurrentStage.Code.Equals(RouteStageCodes.OD01_1) &&
                                              string.IsNullOrEmpty(src.GosNumber)))
                .ForMember(dest => dest.IpcCodes, opt => opt.MapFrom(src => GetStringForSorting(src)))
                .ForMember(dest => dest.IsIndustrial, opt => opt.MapFrom(src => CanHaveIpc(src)))
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.GosNumber))
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(src => src.RegNumber))
                .ForMember(dest => dest.IsComplete,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue ? src.CurrentWorkflow.IsComplete : false))
                .ForMember(dest => dest.IsActiveProtectionDocument,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue &&
                        new[] {RouteStageCodes.OD05, RouteStageCodes.OD05_01}.Contains(src.CurrentWorkflow.CurrentStage
                            .Code)));

            CreateMap<ProtectionDocWorkflow, ProtectionDocWorkflow>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentStage, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.FromStage, opt => opt.Ignore())
                .ForMember(dest => dest.FromUser, opt => opt.Ignore())
                .ForMember(dest => dest.SecondaryCurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.Route, opt => opt.Ignore());

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentInvoices, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocConventionInfos, opt => opt.Ignore())
                .ForMember(dest => dest.IcgsProtectionDocs, opt => opt.Ignore())
                .ForMember(dest => dest.IpcProtectionDocs, opt => opt.Ignore())
                .ForMember(dest => dest.IcisProtectionDocs, opt => opt.Ignore())
                .ForMember(dest => dest.ColorTzs, opt => opt.Ignore())
                .ForMember(dest => dest.EarlyRegs, opt => opt.Ignore())
                .ForMember(dest => dest.Icfems, opt => opt.Ignore())
                .ForMember(dest => dest.Contracts, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflow, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentWorkflowId, opt => opt.Ignore())
                .ForMember(dest => dest.MainAttachment, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocCustomers, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicantType, opt => opt.Ignore())
                .ForMember(dest => dest.ConventionType, opt => opt.Ignore())
                .ForMember(dest => dest.ExpertSearchSimilarities, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocInfo, opt => opt.Ignore())
                .ForMember(dest => dest.SubType, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.ConsiderationType, opt => opt.Ignore())
                .ForMember(dest => dest.IntellectualProperty, opt => opt.Ignore())
                .ForMember(dest => dest.SupportUser, opt => opt.Ignore())
                .ForMember(dest => dest.BulletinUser, opt => opt.Ignore())
                .ForMember(dest => dest.Workflows, opt => opt.Ignore());
        }

        private bool CanHaveIpc(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc)
        {
            var typesWithIpcCodes = new[]
            {
                DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                DicProtectionDocTypeCodes.RequestTypeInventionCode,
                DicProtectionDocTypeCodes.RequestTypeUsefulModelCode
            };
            return typesWithIpcCodes.Contains(protectionDoc?.Type?.Code);
        }

        private string GetStringForSorting(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc)
        {
            if (CanHaveIpc(protectionDoc))
            {
                return protectionDoc.IpcProtectionDocs.Where(i => i.IsMain).Select(i => i.Ipc.Code).FirstOrDefault() ??
                       protectionDoc.RegNumber;
            }
            return protectionDoc.RegNumber;
        }
    }
}
