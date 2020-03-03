using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.Model.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationUser
{
    public class Create
    {
        public class Command : IRequest<UserDetailsDto>
        {
            public Command(UserDetailsDto userDetailsDto)
            {
                UserDetailsDto = userDetailsDto;
            }

            public UserDetailsDto UserDetailsDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                // TODO сейчас в системе ИИН имеет некорректный формат. Временно разрешено редактирование для существующих пользователей
                //RuleFor(x => x.UserDetailsDto.Xin)
                //    .NotEmpty()
                //    .Length(12)
                //    .IsCorrectXinFormat();
                RuleFor(x => x.UserDetailsDto.Email)
                    .NotEmpty()
                    .EmailAddress();
                RuleFor(x => x.UserDetailsDto.NameRu).NotEmpty();
                RuleFor(x => x.UserDetailsDto.Password)
                    .NotEmpty()
                    .When(x => x.UserDetailsDto.Id <= 0)
                    .MinimumLength(6);
                RuleFor(x => x.UserDetailsDto.DepartmentId).NotEmpty();
                RuleFor(x => x.UserDetailsDto.DivisionId).NotEmpty();
                RuleFor(x => x.UserDetailsDto.CustomerId).NotEmpty();
                RuleFor(x => x.UserDetailsDto.RoleIds).NotEmpty();
                RuleFor(x => x.UserDetailsDto).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, UserDetailsDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly UserManager<Domain.Entities.Security.ApplicationUser> _userManager;
            private readonly IUserRolesUpdater _userRolesUpdater;
            private readonly IUserIcgsUpdater _userIcgsUpdater;

            public CommandHandler(
                IMapper mapper,
                UserManager<Domain.Entities.Security.ApplicationUser> userManager,
                IUserRolesUpdater userRolesUpdater,
                IUserIcgsUpdater userIcgsUpdater, NiisWebContext context)
            {
                _mapper = mapper;
                _userManager = userManager;
                _userRolesUpdater = userRolesUpdater;
                _userIcgsUpdater = userIcgsUpdater;
                _context = context;
            }

            public async Task<UserDetailsDto> Handle(Command message)
            {
                if (await _userManager.FindByNameAsync(message.UserDetailsDto.Xin.Trim()) != null)
                {
                    throw new ValidationException("user with same xin already registered");
                }

                if (await _userManager.FindByEmailAsync(message.UserDetailsDto.Email.Trim()) != null)
                {
                    throw new ValidationException("user with same email already registered");
                }

                var userDto = message.UserDetailsDto;
                userDto.Id = 0;
                var newUser = _mapper.Map<UserDetailsDto, Domain.Entities.Security.ApplicationUser>(userDto);

                try
                {
                    await _userManager.CreateAsync(newUser, userDto.Password.Trim());
                    await _userManager.SetLockoutEnabledAsync(newUser, false);
                    await _userRolesUpdater.UpdateAsync(newUser, message.UserDetailsDto.RoleIds);
                    await _userIcgsUpdater.UpdateAsync(newUser, message.UserDetailsDto.IcgsIds);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == newUser.Id)
                    .ToListAsync();

                return _mapper.Map<UserDetailsDto>(newUser, options => options.Items["userRoles"] = userRoles);
            }
        }
    }
}
