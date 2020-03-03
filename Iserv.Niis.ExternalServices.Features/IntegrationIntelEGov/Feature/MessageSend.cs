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
using Iserv.Niis.Utils.Helpers;
using MediatR;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    public class MessageSend
    {
        public class Command : IRequest<MessageSendResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "MessageSend"};
            public MessageSendArgument Argument { get; set; }
            public MessageSendResult Result { get; set; }
        }

        public class CommandPreLogging : AbstractPreLogging<Command>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public CommandPreLogging(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Logging(Command message)
            {
                _systemInfoHelper.LoggingSystemInfo(Mapper.Map<SystemInfoDto>(message.Argument.SystemInfo),
                    message.LogAction);
            }
        }

        public class CommandValidator : AbstractCommonValidate<Command>
        {
            private readonly SystemInfoHelper _systemInfoHelper;
            private readonly IValidateMessageSendArgument _validateMessageSendArgument;

            public CommandValidator(SystemInfoHelper systemInfoHelper,
                IValidateMessageSendArgument validateMessageSendArgument)
            {
                _systemInfoHelper = systemInfoHelper;
                _validateMessageSendArgument = validateMessageSendArgument;
            }

            public override void Validate(Command message)
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
                var error = _validateMessageSendArgument.GetValidationErrors(message.Argument);
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Ошибка валидации: {error}");
                }
            }
        }

        public class CommandHandler : IRequestHandler<Command, MessageSendResult>
        {
            private readonly IMessageSendService _messageSendService;
            private readonly NiisWebContext _niisWebContext;

            public CommandHandler(IMessageSendService messageSendService, NiisWebContext niisWebContext)
            {
                _messageSendService = messageSendService;
                _niisWebContext = niisWebContext;
            }

            public MessageSendResult Handle(Command message)
            {
                using (var transaction = _niisWebContext.Database.BeginTransaction())
                {
                    
                    try
                    {
                        var document = _messageSendService.CorrespondenceAdd(message.Argument);
                        if (document != null)
                        {
                            message.Result.DocumentNumber = document.DocumentNum;
                            message.Result.DocumentUID = document.Barcode;
                            message.Result.DocumentDate = document.DateCreate.DateTime;
                        }

                        var paymentDocument = _messageSendService.PaymentDocumentAdd(message.Argument, document.Id);
                        if (paymentDocument != null)
                        {
                            message.Result.PaymentDocumentNumber = paymentDocument.DocumentNum;
                            message.Result.PaymentDocumentUID = paymentDocument.Barcode;
                            message.Result.PaymentDocumentDate = paymentDocument.DateCreate.DateTime;
                        }
                        _niisWebContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        EntityFrameworkHelper.DetachAll(_niisWebContext);
                        transaction.Rollback();
                        throw new Exception("Смотри внутренний Exception", ex);
                    }
                }
                return message.Result;
            }
        }

        public class CommandPostLogging : AbstractPostLogging<Command>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public CommandPostLogging(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override void Logging(Command message)
            {
                var infoSuccess = _systemInfoHelper.StatusInfoSuccess();
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(infoSuccess);
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
            }
        }

        public class CommandException : AbstractionExceptionHandler<Command, MessageSendResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public CommandException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override MessageSendResult GetExceptionResult(Command message, Exception ex)
            {
                Log.Error(ex, ex.Message);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                message.Result.DocumentNumber = null;
                message.Result.DocumentDate = null;
                message.Result.DocumentUID = -1;
                return message.Result;
            }
        }
    }
}