using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/Dictionaries")]
    [AllowAnonymous]
    public class DictionariesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly NiisWebContext _context;

        public DictionariesController(IMediator mediator, NiisWebContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet("{dictype}")]
        public async Task<IActionResult> Get(string dictype)
        {
            var result = await _mediator.Send(new Features.Dictionary.Base.List.Query(dictype));

            return Ok(result);
        }

        [HttpGet("{dictype}/{id}")]
        public async Task<IActionResult> Get(string dictype, int id)
        {
            var result = await _mediator.Send(new Features.Dictionary.Base.Single.Query(dictype, id));

            return Ok(result);
        }

        [HttpGet("{dictype}/bycodes")]
        public async Task<IActionResult> GetByCodes(string dictype)
        {
            var result = await _mediator.Send(new Features.Dictionary.Base.List.Query(dictype,
                Request.QueryString.Value.Replace("?", "").Replace("=", "").Split(',')));
            return Ok(result);
        }

        [HttpPost("{dictype}")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(string dictype, [FromBody]object dto)
        {
            var resolver = new DicTypeResolver();
            var obj = JsonConvert.DeserializeObject(value: dto.ToString(), type: resolver.Resolve(dictype));

            var dictionaries = _context.Set(resolver.Resolve(dictype)) as IQueryable<IDictionaryEntity<int>>;
            var id = 1;
            var dictionary = dictionaries?.SingleOrDefault(d => d.Id == id);

            MapAllowedProperties(obj, dictionary);

            _context.Update(dictionary);

            await _context.SaveChangesAsync();

            return Ok(obj);


            void MapAllowedProperties(object source, object dest)
            {
                string[] ignored = { "Id", "Code", "ExternalId", "DateCreate", "DateUpdate" };
                bool Pred(PropertyInfo p) => !ignored.Contains(p.Name);

                var sourceProps = source.GetType()
                    .GetProperties()
                    .Where(Pred)
                    .OrderBy(p => p.Name)
                    .ToList();

                var destProps = source.GetType()
                    .GetProperties()
                    .Where(Pred)
                    .OrderBy(p => p.Name)
                    .ToList();

                for (var i = 0; i < destProps.Count; i++)
                {
                    sourceProps[i].SetValue(dest, sourceProps[i].GetValue(source));
                }

            }
        }
        /// <summary>
        /// API для получения справочных данных в формате ключ-значение
        /// </summary>
        /// <param name="dictype">Тип справочника</param>
        /// <returns></returns>
        [HttpGet("{dictype}/select")]
        public async Task<IActionResult> GetSelectOptions(string dictype)
        {
            var result = await _mediator.Send(new Features.Dictionary.SelectOption.List.Query(dictype));

            return Ok(result);
        }
        
        /// <summary>
        /// API äëÿ ïîëó÷åíèÿ ñïðàâî÷íîé çàïèñè â ôîðìàòå êëþ÷-çíà÷åíèå
        /// </summary>
        /// <param name="dictype">Èìÿ ñïðàâî÷íèêà</param>
        /// <returns></returns>
        [HttpGet("{dictype}/select/{id}")]
        public async Task<SelectOptionDto> GetSelectOption(string dictype, int id)
        {
            return await _mediator.Send(new Features.Dictionary.SelectOption.Single.Query(dictype, id));
        }

        [HttpGet("{dictype}/select/bycodes")]
        public async Task<IActionResult> GetSelectOptionByCode(string dictype)
        {
            var result = await _mediator.Send(new Features.Dictionary.SelectOption.List.Query(dictype,
                Request.QueryString.Value.Replace("?", "").Replace("=", "").Split(',')));

            return Ok(result);
        }

        [HttpGet("getBaseTreeNode/{dictype}")]
        public async Task<IActionResult> GetBaseTreeNode(string dictype)
        {
            var result = await _mediator.Send(new Features.Dictionary.Base.ListTreeNode.Query(dictype));
            return Ok(result);
        }
        [HttpGet("getDetailIcgs/{icgsId}")]
        public async Task<IActionResult> GetDetailIcgs(int icgsId)
        {
            var result = await _mediator.Send(new Features.Dictionary.DetailIcgs.List.Query(icgsId));
            return Ok(result);
        }
        [HttpGet("getDicTariffs/{protectionDocTypeId}")]
        public async Task<IActionResult> GetDicTariffs(int protectionDocTypeId)
        {
            var result = await _mediator.Send(new Features.Dictionary.DicTariff.List.Query(protectionDocTypeId));
            return Ok(result);
        }

        //TODO: Перейти на automapper и передавать связи многие-ко-многим в виде массива внешних ключей в обеих сущностях
        [HttpGet("routestageorder")]
        public async Task<IActionResult> GetRouteStageOrder()
        {
            var result = await Task.FromResult(_context.RouteStageOrders);

            return Ok(result);
        }
    }
}
