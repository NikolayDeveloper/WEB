using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Mappings.Request
{
    public class RequestDetailDtoProfile : Profile
    {
        public RequestDetailDtoProfile()
        {
            CreateMap<Domain.Entities.Request.Request, RequestDetailDto>()
                .ForMember(dest => dest.WasScanned,
                    opt => opt.MapFrom(src => src.MainAttachmentId.HasValue))
                .ForMember(dest => dest.ParentRequestIds,
                    opt => opt.MapFrom(src => src.ParentRequests.Select(p => p.ParentId)))
                .ForMember(dest => dest.ChildsRequestIds,
                    opt => opt.MapFrom(src => src.ChildsRequests.Select(c => c.ChildId)))
                .ForMember(dest => dest.IcgsRequestDtos,
                    opt => opt.MapFrom(src => src.ICGSRequests))
                .ForMember(dest => dest.IcisRequestIds,
                    opt => opt.MapFrom(src => src.ICISRequests.Select(i => i.IcisId)))
                .ForMember(dest => dest.IpcIds,
                    opt => opt.MapFrom(src => src.IPCRequests.Select(i => i.IpcId)))
                .ForMember(dest => dest.MainIpcId,
                    opt => opt.MapFrom(
                        src => src.IPCRequests.Where(i => i.IsMain).Select(i => i.IpcId).FirstOrDefault()))
                .ForMember(dest => dest.IcfemIds,
                    opt => opt.MapFrom(src => src.Icfems.Select(i => i.DicIcfemId)))
                .ForMember(dest => dest.ColorTzIds,
                    opt => opt.MapFrom(src => src.ColorTzs.Select(i => i.ColorTzId)))
                .ForMember(dest => dest.InvoiceDtos,
                    opt => opt.UseValue(new PaymentInvoiceDto[0]))
                .ForMember(dest => dest.Subjects,
                    opt => opt.UseValue(new SubjectDto[0]))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image == null
                    ? string.Empty
                    : $"/api/requests/{src.Id}/image?{DateTimeOffset.Now.Ticks}"))
                .ForMember(dest => dest.MediaUrl,
                    opt => opt.MapFrom(src => $"/api/requests/{src.Id}/media?{DateTimeOffset.Now.Ticks}"))
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src => src.RequestInfo.Priority))
                .ForMember(dest => dest.IsExhibitPriority,
                    opt => opt.MapFrom(src => src.RequestInfo.IsExhibitPriority))
                .ForMember(dest => dest.Transliteration,
                    opt => opt.MapFrom(src => src.RequestInfo.Transliteration))
                .ForMember(dest => dest.Translation,
                    opt => opt.MapFrom(src => src.RequestInfo.Translation))
                .ForMember(dest => dest.IsColorPerformance,
                    opt => opt.MapFrom(src => src.RequestInfo.IsColorPerformance))
                .ForMember(dest => dest.ProductSpecialProp,
                    opt => opt.MapFrom(src => src.RequestInfo.ProductSpecialProp))
                .ForMember(dest => dest.ProductPlace,
                    opt => opt.MapFrom(src => src.RequestInfo.ProductPlace))
                .ForMember(dest => dest.Genus,
                    opt => opt.MapFrom(src => src.RequestInfo.Genus))
                .ForMember(dest => dest.BreedingNumber,
                    opt => opt.MapFrom(src => src.RequestInfo.BreedingNumber))
                .ForMember(dest => dest.BreedCountryId,
                    opt => opt.MapFrom(src => src.RequestInfo.BreedCountryId))
                .ForMember(dest => dest.BreedCountryNameRu,
                    opt => opt.MapFrom(src => src.RequestInfo.BreedCountry.NameRu))
                .ForMember(dest => dest.ProtectionDocTypeCode,
                    opt => opt.MapFrom(src => src.ProtectionDocType != null ? src.ProtectionDocType.Code : null))
                .ForMember(dest => dest.RequestTypeCode,
                    opt => opt.MapFrom(src => src.RequestType != null ? src.RequestType.Code : null))
                .ForMember(dest => dest.RequestEarlyRegDtos,
                    opt => opt.MapFrom(src => src.EarlyRegs))
                .ForMember(dest => dest.RequestConventionInfoDtos,
                    opt => opt.MapFrom(src => src.RequestConventionInfos))
                .ForMember(dest => dest.DepartmentNameRu,
                    opt => opt.MapFrom(src => src.Department.NameRu))
                .ForMember(dest => dest.HasProxy,
                    opt => opt.MapFrom(src =>
                        src.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.IN_001_063)))
                .ForMember(dest => dest.AddresseeAddress,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Address : string.Empty))
                .ForMember(dest => dest.AddresseeShortAddress,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.ShortAddress : string.Empty))
                .ForMember(dest => dest.Republic,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Republic : string.Empty))
                .ForMember(dest => dest.Oblast,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Oblast : string.Empty))
                .ForMember(dest => dest.Region,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Region : string.Empty))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.City : string.Empty))
                .ForMember(dest => dest.Street,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Street : string.Empty))
                .ForMember(dest => dest.Apartment,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Apartment : string.Empty))
                .ForMember(dest => dest.AddresseeEmail,
                    opt => opt.MapFrom(src => src.Addressee != null ? src.Addressee.Email : string.Empty))
                .ForMember(dest => dest.SpeciesTrademarkCode,
                    opt => opt.MapFrom(src => src.SpeciesTradeMarkId.HasValue ? src.SpeciesTradeMark.Code : string.Empty))
                .ForMember(dest => dest.IsHasMaterialExpertOpinionWithOugoingNumber,
                    opt => opt.MapFrom(src => src.Documents.Any(rd =>
                        rd.Document.Type.Code == DicDocumentTypeCodes.RegisterNmptExpertConclusion &&
                        string.IsNullOrWhiteSpace(rd.Document.OutgoingNumber) == false)));

            CreateMap<RequestDetailDto, Domain.Entities.Request.Request>()
                .ForMember(dest => dest.CurrentWorkflowId, src => src.Ignore())
                .ForMember(dest => dest.CurrentWorkflow, src => src.Ignore())
                .ForMember(dest => dest.RequestType, opt => opt.Ignore())
                .ForMember(dest => dest.Addressee, opt => opt.Ignore())
                .ForMember(dest => dest.Workflows, src => src.Ignore())
				.ForMember(dest => dest.User, src => src.Ignore());

            CreateMap<RequestDetailDto, RequestInfo>()
                .ConvertUsing(arg =>
                {
                    if (string.IsNullOrWhiteSpace(arg.Priority) &&
                        !arg.IsExhibitPriority.HasValue &&
                        string.IsNullOrWhiteSpace(arg.Transliteration) &&
                        string.IsNullOrWhiteSpace(arg.Translation) &&
                        !arg.IsColorPerformance.HasValue &&
                        string.IsNullOrWhiteSpace(arg.ProductSpecialProp) &&
                        string.IsNullOrWhiteSpace(arg.ProductPlace) &&
                        string.IsNullOrWhiteSpace(arg.Genus) &&
                        string.IsNullOrWhiteSpace(arg.BreedingNumber) &&
                        !arg.BreedCountryId.HasValue)
                        return null;

                    return new RequestInfo
                    {
                        RequestId = arg.Id,
                        Priority = arg.Priority,
                        IsExhibitPriority = arg.IsExhibitPriority,
                        Transliteration = arg.Transliteration,
                        Translation = arg.Translation,
                        IsColorPerformance = arg.IsColorPerformance,
                        ProductSpecialProp = arg.ProductSpecialProp,
                        ProductPlace = arg.ProductPlace,
                        Genus = arg.Genus,
                        BreedingNumber = arg.BreedingNumber,
                        BreedCountryId = arg.BreedCountryId,
                    };
                });

            CreateMap<RequestWorkflow, RequestWorkflow>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentStage, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.FromStage, opt => opt.Ignore())
                .ForMember(dest => dest.FromUser, opt => opt.Ignore())
                .ForMember(dest => dest.Route, opt => opt.Ignore());
        }
    }
}