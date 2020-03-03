using System;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.User;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Api.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;
		private readonly IExecutor Repo;

		public UsersController(IExecutor executor, IMapper mapper, NiisWebContext context, IServiceProvider services)
        {
            _executor = executor;
            _mapper = mapper;
			Repo = new NiisRepository(context, services);
		}

		// GET: api/Users
		[HttpGet]
        public IActionResult Get()
        {
            var userDtos = _executor.GetQuery<GetUsersFilteredQuery>().Process(q => q.Execute(Request));

            return userDtos.AsOkObjectResult(Response); ;
        }

        // GET: api/Users
        [HttpGet("getCurrent")]
        public IActionResult GetCurrent()
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var result = Get(userId);
            return result;
        }

        //TODO: это лишний экшн, перейти на Get()
        // GET: api/Users/list
        [HttpGet("list")]
        public IActionResult GetList()
        {
            var userDtos = _executor.GetQuery<GetUsersAllQuery>().Process(q => q.Execute(Request));

            return Ok(userDtos);
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var user = _executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(id));
            if (user == null)
                throw new DataNotFoundException(nameof(ApplicationUser),
                    DataNotFoundException.OperationType.Read, id);

            var userRoles = _executor.GetQuery<GetUserRolesAllQuery>().Process(q => q.Execute());

            var result =
                _mapper.Map<ApplicationUser, UserDetailsDto>(user, options => options.Items["userRoles"] = userRoles);

            return Ok(result);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDetailsDto userDetailsDto)
        {
            var userId = await _executor.GetHandler<CreateUserHandler>().Process(h => h.HandleAsync(userDetailsDto));
            var user = _executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(userId));
            if (user == null)
                throw new DataNotFoundException(nameof(ApplicationUser),
                    DataNotFoundException.OperationType.Read, userId);

            var userRoles = _executor.GetQuery<GetUserRolesAllQuery>().Process(q => q.Execute());
            var result =
                _mapper.Map<ApplicationUser, UserDetailsDto>(user, options => options.Items["userRoles"] = userRoles);

            return new CreatedAtRouteResult("GetUser", result);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDetailsDto userDetailsDto)
        {
            userDetailsDto.Id = id;
            _executor.GetHandler<UpdateUserHandler>().Process<UserDetailsDto>(h => h.Handle(userDetailsDto));
			_executor.GetCommand<ChangerUserPasswordCommand>().Process(c => c.Execute(userDetailsDto));

			//Repo.GetCommand<Iserv.Niis.DataBridge.TestQuery.ChangerUserPasswordCommand1>().Process(c => c.Execute(userDetailsDto));

			return NoContent();
        }
    }
}