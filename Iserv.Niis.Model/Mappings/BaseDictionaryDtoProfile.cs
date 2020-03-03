
using AutoMapper;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Model.Models;

namespace Iserv.Niis.Model.Mappings
{
    public class BaseDictionaryDtoProfile : Profile
    {
        public BaseDictionaryDtoProfile()
        {
            CreateMap<DicAddress, BaseDictionaryDto>();
            CreateMap<DicApplicantType, BaseDictionaryDto>();
            CreateMap<DicColorTZ, BaseDictionaryDto>();
            CreateMap<DicConsiderationType, BaseDictionaryDto>();
            CreateMap<DicContinent, BaseDictionaryDto>();
            CreateMap<DicContractCategory, BaseDictionaryDto>();
            CreateMap<DicContractStatus, BaseDictionaryDto>();
            CreateMap<DicConventionType, BaseDictionaryDto>();
            CreateMap<DicCountry, BaseDictionaryDto>();
            CreateMap<DicCustomerRole, BaseDictionaryDto>();
            CreateMap<DicCustomerType, BaseDictionaryDto>();
            CreateMap<DicDepartment, BaseDictionaryDto>();
            CreateMap<DicDepartmentType, BaseDictionaryDto>();
            CreateMap<DicDivision, BaseDictionaryDto>();
            CreateMap<DicDocumentClassification, BaseDictionaryDto>();
            CreateMap<DicDocumentType, BaseDictionaryDto>();
            CreateMap<DicEarlyRegType, BaseDictionaryDto>();
            CreateMap<DicEntityAccessType, BaseDictionaryDto>();
            CreateMap<Domain.Entities.Dictionaries.DicICFEM, BaseDictionaryDto>();
            CreateMap<DicICGS, BaseDictionaryDto>();
            CreateMap<DicICIS, BaseDictionaryDto>();
            CreateMap<DicIntellectualPropertyStatus, BaseDictionaryDto>();
            CreateMap<DicIPC, BaseDictionaryDto>();
            CreateMap<DicLocation, BaseDictionaryDto>();
            CreateMap<DicLocationType, BaseDictionaryDto>();
            CreateMap<DicLogType, BaseDictionaryDto>();
            CreateMap<DicOnlineRequisitionStatus, BaseDictionaryDto>();
            CreateMap<DicPosition, BaseDictionaryDto>()
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.PositionType.NameRu))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.PositionType.NameEn))
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(src => src.PositionType.NameKz));
            CreateMap<DicProtectionDocBulletinType, BaseDictionaryDto>();
            CreateMap<DicProtectionDocStatus, BaseDictionaryDto>();
            CreateMap<DicRequestStatus, BaseDictionaryDto>();
            CreateMap<DicProtectionDocSubType, BaseDictionaryDto>();
            CreateMap<DicProtectionDocTMType, BaseDictionaryDto>();
            CreateMap<DicProtectionDocType, BaseDictionaryDto>();
            CreateMap<DicReceiveType, BaseDictionaryDto>();
            CreateMap<DicRedefinitionDocumentType, BaseDictionaryDto>();
            CreateMap<DicRedefinitionType, BaseDictionaryDto>();
            CreateMap<DicRoute, BaseDictionaryDto>();
            CreateMap<DicRouteStage, BaseDictionaryDto>();
            CreateMap<DicSendType, BaseDictionaryDto>();
            CreateMap<DicTariff, BaseDictionaryDto>();
            CreateMap<DicPaymentStatus, BaseDictionaryDto>();
            CreateMap<DicDocumentStatus, BaseDictionaryDto>();
            CreateMap<DicPositionType, BaseDictionaryDto>();
            CreateMap<DicSelectionAchieveType, BaseDictionaryDto>();
            CreateMap<DicContractType, BaseDictionaryDto>();
        }
    }
}