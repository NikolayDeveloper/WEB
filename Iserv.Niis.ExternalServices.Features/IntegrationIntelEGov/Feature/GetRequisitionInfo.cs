using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    public class GetRequisitionInfo
    {
        public class Query : IRequest<GetRequisitionInfoResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "GetRequisitionInfo"};
            public GetRequisitionInfoArgument Argument { get; set; }
            public GetRequisitionInfoResult Result { get; set; }
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
                _systemInfoHelper.LoggingSystemInfo(Mapper.Map<SystemInfoDto>(message.Argument.SystemInfo),
                    message.LogAction);
            }
        }

        public class QueryValidator : AbstractCommonValidate<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryValidator(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Validate(Query message)
            {
                var argumentSystemInfo = Mapper.Map<SystemInfoDto>(message.Argument.SystemInfo);
                var resultSystemInfo = _systemInfoHelper.InitializeSystemInfo(argumentSystemInfo);
                message.Result.SystemInfo = Mapper.Map<SystemInfo>(resultSystemInfo);

                var systemInfoError =
                    _systemInfoHelper.GetValidationSystemInfoError(argumentSystemInfo, resultSystemInfo,
                        message.LogAction);

                if (!string.IsNullOrEmpty(systemInfoError))
                {
                    throw new Exception(systemInfoError);
                }
            }
        }

        public class QueryHandler : IRequestHandler<Query, GetRequisitionInfoResult>
        {
            private readonly IGetRequisitionInfoService _requisitionInfoService;

            public QueryHandler(IGetRequisitionInfoService requisitionInfoService)
            {
                _requisitionInfoService = requisitionInfoService;
            }

            public GetRequisitionInfoResult Handle(Query message)
            {
                _requisitionInfoService.GetRequisitionInfo(message.Argument, message.Result);
                return message.Result;
            }
        }

        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryPostLogging(
                SystemInfoHelper systemInfoHelper)
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

        public class QueryException : AbstractionExceptionHandler<Query, GetRequisitionInfoResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override GetRequisitionInfoResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}