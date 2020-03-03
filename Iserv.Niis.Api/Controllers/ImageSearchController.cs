using AutoMapper;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.ExpertSearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ImageSearch")]
    public class ImageSearchController : Controller
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        private TdmProvider _tdmProvider;
        private readonly IFileStorage _fileStorage;

        public ImageSearchController(
            IExecutor executor,
            IMapper mapper,
            IConfiguration configuration,
            IFileStorage fileStorage)
        {
            _executor = executor;
            _mapper = mapper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            InitializeFormUploadFromConfig();
        }

        // GET: api/ImageSearch/5/searchbyimage
        [AllowAnonymous]
        [HttpGet("{id}/searchbyimage")]
        public async Task<IActionResult> SearchByImage(int id)
        {
            var request = await _executor.GetQuery<GetRequsetImageByIdQuery>().Process(r => r.ExecuteAsync(id));

            var image = request?.Image;

            var imageResponse = string.Empty;
            var phoneticResponse = string.Empty;
            var semanticResponse = string.Empty;
            imageResponse = _tdmProvider.GetResultsSearchByImage(image);

            var tradeMarkCollection =
                await DeserializeTrademarkSearchedResult(imageResponse, phoneticResponse, semanticResponse);

            return Ok(tradeMarkCollection.AsQueryable());
        }

        // GET: api/ImageSearch/5/imageandphonetic
        [AllowAnonymous]
        [HttpGet("{id}/searchbyimageandphonetic")]
        public async Task<IActionResult> SearchByImageAndPhonetic(int id)
        {
            var request = await _executor.GetQuery<GetRequsetImageByIdQuery>().Process(r => r.ExecuteAsync(id));

            var image = request?.Image;

            var imageResponse = string.Empty;
            var phoneticResponse = string.Empty;
            var semanticResponse = string.Empty;
            imageResponse = _tdmProvider.GetResultsSearchByImage(image);

            var names = _executor.GetQuery<GetRequestNamesByIdQuery>().Process(r => r.Execute(id));
            var name = string.Empty;
            if (names != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = names.NameRu;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = names.NameKz;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = names.NameEn;
                }
            }

            phoneticResponse = string.IsNullOrEmpty(name) ? "" : _tdmProvider.GetResultsSearchByPhonetic(name);
            semanticResponse = string.IsNullOrEmpty(name) ? "" : _tdmProvider.GetResultsSearchBySemantic(name);

            var tradeMarkCollection =
                await DeserializeTrademarkSearchedResult(imageResponse, phoneticResponse, semanticResponse);

            return Ok(tradeMarkCollection.AsQueryable());
        }


        private void InitializeFormUploadFromConfig()
        {
            var formUploadConfig = _configuration.GetSection("TdmConnectionString");

            var postImageUrl = formUploadConfig["PostImageUrl"];
            var postPhoneticUrl = formUploadConfig["PostPhoneticUrl"];
            var postSemanticUrl = formUploadConfig["PostSemanticUrl"];
            var contentTypeMultipartPattern = formUploadConfig["ContentTypeMultipartPattern"];
            var contentType = formUploadConfig["ContentType"];
            var userAgent = formUploadConfig["UserAgent"];
            _tdmProvider = new TdmProvider(postImageUrl, postPhoneticUrl, postSemanticUrl,
                contentTypeMultipartPattern, contentType, userAgent);
        }

        /// <summary>
        /// Парсится ответ с сервера ТДМ, ДТОхам инициализируются поля, указывающие процент схожести
        /// по типам поиска, из базы тянутся иконки товарных знаков и вставляются в ДТО
        /// </summary>
        /// <param name="imageResponse">Ответ сервера ТДМ по поиску схожести изображения ТМ</param>
        /// <param name="phoneticResponse">Ответ сервера ТДМ по поиску схожести по фонетике ТМ</param>
        /// <param name="semanticResponse">Ответ сервера ТДМ по поиску схожести по семантике ТМ</param>
        /// <returns></returns>
        private async Task<List<TrademarkDto>> DeserializeTrademarkSearchedResult(string imageResponse, string phoneticResponse, string semanticResponse)
        {
            var tradeMarkCollection = new List<TrademarkDto>();

            tradeMarkCollection.AddRange(await ParseServerResponse(imageResponse, SimilarityType.Image));
            tradeMarkCollection.AddRange(await ParseServerResponse(phoneticResponse, SimilarityType.Phonetic));
            tradeMarkCollection.AddRange(await ParseServerResponse(semanticResponse, SimilarityType.Semantic));

            return tradeMarkCollection;
        }

        private async Task<List<TrademarkDto>> ParseServerResponse(string response, SimilarityType type)
        {
            var tradeMarkCollection = new List<TrademarkDto>();

            if (!string.IsNullOrEmpty(response))
            {
                var results = JsonConvert.DeserializeObject<dynamic>(response);
                foreach (var result in results.results)
                {
                    int.TryParse(result.barcode.ToString(), out int supervisorBarcode);
                    var protDoc = _executor.GetQuery<GetProtectionDocsByBarcodeQuery>()
                        .Process(q => q.Execute(supervisorBarcode));

                    if (protDoc != null)
                    {
                        var tdmDet = _mapper.Map<TrademarkDto>(protDoc);
                        switch (type)
                        {
                            case SimilarityType.Image:
                                tdmDet.ImageSimilarity = result.similarity;
                                break;
                            case SimilarityType.Phonetic:
                                tdmDet.PhonSimilarity = result.similarity;
                                break;
                            case SimilarityType.Semantic:
                                tdmDet.SemSimilarity = result.similarity;
                                break;
                        }

                        byte[] responseImage = protDoc.Image;

                        tdmDet.PreviewImage =
                            responseImage != null ? $"data:image/png;base64,{Convert.ToBase64String(responseImage)}" : null;
                        tradeMarkCollection.Add(tdmDet);
                    }
                }
            }

            return tradeMarkCollection;
        }
    }

    internal enum SimilarityType
    {
        Image,
        Phonetic,
        Semantic
    }
}