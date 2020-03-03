using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.Model.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Features.Administration.ApplicationUser
{
    public class Update
    {
        public class Command : IRequest<Unit>
        {
            public Command(int userId, UserDetailsDto userDetailDto)
            {
                UserId = userId;
                UserDetailDto = userDetailDto;
            }

            public int UserId { get; }
            public UserDetailsDto UserDetailDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                // TODO сейчас в системе ИИН имеет некорректный формат. Временно разрешено редактирование для существующих пользователей
                //RuleFor(x => x.UserDetailDto.Xin)
                //    .NotEmpty()
                //    .Length(12)
                //    .IsCorrectXinFormat();
                RuleFor(x => x.UserDetailDto.Email)
                    .NotEmpty()
                    .EmailAddress();
                RuleFor(x => x.UserDetailDto.NameRu).NotEmpty();
                RuleFor(x => x.UserDetailDto.DivisionId).NotEmpty();
                RuleFor(x => x.UserDetailDto.DepartmentId).NotEmpty();
                RuleFor(x => x.UserDetailDto.PositionId).NotEmpty();
                RuleFor(x => x.UserDetailDto.CustomerId).NotEmpty();
                RuleFor(x => x.UserDetailDto.RoleIds).NotEmpty();
                RuleFor(x => x.UserDetailDto).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly UserManager<Domain.Entities.Security.ApplicationUser> _userManager;
            private readonly IUserRolesUpdater _userRolesUpdater;
            private readonly IUserIcgsUpdater _userIcgsUpdater;
            private readonly IUserPasswordUpdater _userPasswordUpdater;
            private readonly IMapper _mapper;

            public CommandHandler(
                IMapper mapper,
                UserManager<Domain.Entities.Security.ApplicationUser> userManager,
                IUserRolesUpdater userRolesUpdater,
                IUserIcgsUpdater userIcgsUpdater,
                IUserPasswordUpdater userPasswordUpdater)
            {
                _mapper = mapper;
                _userManager = userManager;
                _userRolesUpdater = userRolesUpdater;
                _userPasswordUpdater = userPasswordUpdater;
                _userIcgsUpdater = userIcgsUpdater;
            }

            public async Task<Unit> Handle(Command message)
            {
                var userId = message.UserId;
                var dbUser = await _userManager.FindByIdAsync(userId.ToString());
                _mapper.Map(message.UserDetailDto, dbUser);

                try
                {
                    await _userManager.UpdateAsync(dbUser);
                    await _userRolesUpdater.UpdateAsync(dbUser, message.UserDetailDto.RoleIds);
                    await _userIcgsUpdater.UpdateAsync(dbUser, message.UserDetailDto.IcgsIds);
                    await _userPasswordUpdater.UpdateAsync(dbUser, message.UserDetailDto.Password);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
                return await Unit.Task;
            }
        }
    }
}
