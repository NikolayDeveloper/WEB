using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Subject;
using Microsoft.EntityFrameworkCore.Internal;

namespace Iserv.Niis.Model.Mappings.Subject
{
    public class SubjectDtoProfile : Profile
    {
        public SubjectDtoProfile()
        {
            CreateMap<RequestCustomer, SubjectDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Customer.CountryId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.CustomerRoleId))
                .ForMember(dest => dest.RoleNameRu, opt => opt.MapFrom(src => src.CustomerRole.NameRu))
                .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.CustomerRole.Code))
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.Customer.NameRu))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Customer.NameEn))
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(src => src.Customer.NameKz))
                .ForMember(dest => dest.Republic, opt => opt.MapFrom(src => src.Customer.Republic))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.Oblast, opt => opt.MapFrom(src => src.Customer.Oblast))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Customer.Street))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Customer.TypeId))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Customer.Type.NameRu))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.Customer.ContactInfos))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src =>
                        string.Join(";" + Environment.NewLine,
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("phone")).Select(ci => ci.Info))))
                .ForMember(dest => dest.MobilePhone,
                    opt => opt.MapFrom(src =>
                        string.Join(";" + Environment.NewLine,
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("mobilePhone"))
                                .Select(ci => ci.Info))))
                .ForMember(dest => dest.PhoneFax,
                    opt => opt.MapFrom(src =>
                        string.Join(";" + Environment.NewLine,
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("fax")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        string.Join(";" + Environment.NewLine,
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("email")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Xin, opt => opt.MapFrom(src => src.Customer.Xin))
                .ForMember(dest => dest.IsBeneficiary, opt => opt.MapFrom(src => src.Customer.IsBeneficiary))
                .ForMember(dest => dest.JurRegNumber, opt => opt.MapFrom(src => src.Customer.JurRegNumber))
                .ForMember(dest => dest.PowerAttorneyFullNum,
                    opt => opt.MapFrom(src => src.Customer.PowerAttorneyFullNum))
                .ForMember(dest => dest.GovReg,
                    opt => opt.MapFrom(src => src.Customer.CustomerAttorneyInfos.Select(a => a.CertNum).Join(", ")))
                .ForMember(dest => dest.IsNotResident, opt => opt.MapFrom(src => src.Customer.IsNotResident))
                .ForMember(dest => dest.IsNotMention, opt => opt.MapFrom(src => src.Customer.IsNotMention))
                .ForMember(dest => dest.Apartment, opt => opt.MapFrom(src => src.Customer.Apartment))
                .ForMember(dest => dest.BeneficiaryTypeId, opt => opt.MapFrom(src => src.Customer.BeneficiaryTypeId))
                .ForMember(dest => dest.CommonAddress, opt => opt.MapFrom(src => src.Address ?? src.AddressKz ?? src.AddressEn ?? src.Customer.Address ?? src.Customer.AddressKz ?? src.Customer.AddressEn ?? string.Empty))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => GetDisplayOrder(src.CustomerRole.Code)));

            CreateMap<SubjectDto, RequestCustomer>()
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.CustomerRoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<DicContactInfoType, BaseDictionaryDto>();

            CreateMap<BaseDictionaryDto, DicContactInfoType>();

            CreateMap<ContactInfo, ContactInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ContactInfoDto, ContactInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SubjectDto, DicCustomer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.ContactInfos));

            CreateMap<DicCustomer, SubjectDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Type.NameRu))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.ContactInfos));

            CreateMap<ContractCustomer, SubjectDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.ContractId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.CustomerRoleId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Customer.CountryId))
                .ForMember(dest => dest.RoleNameRu, opt => opt.MapFrom(src => src.CustomerRole.NameRu))
                .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.CustomerRole.Code))
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.Customer.NameRu))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Customer.NameEn))
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(src => src.Customer.NameKz))
                .ForMember(dest => dest.Republic, opt => opt.MapFrom(src => src.Customer.Republic))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.Oblast, opt => opt.MapFrom(src => src.Customer.Oblast))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Customer.Street))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Customer.TypeId))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Customer.Type.NameRu))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.Customer.ContactInfos))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("phone")).Select(ci => ci.Info))))
                .ForMember(dest => dest.MobilePhone,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("mobilePhone"))
                                .Select(ci => ci.Info))))
                .ForMember(dest => dest.PhoneFax,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("fax")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("email")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Xin, opt => opt.MapFrom(src => src.Customer.Xin))
                .ForMember(dest => dest.IsNotMention, opt => opt.MapFrom(src => src.Customer.IsNotMention))
                .ForMember(dest => dest.JurRegNumber, opt => opt.MapFrom(src => src.Customer.JurRegNumber))
                .ForMember(dest => dest.PowerAttorneyFullNum,
                    opt => opt.MapFrom(src => src.Customer.PowerAttorneyFullNum))
                .ForMember(dest => dest.GovReg,
                    opt => opt.MapFrom(src => src.Customer.CustomerAttorneyInfos.Select(a => a.CertNum).Join(", ")))
                .ForMember(dest => dest.IsBeneficiary, opt => opt.MapFrom(src => src.Customer.IsBeneficiary))
                .ForMember(dest => dest.IsNotResident, opt => opt.MapFrom(src => src.Customer.IsNotResident))
                .ForMember(dest => dest.Apartment, opt => opt.MapFrom(src => src.Customer.Apartment))
                .ForMember(dest => dest.BeneficiaryTypeId, opt => opt.MapFrom(src => src.Customer.BeneficiaryTypeId))
                .ForMember(dest => dest.CommonAddress, opt => opt.MapFrom(src => src.Address ?? src.AddressKz ?? src.AddressEn ?? src.Customer.Address ?? src.Customer.AddressKz ?? src.Customer.AddressEn ?? string.Empty))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => GetDisplayOrder(src.CustomerRole.Code)));

            CreateMap<SubjectDto, ContractCustomer>()
                .ForMember(dest => dest.ContractId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.CustomerRoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ProtectionDocCustomer, SubjectDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.ProtectionDocId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.CustomerRoleId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Customer.CountryId))
                .ForMember(dest => dest.RoleNameRu, opt => opt.MapFrom(src => src.CustomerRole.NameRu))
                .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.CustomerRole.Code))
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.Customer.NameRu))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Customer.NameEn))
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(src => src.Customer.NameKz))
                .ForMember(dest => dest.Republic, opt => opt.MapFrom(src => src.Customer.Republic))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.Oblast, opt => opt.MapFrom(src => src.Customer.Oblast))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Customer.Street))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Customer.TypeId))
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Customer.Type.NameRu))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.Customer.ContactInfos))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("phone")).Select(ci => ci.Info))))
                .ForMember(dest => dest.MobilePhone,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("mobilePhone"))
                                .Select(ci => ci.Info))))
                .ForMember(dest => dest.PhoneFax,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("fax")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        string.Join(";",
                            src.Customer.ContactInfos.Where(ci => ci.Type.Code.Equals("email")).Select(ci => ci.Info))))
                .ForMember(dest => dest.Xin, opt => opt.MapFrom(src => src.Customer.Xin))
                .ForMember(dest => dest.IsBeneficiary, opt => opt.MapFrom(src => src.Customer.IsBeneficiary))
                .ForMember(dest => dest.JurRegNumber, opt => opt.MapFrom(src => src.Customer.JurRegNumber))
                .ForMember(dest => dest.PowerAttorneyFullNum,
                    opt => opt.MapFrom(src => src.Customer.PowerAttorneyFullNum))
                .ForMember(dest => dest.GovReg,
                    opt => opt.MapFrom(src => src.Customer.CustomerAttorneyInfos.Select(a => a.CertNum).Join(", ")))
                .ForMember(dest => dest.IsNotResident, opt => opt.MapFrom(src => src.Customer.IsNotResident))
                .ForMember(dest => dest.IsNotMention, opt => opt.MapFrom(src => src.Customer.IsNotMention))
                .ForMember(dest => dest.Apartment, opt => opt.MapFrom(src => src.Customer.Apartment))
                .ForMember(dest => dest.BeneficiaryTypeId, opt => opt.MapFrom(src => src.Customer.BeneficiaryTypeId))
                .ForMember(dest => dest.CommonAddress, opt => opt.MapFrom(src => src.Address ?? src.AddressKz ?? src.AddressEn ?? src.Customer.Address ?? src.Customer.AddressKz ?? src.Customer.AddressEn ?? string.Empty))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => GetDisplayOrder(src.CustomerRole.Code)));

            CreateMap<SubjectDto, ProtectionDocCustomer>()
                .ForMember(dest => dest.ProtectionDocId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.CustomerRoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<RequestCustomer, ProtectionDocCustomer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerRole, opt => opt.Ignore());
        }

        private int GetDisplayOrder(string roleCode)
        {
            switch (roleCode)
            {
                case DicCustomerRoleCodes.Declarant:
                case DicCustomerRoleCodes.Owner:
                case DicCustomerRoleCodes.PatentOwner:
                    return 1;
                case DicCustomerRoleCodes.Author:
                    return 2;
                case DicCustomerRoleCodes.PatentAttorney:
                    return 3;
                case DicCustomerRoleCodes.Confidant:
                    return 4;
                case DicCustomerRoleCodes.Correspondence:
                    return 5;
            }
            return 0;
        }
    }
}