using System;
using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    public class CheckPatentStatement
    {
        public class Query : IRequest<CheckPatentStatementResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "CheckPatentStatement"}; // TODO потом исправить
            public CheckPatentStatementArgument Argument { get; set; }
            public CheckPatentStatementResult Result { get; set; }
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
            private readonly IValidateCheckPatentStatementArgument _validateCheckPatent;

            public QueryValidator(
                IValidateCheckPatentStatementArgument checkPatentStatement,
                SystemInfoHelper systemInfoHelper)
            {
                _validateCheckPatent = checkPatentStatement;
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

                var error = _validateCheckPatent.GetValidationErrors(message.Argument);
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Ошибка валидации: {error}");
                }
            }
        }

        public class QueryHandler : IRequestHandler<Query, CheckPatentStatementResult>
        {
            private readonly ICheckPatentStatementService _checkPatentStatementService;

            public QueryHandler(ICheckPatentStatementService service)
            {
                _checkPatentStatementService = service;
            }

            public CheckPatentStatementResult Handle(Query message)
            {
                var statementFile = _checkPatentStatementService.GetStatementFile(message.Argument.Identifier,
                    message.Argument.GosNumber);
                _checkPatentStatementService.GetCheckPatentResult(message.Result, statementFile);
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

        public class QueryException : AbstractionExceptionHandler<Query, CheckPatentStatementResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override CheckPatentStatementResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.IsSuccess = false;
                message.Result.StatusRu = "Ошибка: " + Environment.NewLine + ex.Message;
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}