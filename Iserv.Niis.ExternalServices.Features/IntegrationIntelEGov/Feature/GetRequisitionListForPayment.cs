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
    public class GetRequisitionListForPayment
    {
        public class Query : IRequest<GetRequisitionListForPaymentResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "GetRequisitionListForPayment"};
            public GetRequisitionListForPaymentArgument Argument { get; set; }
            public GetRequisitionListForPaymentResult Result { get; set; }
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
                if (string.IsNullOrEmpty(message.Argument.XIN) || message.Argument.XIN.Length != 12)
                {
                    throw new Exception("Ошибка валидации: Некорректный ИИН/БИН.");
                }
                if (message.Argument.DocumentType == null || message.Argument.PatentType == null ||
                    string.IsNullOrEmpty(message.Argument.XIN))
                {
                    throw new Exception("Ошибка валидации: Неверные входные данные");
                }
            }
        }

        public class QueryHandler : IRequestHandler<Query, GetRequisitionListForPaymentResult>
        {
            private readonly IGetRequisitionListForPaymentService _getRequisitionListForPaymentService;

            public QueryHandler(IGetRequisitionListForPaymentService getRequisitionListForPaymentService)
            {
                _getRequisitionListForPaymentService = getRequisitionListForPaymentService;
            }

            public GetRequisitionListForPaymentResult Handle(Query message)
            {
                _getRequisitionListForPaymentService.Handle(message.Argument, message.Result);
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

        public class QueryException : AbstractionExceptionHandler<Query, GetRequisitionListForPaymentResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override GetRequisitionListForPaymentResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}