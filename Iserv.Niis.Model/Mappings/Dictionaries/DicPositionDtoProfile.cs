using AutoMapper;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Model.Models.Dictionaries;

namespace Iserv.Niis.Model.Mappings.Dictionaries
{
    public class DicPositionDtoProfile : Profile
    {
        public DicPositionDtoProfile()
        {
            CreateMap<DicPosition, DicPositionDto>()
                .ForMember(dest => dest.PositionTypeId, opt => opt.MapFrom(src => src.PositionType.Id))
                .ForMember(dest => dest.PositionTypeCode, opt => opt.MapFrom(src => src.PositionType.Code))
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.PositionType.NameRu))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.PositionType.NameEn))
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(src => src.PositionType.NameKz));
        }
    }
}
