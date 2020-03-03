using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCommon;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDetailICGS;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Dictionaries.DicIPC;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Dictionaries.DicTariffs;
using Iserv.Niis.BusinessLogic.Dictionaries.RouteStageOrders;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Dictionaries;
using Iserv.Niis.Model.Models.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/dictionaries")]
    public class DictionariesController : Controller
    {
        private readonly IExecutor _executor;
        private IMemoryCache cache;
        private readonly IMapper _mapper;

        public DictionariesController(IExecutor executor, IMapper mapper, IMemoryCache memoryCache)
        {
            _executor = executor;
            _mapper = mapper;
            cache = memoryCache;
        }

        [HttpGet("{dictionaryType}")]
        public async Task<IActionResult> Get(DictionaryType dictionaryType)
        {
            List<dynamic> dictionaries;

            if (Request.Query.Count > 0)
            {
                dictionaries = await _executor.GetQuery<GetDictionaryByEntityNameAndFilterByColumnsQuery>()
                    .Process(r => r.ExecuteAsync(dictionaryType, Request));
            }
            else
            {
                if (!cache.TryGetValue(dictionaryType, out dictionaries))
                {
                    dictionaries = await _executor.GetQuery<GetDictionaryByEntityNameQuery>().Process(r => r.ExecuteAsync(dictionaryType));
                    if (dictionaries != null)
                    {
                        cache.Set(dictionaryType, dictionaries, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                    }
                }
            }

            return Ok((dictionaries ?? throw new InvalidOperationException()).Where(d => d.IsDeleted != true));
        }

        #region Метод для получения статусов заявки.
        /// <summary>
        /// Метод для получения статусов заявки.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDicRequestStatusForExpertSearch")]
        public async Task<IActionResult> GetDicRequestStatusForExpertSearch()
        {
            var dicRequestStatuses = await _executor.GetQuery<GetDicRequestStatusForExpertSearchQuery>().Process(r => r.ExecuteAsync());
            return Ok(dicRequestStatuses);
        }
        #endregion

        #region Метод для получения статусов документов.
        /// <summary>
        /// Метод для получения статусов документов.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDicProtectionDocStatusForExpertSearch")]
        public async Task<IActionResult> GetDicProtectionDocStatusForExpertSearch()
        {
            var dicProtectionDocStatuses = await _executor.GetQuery<GetDicProtectionDocStatusForExpertSearchQuery>().Process(r => r.ExecuteAsync());
            return Ok(dicProtectionDocStatuses);
        }
        #endregion

        [HttpGet("{dictionaryType}/{id}")]
        public async Task<IActionResult> Get(DictionaryType dictionaryType, int id)
        {
            var key = dictionaryType + "_" + id;
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDictionaryRowByEntityNameAndIdQuery>().Process(r => r.ExecuteAsync(dictionaryType, id));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }
            
            var result = _mapper.Map<BaseDictionaryDto>(dictionary);
            return Ok(result);
        }

        [HttpGet("{dictionaryType}/bycodes")]
        public async Task<IActionResult> GetByCodes(DictionaryType dictionaryType)
        {
            var codes = Request.QueryString.Value.Replace("?", "").Replace("=", "").Split(',').ToList();

            var key = dictionaryType + "_code_" + String.Join("_",codes);
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDictionaryByEntityNameAndCodesQuery>().Process(r => r.ExecuteAsync(dictionaryType, codes));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }
            
            var result = _mapper.Map<List<BaseDictionaryDto>>(dictionary);
            return Ok(result);
        }

        /// <summary>
        /// API для получения справочных данных в формате ключ-значение
        /// </summary>
        /// <param name="dictionaryType">Тип справочника</param>
        /// <returns></returns>
        [HttpGet("{dictionaryType}/select")]
        public async Task<IActionResult> GetSelectOptions(DictionaryType dictionaryType)
        {
            if (!cache.TryGetValue(dictionaryType, out List<dynamic> dictionaries))
            {
                dictionaries = await _executor.GetQuery<GetDictionaryByEntityNameQuery>().Process(r => r.ExecuteAsync(dictionaryType));
                if (dictionaries != null)
                {
                    cache.Set(dictionaryType, dictionaries, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            return Ok((dictionaries ?? throw new InvalidOperationException()).Where(d => d.IsDeleted != true));
        }

        /// <summary>
        /// API для получения справочных данных в формате ключ-значение
        /// </summary>
        /// <param name="dictionaryType">Тип справочника</param>
        /// <param name="id">Идентефикатор</param>
        /// <returns></returns>
        [HttpGet("{dictionaryType}/select/{id}")]
        public async Task<SelectOptionDto> GetSelectOption(DictionaryType dictionaryType, int id)
        {
            var key = dictionaryType + "_" + id;
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDictionaryRowByEntityNameAndIdQuery>().Process(r => r.ExecuteAsync(dictionaryType, id));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            if (dictionary == null)
                throw new DataNotFoundException(nameof(dictionaryType), DataNotFoundException.OperationType.Read, id);

            return _mapper.Map<SelectOptionDto>(dictionary);
        }

        [HttpGet("{dictionaryType}/select/bycodes")]
        public async Task<IActionResult> GetSelectOptionByCode(DictionaryType dictionaryType)
        {
            var codes = Request.QueryString.Value.Replace("?", "").Replace("=", "").Split(',').ToList();

            var key = dictionaryType + "_code_" + codes;
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDictionaryByEntityNameAndCodesQuery>().Process(r => r.ExecuteAsync(dictionaryType, codes));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<List<SelectOptionDto>>(dictionary);
            return Ok(result);
        }


        [HttpGet("getBaseTreeNode/{dictionaryType}")]
        public async Task<IActionResult> GetBaseTreeNode(DictionaryType dictionaryType)
        {
            var key = dictionaryType + "_baseTreeNode";
            if (!cache.TryGetValue(key, out List<DictionaryEntity<int>> dictionary))
            {
                dictionary = await _executor.GetQuery<GetDitionaryTreeQuery>().Process(r => r.ExecuteAsync(dictionaryType));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var baseTreeNodeDtos = _mapper.Map<IEnumerable<BaseTreeNodeDto>>((dictionary ?? throw new InvalidOperationException()).Where(d => d.IsDeleted != true).ToList());

            return Ok(baseTreeNodeDtos);
        }

        
        [HttpGet("getDetailIcgs/{icgsId}")]
        public async Task<IActionResult> GetDetailIcgs(int icgsId)
        {
            var key = icgsId + "_detail";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicDetailICGSByIcgsIdQuery>().Process(r => r.ExecuteAsync(icgsId));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }
            
            var result = _mapper.Map<List<SelectOptionDto>>(dictionary);

            return Ok(result);
        }

        [HttpGet("getDicTariffs/{protectionDocTypeId}")]
        public async Task<IActionResult> GetDicTariffs(int protectionDocTypeId)
        {
            var key = protectionDocTypeId + "_tariffs";
            if (!cache.TryGetValue(key, out List<DicTariff> dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicTariffsByProtectionDocTypeIdQuery>().Process(r => r.ExecuteAsync(protectionDocTypeId));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            return Ok((dictionary ?? throw new InvalidOperationException()).Where(d => d.IsDeleted != true));
        }
        
        //TODO: Перейти на automapper и передавать связи многие-ко-многим в виде массива внешних ключей в обеих сущностях
        [HttpGet("routestageorder")]
        public async Task<IActionResult> GetRouteStageOrder()
        {
            var key = "routestageorder";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetRouteStageOrderAllQuery>().Process(r => r.Execute());
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<List<RouteStageOrderDto>>(dictionary);
            return Ok(result);
        }

        [HttpGet("getDicRouteStages")]
        public async Task<IActionResult> GetDicRouteStages()
        {
            var key = "dicRouteStages";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicRouteStagesQuery>().Process(q => q.ExecuteAsync());
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<List<DicRouteStageDto>>(dictionary);
            return Ok(result);
        }

        [HttpGet("getDicIpcChildren/{parentId}")]
        public async Task<IActionResult> GetDicIpcChildren(int parentId)
        {
            var key = parentId + "_dicIpcChildren";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicIpcChildrenByParentIdQuery>().Process(q => q.ExecuteAsync(parentId));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionary);
            return Ok(result);
        }

        [HttpGet("getDicIpcRoots")]
        public async Task<IActionResult> GetDicIpcRoots()
        {
            var key = "dicIpcRoots";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicIpcRootsQuery>().Process(q => q.ExecuteAsync());
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionary);
            return Ok(result);
        }

        [HttpPost("getDicIpcs")]
        public async Task<IActionResult> GetDicIpcs([FromBody] int[] ipcIds)
        {
            if (ipcIds == null)
            {
                return NoContent();
            }

            var key = string.Join(",", ipcIds.OrderBy(d => d)) + "_dicIpcs";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicIpcsByIdsQuery>().Process(q => q.ExecuteAsync(ipcIds.ToList()));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionary);
            return Ok(result);
        }

        [HttpPost("searchDicIpc")]
        public async Task<IActionResult> GetDicIpcsByNameRuOrCode([FromBody] StringWrapper searchText)
        {
            var key = searchText.Str + "_searchDicIpc";
            if (!cache.TryGetValue(key, out var dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicIpcsByNameRuOrCodeQuery>().Process(q => q.ExecuteAsync(searchText.Str));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionary);
            return Ok(result);
        }

        [HttpGet("getDicICFEMColors")]
        public async  Task<IActionResult> GetDicIcfemColors()
        {
            //var dicICFEM = await _executor.GetQuery<GetDitionaryTreeQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicICFEM));
            var key = "dicICFEMColors_DicICFEM";
            if (!cache.TryGetValue(key, out List<DictionaryEntity<int>> dictionary))
            {
                dictionary = await _executor.GetQuery<GetDitionaryTreeQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicICFEM));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var dicIcfemColors = (dictionary ?? throw new InvalidOperationException()).Where(d => d.Code == DicICFEMCodes.Colors);

            //var dicColors = await _executor.GetQuery<GetDictionaryByEntityNameQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicColorTZ));
            var keyTwo = "dicICFEMColors_DicColorTZ";
            if (!cache.TryGetValue(keyTwo, out List<dynamic> colors))
            {
                colors = await _executor.GetQuery<GetDictionaryByEntityNameQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicColorTZ));
                if (colors != null)
                {
                    cache.Set(keyTwo, colors, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            UpdateIds(colors, dicIcfemColors);

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dicIcfemColors);
            return Ok(result);
        }

        [HttpGet("getDicICISs")]
        public async Task<IActionResult> GetDicICISs()
        {
            //var dicICISs = await _executor.GetQuery<GetDitionaryTreeQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicICIS));
            var key = "dicICISs";
            if (!cache.TryGetValue(key, out List<DictionaryEntity<int>> dictionary))
            {
                dictionary = await _executor.GetQuery<GetDitionaryTreeQuery>().Process(r => r.ExecuteAsync(DictionaryType.DicICIS));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var result = _mapper.Map<IEnumerable<BaseTreeNodeDto>>(dictionary);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает <see cref="Dictionary{TKey,TValue}"/> где ключ это код типа документа, а значение это идентификатор типа документа.
        /// </summary>
        /// <param name="codes"> Коды типов документов.</param>
        /// <returns> <see cref="Dictionary{TKey,TValue}"/> где ключ это код типа документа, а значение это идентификатор типа документа/</returns>
        [HttpPost("getDocumentTypeIds")]
        public async Task<IActionResult> GetDocumentTypes([FromBody] IEnumerable<string> codes)
        {
            //var documentTypes = await _executor.GetQuery<GetDicDocumentTypesByCodesQuery>().Process(query => query.ExecuteAsync(codes));

            var key = "getDocumentTypeIds";
            if (!cache.TryGetValue(key, out List<DicDocumentType> dictionary))
            {
                dictionary = await _executor.GetQuery<GetDicDocumentTypesByCodesQuery>().Process(query => query.ExecuteAsync(codes));
                if (dictionary != null)
                {
                    cache.Set(key, dictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            var idsWithCodes = new Dictionary<string, int>();

            if (dictionary != null)
            {
                foreach (var documentType in dictionary.Where(d => d.IsDeleted == false))
                {
                    idsWithCodes.Add(documentType.Code, documentType.Id);
                }
            }

            return Ok(idsWithCodes);
        }

        /// <summary>
        /// Строим дерево цветов по справочнику венской классификации но Id Берем из справочника цвета, удалить после переделки логики работы со справочником цвета.
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="items"></param>
        private void UpdateIds(List<dynamic> colors, IEnumerable<dynamic> items)
        {
            foreach (var dicIcfemColor in items)
            {
                var dicColor = colors.FirstOrDefault(d => d.ExternalId == dicIcfemColor.ExternalId);

                if (dicColor != null)
                    dicIcfemColor.Id = dicColor.Id;

                if (dicIcfemColor.Childs != null && dicIcfemColor.Childs.Count > 0)
                    UpdateIds(colors, dicIcfemColor.Childs);
            }
        }
    }
}