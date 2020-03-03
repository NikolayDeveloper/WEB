using System;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractRequestRelationDtoProfile: Profile
    {
        public ContractRequestRelationDtoProfile()
        {
            CreateMap<ContractRequestRelation, ContractRequestRelationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Request, opt => opt.MapFrom(src => src.Request))
                .ForMember(dest => dest.IcgsRequestItemDtos, opt => opt.MapFrom(src => src.Request.ICGSRequests))
                .ForMember(dest => dest.ICGSRequestRelations, opt => opt.MapFrom(src => src.ContractRequestICGSRequests));
            CreateMap<ContractRequestRelationDto, ContractRequestRelation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dest, member, context) => context.Options.Items["ContractId"]))
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.ContractRequestICGSRequests, opt => opt.Ignore());

            CreateMap<ContractRequestRelation, MaterialOwnerDto>()
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => Owner.Type.Request))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.Request.ProtectionDocTypeId))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.RequestId));

            CreateMap<MaterialOwnerDto, ContractRequestRelation>()
                .ForMember(dest => dest.ContractId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore());

            CreateMap<ContractRequestICGSRequestRelation, ContractRequestICGSRequestRelationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ICGSRequest, opt => opt.MapFrom(src => src.ICGSRequest))
                .ForMember(dest => dest.ICGSRequestId, opt => opt.MapFrom(src => src.ICGSRequestId))
                .ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Description.Split(';')));
            CreateMap<ContractRequestICGSRequestRelationDto, ContractRequestICGSRequestRelation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ICGSRequest, opt => opt.Ignore())
                .ForMember(dest => dest.ICGSRequestId, opt => opt.MapFrom(src => src.ICGSRequestId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => GetDescription(src.Descriptions)));
        }

        private static string GetDescription(string[] descriptions)
        {
            var result = string.Empty;
            foreach (var description in descriptions)
            {
                result += $"{description};";
            }
            return result;
        }
    }
}