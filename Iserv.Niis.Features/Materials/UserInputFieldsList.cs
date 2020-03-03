using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.Model.Models.Material;
using MediatR;

namespace Iserv.Niis.Features.Materials
{
    public class UserInputFieldsList
    {
        public class Query : IRequest<UserInputConfigDto>
        {
            public string Code { get; }

            public Query(string code)
            {
                Code = code;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, UserInputConfigDto>
        {
            private readonly ITemplateUserInputChecker _templateUserInputChecker;

            public QueryHandler(ITemplateUserInputChecker templateUserInputChecker)
            {
                _templateUserInputChecker = templateUserInputChecker;
            }

            public async Task<UserInputConfigDto> Handle(Query message)
            {
                _templateUserInputChecker.GetConfig(message.Code, out var config);

                return config;
            }
        }
    }
}
