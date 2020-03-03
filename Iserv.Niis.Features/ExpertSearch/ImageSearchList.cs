using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Features.ExpertSearch
{
    public class ImageSearchList
    {
        public class Query : IRequest<IQueryable<TrademarkDto>>
        {
            public Query(int requestId, bool isPhonetic, int userId)
            {
                RequestId = requestId;
                IsPhonetic = isPhonetic;
                UserId = userId;
            }

            public bool IsPhonetic { get; set; }

            public int RequestId { get; }
            public int UserId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<TrademarkDto>>
        {
            private readonly IConfiguration _configuration;
            private readonly NiisWebContext _context;

            private readonly IFileStorage _fileStorage;
            private readonly IMapper _mapper;
            private TdmProvider _tdmProvider;

            public QueryHandler(IMapper mapper, NiisWebContext context, IConfiguration configuration,
                IFileStorage fileStorage)
            {
                _mapper = mapper;
                _context = context;
                _configuration = configuration;
                _fileStorage = fileStorage;
                InitializeFormUploadFromConfig();
            }

            public async Task<IQueryable<TrademarkDto>> Handle(Query message)
            {
                var requestId = message.RequestId;

                var image = _context.Requests.SingleOrDefault(r => r.Id == requestId)?.Image;
                ICollection<TrademarkDto> collection = new HashSet<TrademarkDto>();

                var imageResponse = string.Empty;
                var phoneticResponse = string.Empty;
                var semanticResponse = string.Empty;
                try
                {
                    imageResponse = _tdmProvider.GetResultsSearchByImage(image);

                    if (message.IsPhonetic)
                    {
                        //return await Task.FromResult(collection.AsQueryable());

                        var nameRu = _context.Requests.Where(r => r.Id == requestId).Select(r => r.NameRu)
                                         .FirstOrDefault() ?? string.Empty;
                        var nameKz = _context.Requests.Where(r => r.Id == requestId).Select(r => r.NameKz)
                                         .FirstOrDefault() ?? string.Empty;
                        var nameEn = _context.Requests.Where(r => r.Id == requestId).Select(r => r.NameEn)
                                         .FirstOrDefault() ?? string.Empty;

                        phoneticResponse = nameRu != string.Empty
                            ? _tdmProvider.GetResultsSearchByPhonetic(nameRu)
                            : nameKz != string.Empty
                                ? _tdmProvider.GetResultsSearchByPhonetic(nameKz)
                                : nameEn != string.Empty
                                    ? _tdmProvider.GetResultsSearchByPhonetic(nameEn)
                                    : string.Empty;

                        semanticResponse = nameRu != string.Empty
                            ? _tdmProvider.GetResultsSearchBySemantic(nameRu)
                            : nameKz != string.Empty
                                ? _tdmProvider.GetResultsSearchBySemantic(nameKz)
                                : nameEn != string.Empty
                                    ? _tdmProvider.GetResultsSearchBySemantic(nameEn)
                                    : "";
                    }
                }
                catch
                {
                    //todo:log exection
                }
                await TdmResultsJsonProvider.ImageSearchResultsDeserializer(imageResponse, phoneticResponse,
                    semanticResponse, collection, _mapper, _context, _fileStorage);
                return collection.AsQueryable();
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
        }
    }
}