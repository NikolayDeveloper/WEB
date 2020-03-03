using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.BusinessLogic.ClaimConstants;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRoutes;
using Iserv.Niis.BusinessLogic.Roles;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Iserv.Niis.DataAccess.EntityFramework.Repositories;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.Extensions.Configuration;
using Iserv.Niis.DataBridge.Repositories;
//using Iserv.Niis.DataBridge.TestQuery;

namespace Iserv.Niis.Api.Controllers.Administration
{
	[Produces("application/json")]
    [Route("api/Roles")]
    public class RolesController : BaseNiisApiController
    {
		protected IExecutor Repo;

		public RolesController(NiisWebContext context, IServiceProvider services)
		{
			Repo = new NiisRepository(context, services);
		}

		[HttpGet]
        public async Task<IActionResult> Get()
        {
            var roleClaims = await Executor.GetQuery<GetRoleClaimsQuery>().Process(q => q.ExecuteAsync());
            var roles = Executor.GetQuery<GetRolesQuery>().Process(q => q.Execute());
            var result = roles.ProjectTo<RoleDto>(new {roleClaims});
            return result
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        [HttpGet("{id}", Name = "GetRole")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await Executor.GetQuery<GetRoleByIdQuery>().Process(q => q.ExecuteAsync(id));
            var roleClaims = await Executor.GetQuery<GetRoleClaimsQuery>().Process(q => q.ExecuteAsync());
            var roleDto = Mapper.Map<RoleDetailsDto>(role, opts => opts.Items["roleClaims"] = roleClaims);
            return Ok(roleDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleDetailsDto model)
        {
            await Executor.GetHandler<CreateRoleHandler>().Process(h => h.ExecuteAsync(model));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RoleDetailsDto model)
        {
            var roleId = id;
            var roleDetailsDto = model;

            var role = await Executor.GetQuery<GetRoleByIdQuery>().Process(q => q.ExecuteAsync(roleId));
            Mapper.Map(roleDetailsDto, role);

            try
            {
                Executor.CommandChain()
                    .AddCommand<UpdateRoleCommand>(c => c.Execute(role))
                    .AddCommand<UpdateRoleClaimsCommand>(c => c.Execute(role, roleDetailsDto.Permissions))
                    .ExecuteAllWithTransaction();

                Executor.GetHandler<UpdateRoleRouteStagesHandler>()
                    .Process<int>(h => h.Execute(roleId, roleDetailsDto.RoleStages));
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException?.Message ?? e.Message);
            }

            return NoContent();
        }

        [HttpGet("select")]
        public IActionResult Select()
        {
            var roles = Executor.GetQuery<GetRolesQuery>().Process(q => q.Execute());
            var result = roles.ProjectTo<SelectOptionDto>();
            return Ok(result);
        }

        [HttpGet("prm")]
        public async Task<IActionResult> Permissions()
        {
            var claimConstants = await Executor.GetQuery<GetClaimConstantsQuery>().Process(q => q.ExecuteAsync());
            var claimConstantDtos = Mapper.Map<List<ClaimDto>>(claimConstants);
            return Ok(claimConstantDtos);
        }

        [HttpGet("stagesTree")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStagesTree()
        {



            { //Test

                


                //var ch = Executor.CommandChain();
                //var comm1 = ch.AddCommand<DataBridge.TestQuery.TestCommand1>(q => q.Execute(123));
                //var comm2 = ch.AddCommand<DataBridge.TestQuery.TestCommand2>(q => q.Execute(321));
                //comm2.ExecuteAllWithTransaction();

                //comm2.Dispose();
                //var protectionDocTypes4 = await Executor.GetQuery<GetDicRoutesQuery>().Process(q => q.ExecuteAsync());

                //Executor.CommandChain()
                //    .AddCommand<DataBridge.TestQuery.TestCommand1>(q => q.Execute(123))
                //    .AddCommand<DataBridge.TestQuery.TestCommand2>(q => q.Execute(321))
                //    .ExecuteAll();

                //Executor.CommandChain()
                //    .AddCommand<GetDicProtectionDocTypesQuery>(q => q.ExecuteAsync(321))
                //    .AddCommand<GetDicProtectionDocTypesQuery>(q => q.ExecuteAsync(123))
                //.ExecuteAll();

                //var protectionDocTypes12 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var protectionDocTypes22 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var t32 = await protectionDocTypes12.Process(q => q.ExecuteAsync());

                //protectionDocTypes12.Dispose();

                //var protectionDocTypes32 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var protectionDocTypes23 = await protectionDocTypes22.Process(q => q.ExecuteAsync());
                //var t122 = await protectionDocTypes32.Process(q => q.ExecuteAsync());

                ////return null;


                //var protectionDocTypes21 = Executor.GetQuery<GetDicProtectionDocTypesQuery>().Process(q => q.ExecuteAsync(123));
                //var protectionDocTypes211 = protectionDocTypes21.Result;

                ////comm1.ExecuteAll();
                //var protectionDocTypes1 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var protectionDocTypes2 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var t3 = await protectionDocTypes1.Process(q => q.ExecuteAsync());

                //protectionDocTypes1.Dispose();

                //var protectionDocTypes3 = Executor.GetQuery<GetDicProtectionDocTypesQuery>();
                //var protectionDocTypes99 = await protectionDocTypes2.Process(q => q.ExecuteAsync());
                //var t12 = await protectionDocTypes3.Process(q => q.ExecuteAsync());

            }



             var protectionDocTypes = await Executor.GetQuery<GetDicProtectionDocTypesQuery>().Process(q => q.ExecuteAsync());
            var routes = await Executor.GetQuery<GetDicRoutesQuery>().Process(q => q.ExecuteAsync());
            
            var tree = routes
                .Select(r =>
                    new BaseTreeNodeDto
                    {
                        Data = 0,
                        Label = r.NameRu + (protectionDocTypes.Any(dt => dt.RouteId == r.Id)
                                    ? $" ({string.Join(", ", protectionDocTypes.Where(dt => dt.RouteId == r.Id).Select(t => t.NameRu))})"
                                    : string.Empty),
                        Selectable = true,
                        Children = r.RouteStages.Select(rs => new BaseTreeNodeDto
                        {
                            Data = rs.Id,
                            Label = rs.NameRu,
                            Selectable = true
                        })
                    })
                .OrderBy(t => t.Label)
                .ToList();
            return Ok(tree);
        }
    }
}