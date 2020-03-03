using AutoMapper;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings
{
    public class PatentAttorneyProfile : Profile
    {
        public PatentAttorneyProfile()
        {
            CreateMap<CustomerAttorneyInfo, PatentAttorney>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.AttorneyId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dst => dst.Xin, opt => opt.MapFrom(src => src.Iin))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.NameLast))
                .ForMember(dst => dst.Firstname, opt => opt.MapFrom(src => src.NameFirst))
                .ForMember(dst => dst.Secondname, opt => opt.MapFrom(src => src.NameMiddle))
                .ForMember(dst => dst.CertificateNumber, opt => opt.MapFrom(src => src.CertNum))
                .ForMember(dst => dst.CertificateDate, opt => opt.MapFrom(src => src.CertDate))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.RevalidNote, opt => opt.MapFrom(src => src.RevalidNote))
                .ForMember(dst => dst.Activites, opt => opt.MapFrom(src => src.Ops))
                .ForMember(dst => dst.Knowledge, opt => opt.MapFrom(src => src.KnowledgeArea))
                .ForMember(dst => dst.PlaceOfWork, opt => opt.MapFrom(src => src.Job))
                .ForMember(dst => dst.Languages, opt => opt.MapFrom(src => src.Language))
                .ForMember(dst => dst.PartOfTheWorld, opt => opt.Ignore())

                .ForMember(dst => dst.Country, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Country != null ? src.Customer.Country.NameRu : string.Empty : string.Empty))
                .ForMember(dst => dst.Region, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Oblast : string.Empty))
                .ForMember(dst => dst.District, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Region : string.Empty))
                .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.City : string.Empty))
                .ForMember(dst => dst.AddressEn, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.AddressEn : string.Empty))
                .ForMember(dst => dst.AddressKz, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.AddressKz : string.Empty))
                .ForMember(dst => dst.AddressRu, opt => opt.MapFrom(src => src.Address ?? (src.Customer != null ? src.Customer.Address : string.Empty)))

                .ForMember(dst => dst.Postcode, opt => opt.Ignore())
                .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dst => dst.Fax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
                ;
        }
    }
}