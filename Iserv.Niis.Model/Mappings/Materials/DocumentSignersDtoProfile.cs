using AutoMapper;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;

namespace Iserv.Niis.Model.Mappings.Materials
{
    public class DocumentSignersDtoProfile : Profile
    {
        public DocumentSignersDtoProfile()
        {
            CreateMap<DocumentUserSignature, DocumentUserSignatureDto>()
                .ForMember(dest => dest.SignatureDate, opt => opt.MapFrom(src => src.DateCreate));
            CreateMap<DocumentUserSignatureDto, DocumentUserSignature>()
                .ForMember(dest => dest.DateCreate, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.Ignore());
        }
    }
}