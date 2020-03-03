using System;
using AutoMapper;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
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
    public class GetPayTariff
    {
        public class Query : IRequest<GetPayTarifResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "GetPayTariff"};
            public GetPayTarifArgument Argument { get; set; }
            public GetPayTarifResult Result { get; set; }
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

        public class QueryHandler : IRequestHandler<Query, GetPayTarifResult>
        {
            private readonly DictionaryHelper _dictionaryHelper;
            private readonly IGetPayTariffService _getPayTariffService;
            private readonly NiisWebContext _niisWebContext;

            public QueryHandler(
                NiisWebContext niisWebContext,
                IGetPayTariffService getPayTariffService,
                DictionaryHelper dictionaryHelper)
            {
                _niisWebContext = niisWebContext;
                _getPayTariffService = getPayTariffService;
                _dictionaryHelper = dictionaryHelper;
            }

            public GetPayTarifResult Handle(Query message)
            {
                var protectionDocTypeCode = _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType),
                    message.Argument.PatentType);

                var protectionDoc = _getPayTariffService.GetProtectionDoc(protectionDocTypeCode,
                    message.Argument.PatentN, message.Result);

                if (protectionDoc == null)
                {
                    message.Result.Result = PayTarifResult.Error;
                    message.Result.Message = "Охранный документ не найден";
                }
                else
                {
                    message.Result.Result = PayTarifResult.TarifFound;
                    message.Result.Message = "Охранный документ найден";

                    var discontinued = _getPayTariffService.GetDiscontinued(protectionDoc.StatusId,
                        protectionDocTypeCode, protectionDoc?.ValidDate?.DateTime.AddMonths(6),
                        protectionDoc.ExtensionDate?.DateTime);

                    if (discontinued)
                    {
                        message.Result.Result = PayTarifResult.Info;
                        message.Result.Message = "Срок действия ОД прекращен";
                    }
                    else
                    {
                        _getPayTariffService.GetTariffId(protectionDocTypeCode, protectionDoc.Id,
                            message.Result);

                        message.Result.Status = protectionDoc.Status.NameRu;
                        message.Result.PatentName = protectionDoc.NameRu;
                        message.Result.PatentUID = protectionDoc.ExternalId.Value;
                        message.Result.Validity =
                            _getPayTariffService.GetValidity(protectionDocTypeCode,
                                protectionDoc.ExtensionDate?.DateTime) ?? protectionDoc.ValidDate?.DateTime;
                        ;
                        if (protectionDoc.RegDate != null)
                        {
                            message.Result.RegDate = protectionDoc.RegDate.Value.DateTime;
                        }

                        if (protectionDoc.EarlyTerminationDate != null)
                        {
                            message.Result.DateTerminate = protectionDoc.EarlyTerminationDate.Value.DateTime;
                        }
                    }
                }

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

        public class QueryException : AbstractionExceptionHandler<Query, GetPayTarifResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override GetPayTarifResult GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                return message.Result;
            }
        }
    }
}