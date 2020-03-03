using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Utils;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Features
{
    public class TypeInfoList
    {
        public class Query : IRequest<GetReferenceResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "GetReference"};
            public GetReferenceArgument Argument { get; set; }
            public GetReferenceResult Result { get; set; }
        }

        public class QueryPreLogging : AbstractPreLogging<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryPreLogging(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Logging(Query message)
            {
                var systemInfoDto = _systemInfoHelper.InitializeSystemInfo();
                message.Result.SystemInfo = Mapper.Map<SystemInfo>(systemInfoDto);
                _systemInfoHelper.LoggingSystemInfo(systemInfoDto, message.LogAction);
            }
        }

        public class QueryValidator : AbstractCommonValidate<Query>
        {
            private readonly ValidatePasswordHelper _passwordHelper;

            public QueryValidator(ValidatePasswordHelper passwordHelper)
            {
                _passwordHelper = passwordHelper;
            }
            public override void Validate(Query message)
            {
                if (!_passwordHelper.IsValidPassword(message.Argument.Password))
                {
                    throw new Exception("Указан неверный пароль");
                }
            }
        }

        public class QueryHandler : IRequestHandler<Query, GetReferenceResult>
        {
            private readonly ITypeInfoService _typeInfoService;

            public QueryHandler(ITypeInfoService typeInfoService)
            {
                _typeInfoService = typeInfoService;
            }

            public GetReferenceResult Handle(Query message)
            {
                _typeInfoService.GetTypesInfo(message.Argument, message.Result);
                return message.Result;
            }
        }

        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryPostLogging(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Logging(Query message)
            {
                var infoSuccess = _systemInfoHelper.StatusInfoSuccess();
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(infoSuccess);
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
            }
        }

        public class QueryException : AbstractionExceptionHandler<Query, GetReferenceResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override GetReferenceResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.Items = null;
                var statusFail = _systemInfoHelper.StatusInfoFail(ex);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(statusFail);
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}