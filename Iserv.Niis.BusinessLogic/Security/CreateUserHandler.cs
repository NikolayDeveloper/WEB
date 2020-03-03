using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class CreateUserHandler : BaseHandler
    {
        private readonly IMapper _mapper;

        public CreateUserHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> HandleAsync(UserDetailsDto userDetailDto)
        {
            var userByXin = Executor.GetQuery<GetUserByXinQuery>().Process(q => q.Execute(userDetailDto.Xin?.Trim()));
            if (userByXin != null)
                throw new ValidationException("user with same xin already registered");

            var userByEmail = Executor.GetQuery<GetUserByEmailQuery>().Process(q => q.Execute(userDetailDto.Email?.Trim()));
            if (userByEmail != null)
                throw new ValidationException("user with same email already registered");

            var newUser = _mapper.Map<UserDetailsDto, ApplicationUser>(userDetailDto);

            Executor.CommandChain()
                .AddCommand<CreateUserCommand>(c => c.Execute(newUser))
                .AddCommand<UpdateUserRolesCommand>(c => c.Execute(newUser.Id, userDetailDto.RoleIds))
                .AddCommand<UpdateUserIcgsCommand>(c => c.Execute(newUser.Id, userDetailDto.IcgsIds))
                .ExecuteAllWithTransaction();

            return newUser.Id;
        }
    }
}