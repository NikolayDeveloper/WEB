using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Feature
{
    public class ContractApplicationSend
    {
        /// <summary>
        /// Входной запрос
        /// </summary>
        public class Query : IRequest<ContractResponse>
        {
            /// <summary>
            /// Имя Экшена для лога
            /// </summary>
            public readonly LogAction LogAction = new LogAction { Type = "ContractApplicationSend" };

            /// <summary>
            /// Параметры запроса данных
            /// </summary>
            public ContractRequest Request { get; set; }

            /// <summary>
            /// Объект ответа сервера
            /// </summary>
            public ContractResponse Responce { get; set; }
        }

        /// <summary>
        /// Логирование данных запроса
        /// </summary>
        public class QueryPreLogging : AbstractPreLogging<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryPreLogging(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Logging(Query message)
            {
                var systemInfoDto = Mapper.Map<SystemInfoDto>(message.Request);
                _systemInfoHelper.LoggingSystemInfo(systemInfoDto, message.LogAction);
            }
        }

        /// <summary>
        /// Валидирование параметров запроса
        /// </summary>
        public class QueryValidator : AbstractCommonValidate<Query>
        {
            private readonly SystemInfoHelper _systemInfoHelper;
            private readonly IValidateGetCustomerPatentValidityArgument _validateGetPaySum;

            public QueryValidator(SystemInfoHelper systemInfoHelper, IValidateGetCustomerPatentValidityArgument validateGetPaySum)
            {
                _systemInfoHelper = systemInfoHelper;
                _validateGetPaySum = validateGetPaySum;
            }

            public override void Validate(Query message)
            {
                var argumentSystemInfo = Mapper.Map<SystemInfoDto>(message.Request);
                var resultSystemInfo = _systemInfoHelper.InitializeSystemInfo(argumentSystemInfo);
                var systemInfo = Mapper.Map<SystemInfo>(resultSystemInfo);

                message.Responce.StatusInfo = systemInfo.StatusInfo;
                message.Responce.SenderInfo = systemInfo.SenderInfo;

                var systemInfoError = _systemInfoHelper.GetValidationSystemInfoError(argumentSystemInfo, resultSystemInfo, message.LogAction);
                if (!string.IsNullOrEmpty(systemInfoError))
                {
                    throw new Exception(systemInfoError);
                }

                //var error = _validateGetPaySum.GetValidationErrors(message.Request);
                //if (!string.IsNullOrEmpty(error))
                //{
                //    throw new Exception($"Ошибка валидации: {error}");
                //}
            }
        }

        /// <summary>
        /// Запрос на добавление договора
        /// </summary>
        public class QueryHandler : IRequestHandler<Query, ContractResponse>
        {
            private readonly IContractApplicationSendService _contractApplicationSendService;

            public QueryHandler(IContractApplicationSendService contractApplicationSendService)
            {
                _contractApplicationSendService = contractApplicationSendService;
            }

            public ContractResponse Handle(Query message)
            {
                var argument = message.Request;

                message.Responce = _contractApplicationSendService.AddContract(argument, message.Responce);

                return message.Responce;
            }
        }

        /// <summary>
        /// Логирование ответа по результатам добавления договора 
        /// </summary>
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
                message.Responce.StatusInfo = Mapper.Map<StatusInfo>(infoSuccess);
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Responce), message.LogAction);
            }
        }

        /// <summary>
        /// Обработка ошибок запроса и логирование
        /// </summary>
        public class QueryException : AbstractionExceptionHandler<Query, ContractResponse>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public QueryException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override ContractResponse GetExceptionResult(Query message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Responce.StatusInfo = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Responce), message.LogAction);
                return message.Responce;
            }
        }
    }
}
