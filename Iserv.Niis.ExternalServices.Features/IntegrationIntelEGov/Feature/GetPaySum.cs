using System;
using System.Linq;
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
    public class GetPaySum
    {
        public class Query : IRequest<GetPaySumResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "GetPaySum"};
            public GetPaySumArgument Argument { get; set; }
            public GetPaySumResult Result { get; set; }
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
                _systemInfoHelper.LoggingSystemInfo(Mapper.Map<SystemInfoDto>(message.Argument.SystemInfo), message.LogAction);
            }
        }

        public class QueryValidator : AbstractCommonValidate<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;
            private readonly IValidateGetPaySumArgument _validateGetPaySum;

            public QueryValidator(SystemInfoHelper systemInfoHelper, IValidateGetPaySumArgument validateGetPaySum)
            {
                _systemInfoHelper = systemInfoHelper;
                _validateGetPaySum = validateGetPaySum;
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

                var error = _validateGetPaySum.GetValidationErrors(message.Argument);
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Ошибка валидации: {error}");
                }
            }
        }

        public class QueryHandler : IRequestHandler<Query, GetPaySumResult>
        {
            private readonly IGetPaySumService _getPaySumService;
            private readonly NiisWebContext _niisWebContext;

            public QueryHandler(NiisWebContext niisWebContext, IGetPaySumService getPaySumService)
            {
                _niisWebContext = niisWebContext;
                _getPaySumService = getPaySumService;
            }

            public GetPaySumResult Handle(Query message)
            {
                var argument = message.Argument;
                var countForTariff = 1;
                int? tariffId = null;
                var paymentCalcs = _getPaySumService.GetPaymentCalcs(argument.DocumentType.UID,
                    argument.MainDocumentType.UID, true);

                if (paymentCalcs.Any())
                {
                    tariffId = paymentCalcs.Select(x => x.TariffId).FirstOrDefault();
                }
                else
                {
                    if (argument.Count != null || argument.Count > 0)
                    {
                        paymentCalcs = _getPaySumService.GetPaymentCalcs(argument.DocumentType.UID,
                            argument.MainDocumentType.UID, false);
                        if (paymentCalcs.Any())
                        {
                            switch (paymentCalcs.Count())
                            {
                                case 1:
                                    tariffId = paymentCalcs.Select(x => x.TariffId).FirstOrDefault();
                                    countForTariff =
                                        _getPaySumService.GetCountForTariff((int) argument.Count, paymentCalcs);
                                    break;
                                case 2:
                                    tariffId = paymentCalcs.OrderBy(x => x.MinCount).Select(x => x.TariffId)
                                        .FirstOrDefault();
                                    countForTariff =
                                        _getPaySumService.GetCountForTariff((int) argument.Count, paymentCalcs);
                                    break;
                                default:
                                    return message.Result;
                            }
                        }
                    }
                }

                _getPaySumService.GetPaySum(message.Result, argument, countForTariff, tariffId);


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

        public class QueryException : AbstractionExceptionHandler<Query, GetPaySumResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override GetPaySumResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}