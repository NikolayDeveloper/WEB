using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Mappings.BibliographicData
{
    public class IcgsDtoProfile : Profile
    {
        public IcgsDtoProfile()
        {
            CreateMap<ICGSRequest, IcgsDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IcgsId,
                    opt => opt.MapFrom(src => src.IcgsId))
                .ForMember(dest => dest.IcgsName,
                    opt => opt.MapFrom(src => src.Icgs.NameRu))
                .ForMember(dest => dest.ClaimedDescription,
                    opt => opt.MapFrom(src => src.ClaimedDescription))
                .ForMember(dest => dest.ClaimedDescriptionEn,
                    opt => opt.MapFrom(src => src.ClaimedDescriptionEn))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DescriptionKz,
                    opt => opt.MapFrom(src => src.DescriptionKz))
                .ForMember(dest => dest.NegativeDescription,
                    opt => opt.MapFrom(src => src.NegativeDescription));

            CreateMap<IcgsDto, ICGSRequest>();

            CreateMap<IcgsDto, ICGSProtectionDoc>();

            CreateMap<ICGSProtectionDoc, IcgsDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IcgsId,
                    opt => opt.MapFrom(src => src.IcgsId))
                .ForMember(dest => dest.IcgsName,
                    opt => opt.MapFrom(src => src.Icgs.NameRu))
                .ForMember(dest => dest.ClaimedDescription,
                    opt => opt.MapFrom(src => src.ClaimedDescription))
                .ForMember(dest => dest.ClaimedDescriptionEn,
                    opt => opt.MapFrom(src => src.ClaimedDescriptionEn))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DescriptionKz,
                    opt => opt.MapFrom(src => src.DescriptionKz))
                .ForMember(dest => dest.NegativeDescription,
                    opt => opt.MapFrom(src => src.NegativeDescription));

            CreateMap<ICGSRequest, ICGSRequestItemDto>()
                .ForMember(dest => dest.IcgsNameRu, opt => opt.MapFrom(src => src.Icgs.NameRu))
                .ForMember(dest => dest.IcgsNameKz, opt => opt.MapFrom(src => src.Icgs.NameKz))
                .ForMember(dest => dest.IcgsNameEn, opt => opt.MapFrom(src => src.Icgs.NameEn));

            CreateMap<ICGSRequestItemDto, ICGSRequest>()
                .ForMember(dest => dest.Icgs, src => src.Ignore());
        }
    }
}