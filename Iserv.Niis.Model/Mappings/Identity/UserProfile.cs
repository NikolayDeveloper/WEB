using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Model.Mappings.Identity
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            IEnumerable<ApplicationRole> roles = null;
            IEnumerable<IdentityUserRole<int>> userRoles = null;

            CreateMap<ApplicationUser, UserDetailsDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.NameRu))
                .ForMember(dest => dest.DivisionId, opt => opt.MapFrom(src => src.Department.DivisionId))
                .ForMember(dest => dest.PositionTypeNameRu, opt => opt.MapFrom(src => src.Position.PositionType.NameRu))
                .ForMember(dest => dest.PositionTypeCode, opt => opt.MapFrom(src => src.Position.PositionType.Code))
                .ForMember(dest => dest.RoleIds,
                    opt => opt.ResolveUsing((source, dest, member, context) =>
                        ((IEnumerable<IdentityUserRole<int>>)context.Items["userRoles"])
                            .Where(ur => ur.UserId == source.Id)
                            .Select(ur => ur.RoleId)))
                .ForMember(dest => dest.IcgsIds, opt => opt.MapFrom(src => src.Icgss.Select(i => i.IcgsId)))
                .ForMember(dest => dest.IpcIds, opt => opt.MapFrom(src => src.Ipcs.Select(i => i.IpcId)));

            CreateMap<UserDetailsDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Xin.Trim()))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(src => src.NameRu))
                .ForMember(dest => dest.RoleNameRu, opt =>
                    opt.MapFrom(src =>
                        string.Join(", ",
                             userRoles
                             .Join(roles,
                                ur => ur.RoleId,
                                role => role.Id,
                                (ur, role) => new { ur, role })
                             .Where(j => j.ur.UserId == src.Id)
                             .Select(j => j.role.NameRu)
                        )
                    )
                 )
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(x => x.DepartmentId))
                .ForMember(dest => dest.DepartmentNameRu, opt => opt.MapFrom(x => x.Department.NameRu))
                .ForMember(dest => dest.DivisionNameRu, opt => opt.MapFrom(src => src.Department.Division.NameRu));

            CreateMap<ApplicationUser, SelectOptionDto>();
        }
    }
}
