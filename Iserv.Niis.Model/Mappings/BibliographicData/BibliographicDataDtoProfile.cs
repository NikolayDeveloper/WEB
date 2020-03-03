using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Subject;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Model.Mappings.BibliographicData
{
    public class BibliographicDataDtoProfile: Profile
    {
        public BibliographicDataDtoProfile()
        {
            CreateMap<Domain.Entities.Request.Request, BibliographicDataDto>()
                .ForMember(dest => dest.IcgsRequestDtos,
                    opt => opt.MapFrom(src => src.ICGSRequests))
                .ForMember(dest => dest.IcisRequestIds,
                    opt => opt.MapFrom(src => src.ICISRequests.Select(i => i.IcisId)))
                .ForMember(dest => dest.IpcIds,
                    opt => opt.MapFrom(src => src.IPCRequests.Select(i => i.IpcId)))
                .ForMember(dest => dest.MainIpcId,
                    opt => opt.MapFrom(src => src.IPCRequests.Where(i => i.IsMain).Select(i => i.IpcId).FirstOrDefault()))
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
                .ForMember(dest => dest.RequestEarlyRegDtos,
                    opt => opt.MapFrom(src => src.EarlyRegs))
                .ForMember(dest => dest.RequestConventionInfoDtos,
                    opt => opt.MapFrom(src => src.RequestConventionInfos))
                .ForMember(dest => dest.HasProxy,
                    opt => opt.MapFrom(src =>
                        src.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.IN_001_063)));

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, BibliographicDataDto>()
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
                .ForMember(dest => dest.BulletinId,
                    opt => opt.MapFrom(src => src.Bulletins.Any() ? src.Bulletins.FirstOrDefault().Bulletin.Id : 0))
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

            CreateMap<BibliographicDataDto, RequestInfo>()
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

            CreateMap<BibliographicDataDto, ProtectionDocInfo>()
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

                    return new ProtectionDocInfo
                    {
                        ProtectionDocId = arg.Id,
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

            CreateMap<RequestInfo, ProtectionDocInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BreedCountry, opt => opt.Ignore());

            CreateMap<ICGSRequest, ICGSProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ICISRequest, ICISProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<IPCRequest, IPCProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<DicIcfemRequestRelation, DicIcfemProtectionDocRelation>();

            CreateMap<DicIcfemRequestRelation, DicIcfemRequestRelation>()
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore());

            CreateMap<DicIcfemProtectionDocRelation, DicIcfemProtectionDocRelation>()
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore());

            CreateMap<RequestEarlyReg, ProtectionDocEarlyReg>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EarlyRegType, opt => opt.Ignore())
                .ForMember(dest => dest.RegCountry, opt => opt.Ignore());

            CreateMap<RequestConventionInfo, ProtectionDocConventionInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<RequestInfo, RequestInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.BreedCountry, opt => opt.Ignore());

            CreateMap<ICGSRequest, ICGSRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.Icgs, opt => opt.Ignore());

            CreateMap<ICISRequest, ICISRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            CreateMap<IPCRequest, IPCRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            CreateMap<RequestEarlyReg, RequestEarlyReg>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EarlyRegType, opt => opt.Ignore())
                .ForMember(dest => dest.RegCountry, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            CreateMap<RequestConventionInfo, RequestConventionInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            CreateMap<ProtectionDocInfo, ProtectionDocInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore())
                .ForMember(dest => dest.BreedCountry, opt => opt.Ignore());

            CreateMap<ICGSProtectionDoc, ICGSProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore())
                .ForMember(dest => dest.Icgs, opt => opt.Ignore());

            CreateMap<ICISProtectionDoc, ICISProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore());

            CreateMap<IPCProtectionDoc, IPCProtectionDoc>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore());

            CreateMap<ProtectionDocEarlyReg, ProtectionDocEarlyReg>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EarlyRegType, opt => opt.Ignore())
                .ForMember(dest => dest.RegCountry, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore());

            CreateMap<ProtectionDocConventionInfo, ProtectionDocConventionInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore());

            CreateMap<BibliographicDataDto, Domain.Entities.Request.Request>();

            CreateMap<BibliographicDataDto, Domain.Entities.ProtectionDoc.ProtectionDoc>();
        }
    }
}
