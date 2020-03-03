using AutoMapper;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Model.Models;

namespace Iserv.Niis.Model.Mappings
{
    public class SelectOptionDtoProfile : Profile
    {
        public SelectOptionDtoProfile()
        {
            CreateMap<DictionaryEntity<int>, SelectOptionDto>();
            CreateMap<DicAddress, SelectOptionDto>();
            CreateMap<DicApplicantType, SelectOptionDto>();
            CreateMap<DicColorTZ, SelectOptionDto>();
            CreateMap<DicConsiderationType, SelectOptionDto>();
            CreateMap<DicContinent, SelectOptionDto>();
            CreateMap<DicContractCategory, SelectOptionDto>();
            CreateMap<DicContractStatus, SelectOptionDto>();
            CreateMap<DicConventionType, SelectOptionDto>();
            CreateMap<DicCountry, SelectOptionDto>();
            CreateMap<DicCustomerRole, SelectOptionDto>();
            CreateMap<DicCustomer, SelectOptionDto>();
            CreateMap<DicCustomerType, SelectOptionDto>();
            CreateMap<DicDepartment, SelectOptionDto>();
            CreateMap<DicDepartmentType, SelectOptionDto>();
            CreateMap<DicDivision, SelectOptionDto>();
            CreateMap<DicDocumentClassification, SelectOptionDto>();
            CreateMap<DicDocumentType, SelectOptionDto>();
            CreateMap<DicEarlyRegType, SelectOptionDto>();
            CreateMap<DicEntityAccessType, SelectOptionDto>();
            CreateMap<Domain.Entities.Dictionaries.DicICFEM, SelectOptionDto>();
            CreateMap<DicICGS, SelectOptionDto>();
            CreateMap<DicICIS, SelectOptionDto>();
            CreateMap<DicIntellectualPropertyStatus, SelectOptionDto>();
            CreateMap<DicIPC, SelectOptionDto>();
            CreateMap<DicLocation, SelectOptionDto>();
            CreateMap<DicLocationType, SelectOptionDto>();
            CreateMap<DicLogType, SelectOptionDto>();
            CreateMap<DicOnlineRequisitionStatus, SelectOptionDto>();
            CreateMap<DicPosition, SelectOptionDto>()
               .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.PositionType.NameRu));
            CreateMap<DicProtectionDocBulletinType, SelectOptionDto>();
            CreateMap<DicProtectionDocStatus, SelectOptionDto>();
            CreateMap<DicRequestStatus, SelectOptionDto>();
            CreateMap<DicProtectionDocSubType, SelectOptionDto>();
            CreateMap<DicProtectionDocTMType, SelectOptionDto>();
            CreateMap<DicProtectionDocType, SelectOptionDto>();
            CreateMap<DicReceiveType, SelectOptionDto>();
            CreateMap<DicRedefinitionDocumentType, SelectOptionDto>();
            CreateMap<DicRedefinitionType, SelectOptionDto>();
            CreateMap<DicRoute, SelectOptionDto>();
            CreateMap<DicRouteStage, SelectOptionDto>();
            CreateMap<DicSendType, SelectOptionDto>();
            CreateMap<DicTariff, SelectOptionDto>();
            CreateMap<DicPaymentStatus, SelectOptionDto>();
            CreateMap<DicDocumentStatus, SelectOptionDto>();
            CreateMap<DicPositionType, SelectOptionDto>();
            CreateMap<DicEventType, SelectOptionDto>();
            CreateMap<Domain.Entities.Dictionaries.DicDetailICGS, SelectOptionDto>();
            CreateMap<DicSelectionAchieveType, SelectOptionDto>();
            CreateMap<DicContractType, SelectOptionDto>();
        }
    }
}