using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Role;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Model.Mappings.Identity
{
    public class RoleProfile : Profile
    {

        public RoleProfile()
        {
            IEnumerable<IdentityRoleClaim<int>> roleClaims = null;

            CreateMap<ApplicationRole, SelectOptionDto>();

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(dest => dest.ClaimsTotal, opt => opt
                    .MapFrom(src => roleClaims.Count(rc => rc.RoleId == src.Id)))
                .ForMember(dest => dest.StagesTotal, opt => opt.MapFrom(src => src.Stages.Count));

            CreateMap<ApplicationRole, RoleDetailsDto>()
                .ForMember(dest => dest.Permissions, opt => opt
                    .ResolveUsing((role, dto, res, context) =>
                        ((IEnumerable<IdentityRoleClaim<int>>) context.Items["roleClaims"])
                        .Where(rc => rc.RoleId == role.Id)
                        .Select(x => x.ClaimValue)))
                .ForMember(dest => dest.RoleStages, opt => opt
                    .MapFrom(src => src.Stages.Select(rs => rs.StageId)));

            CreateMap<RoleDetailsDto, ApplicationRole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForSourceMember(src => src.Permissions, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Code));

            CreateMap<ClaimConstant, ClaimDto>();
        }
    }
}
