using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ; ;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.Model.Mappings;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;

namespace Iserv.Niis.Api.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccessController : Controller
    {
        private readonly IExecutor _executor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessController(IExecutor executor, UserManager<ApplicationUser> userManager)
        {
            _executor = executor;
            _userManager = userManager;
        }

        [HttpGet("getDocumentKinds/{ownerId}")]
        public async Task<IActionResult> GetDocumentKinds(int? ownerId)
        {
            var possibleRouteCodes = new[] { DicRoute.Codes.IN, DicRoute.Codes.OUT, DicRoute.Codes.W, DicRoute.Codes.DR, DicRoute.Codes.U,
                DicRoute.Codes.S2, DicRoute.Codes.TM, DicRoute.Codes.NMPT, DicRoute.Codes.TMI,
                DicRoute.Codes.SA, DicRoute.Codes.B, DicRoute.Codes.DK};

            var materialRouteCodes = new[] { DicRoute.Codes.IN, DicRoute.Codes.OUT, DicRoute.Codes.DR, DicRoute.Codes.W };

            if (ownerId != null)
                possibleRouteCodes = materialRouteCodes;

            var user = _executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
            var roleNames = await _userManager.GetRolesAsync(user);
            var allStages = _executor.GetQuery<GetAccessRouteStagesByRolesQuery>()
                .Process(q => q.Execute(roleNames));

            var stagesIsFirst = allStages.Where(s => s.IsFirst && s.Route != null);
            var stages = stagesIsFirst.Where(s => possibleRouteCodes.Contains(s.Route.Code));

            var routeCodes = stages.Select(s => s.Route.Code).Distinct();

            return Ok(routeCodes.Select(c => GetDocumentTypeByCode(c)).Distinct());
        }

        [HttpGet("getAccessProtectionDocTypes")]
        public async Task<IActionResult> GetAccessProtectionDocType()
        {
            var user = _executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
            var roleNames = await _userManager.GetRolesAsync(user);
            var protectionDocTypes = await _executor.GetQuery<GetDicProtectionDocTypesQuery>().Process(q => q.ExecuteAsync());
            var routeCodes = _executor.GetQuery<GetAccessRouteStagesByRolesQuery>()
                .Process(q => q.Execute(roleNames)).Where(d => d.Route != null).Select(s => s.Route.Code);

            return Ok(protectionDocTypes.Where(pt => routeCodes.Contains(pt.Route?.Code)));
        }

        private DocumentType GetDocumentTypeByCode(string code)
        {
            switch (code)
            {
                case DicRoute.Codes.IN:
                    return DocumentType.Incoming;
                case DicRoute.Codes.OUT:
                    return DocumentType.Outgoing;
                case DicRoute.Codes.W:
                    return DocumentType.Internal;
                case DicRoute.Codes.DR:
                    return DocumentType.DocumentRequest;
                case DicRoute.Codes.U:
                case DicRoute.Codes.NMPT:
                case DicRoute.Codes.SA:
                case DicRoute.Codes.B:
                case DicRoute.Codes.S2:
                case DicRoute.Codes.TM:
                case DicRoute.Codes.TMI:
                    return DocumentType.Request;
                case DicRoute.Codes.DK:
                    return DocumentType.Contract;
                default:
                    return DocumentType.None;
            }
        }
    }
}