using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Material;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractProtectionDocRelationDtoProfile: Profile
    {
        public ContractProtectionDocRelationDtoProfile()
        {
            CreateMap<ContractProtectionDocRelation, MaterialOwnerDto>()
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => Owner.Type.ProtectionDoc))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.ProtectionDoc.TypeId))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.ProtectionDocId));

            CreateMap<MaterialOwnerDto, ContractProtectionDocRelation>()
                .ForMember(dest => dest.ContractId, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDocId, opt => opt.Ignore());

            CreateMap<ContractProtectionDocRelation, ContractProtectionDocRelationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProtectionDoc, opt => opt.MapFrom(src => src.ProtectionDoc))
                .ForMember(dest => dest.ICGSProtectionDocs, opt => opt.MapFrom(src => src.ContractProtectionDocICGSProtectionDocs));

            CreateMap<ContractProtectionDocRelationDto, ContractProtectionDocRelation>()
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore())
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dest, member, context) => context.Options.Items["ContractId"]));

            CreateMap<ContractProtectionDocICGSProtectionDocRelation, ContractProtectionDocICGSProtectionDocRelationDto>()
                .ForMember(dest => dest.ICGSProtectionDoc, opt => opt.MapFrom(src => src.ICGSProtectionDoc))
                .ReverseMap();
        }
    }
}