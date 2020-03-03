using System;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class UpdateUserHandler : BaseHandler
    {
        private readonly IMapper _mapper;

        public UpdateUserHandler(
            IMapper mapper)
        {
            _mapper = mapper;
        }
        //Вернул обнуление департамента и позиции
        public void Handle(UserDetailsDto userDetailsDto)
        {
            var userId = userDetailsDto.Id;
            var user = Executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(userId));

            if (user == null)
                throw new DataNotFoundException(nameof(ApplicationUser),
                    DataNotFoundException.OperationType.Read, userId);

            _mapper.Map(userDetailsDto, user);
            user.CustomerId = null;
            user.Department = null;
            user.Position = null;
            try
            {
                Executor.CommandChain()
                    .AddCommand<UpdateUserCommand>(c => c.Execute(user))
                    .AddCommand<UpdateUserRolesCommand>(c => c.Execute(userId, userDetailsDto.RoleIds))
                    .AddCommand<UpdateUserIcgsCommand>(c => c.Execute(userId, userDetailsDto.IcgsIds))
                    .AddCommand<UpdateUserIpcsCommand>(c => c.Execute(userId, userDetailsDto.IpcIds))
                    .ExecuteAllWithTransaction();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}