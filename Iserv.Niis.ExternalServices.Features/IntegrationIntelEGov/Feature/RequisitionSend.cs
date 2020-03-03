using System;
using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
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
    public class RequisitionSend
    {
        public class Command : IRequest<RequisitionSendResult>
        {
            public readonly LogAction LogAction = new LogAction {Type = "RequisitionSend"};
            public RequisitionSendArgument Argument { get; set; }
            public RequisitionSendResult Result { get; set; }
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
            private readonly IValidateRequisitionSendArgument _validateRequisition;

            public CommandValidator(SystemInfoHelper systemInfoHelper,
                IValidateRequisitionSendArgument validateRequisition)
            {
                _systemInfoHelper = systemInfoHelper;
                _validateRequisition = validateRequisition;
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
                var error = _validateRequisition.GetValidationErrors(message.Argument);
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Ошибка валидации: {error}");
                }
            }
        }

        public class Commandhandler : IRequestHandler<Command, RequisitionSendResult>
        {
            private readonly AppConfiguration _configuration;
            private readonly NiisWebContext _niisContext;
            private readonly IRequisitionSendService _requisitionSendService;

            public Commandhandler(NiisWebContext niisContext, IRequisitionSendService requisitionSendService,
                AppConfiguration configuration)
            {
                _niisContext = niisContext;
                _requisitionSendService = requisitionSendService;
                _configuration = configuration;
            }

            public RequisitionSendResult Handle(Command message)
            {
                using (var transaction = _niisContext.Database.BeginTransaction())
                {
                    try
                    {
                        var requestInfo = _requisitionSendService.RequisitionDocumentAdd(message.Argument);
                        if (message.Argument.BlockClassification != null)
                        {
                            _requisitionSendService.RequisitionMktuAdd(message.Argument.BlockClassification,
                                requestInfo.requestId);
                        }

                        if (message.Argument.BlockEarlyReg != null)
                        {
                            _requisitionSendService.RequisitionEarlyRegAdd(message.Argument.BlockEarlyReg,
                                requestInfo.requestId);
                        }

                        if (message.Argument.BlockColor != null)
                        {
                            _requisitionSendService.RequisitionColorAdd(message.Argument.BlockColor,
                                requestInfo.requestId, message.Argument.PatentType.UID);
                        }

                        if (message.Argument.BlockCustomer != null)
                        {
                            _requisitionSendService.RequisitionBlockCustomerAdd(message.Argument.BlockCustomer,
                                requestInfo.requestId);
                        }

                        if (message.Argument.BlockFile != null)
                        {
                            _requisitionSendService.RequisitionBlockFileAdd(message.Argument.BlockFile,
                                requestInfo.requestId, message.Argument.SystemInfo.Sender);
                        }

                        var integrationRequisition = new IntegrationRequisition
                        {
                            ChainId = message.Argument.SystemInfo.ChainId,
                            ProtectionDocTypeId = message.Argument.PatentType.UID,
                            Sender = message.Argument.SystemInfo.Sender,
                            StatusURL = _configuration.UrlServiceKazPatent,
                            RequestBarcode = requestInfo.barcode,
                            RequestNumber = requestInfo.incomingNum,
                            OnlineRequisitionStatusId = requestInfo.onlineStatusId,
                            Callback = "RequisitionSend"
                        };
                        _niisContext.IntegrationRequisitions.Add(integrationRequisition);
                        _niisContext.SaveChanges();
                        transaction.Commit();

                        message.Result.DocumentID = requestInfo.barcode;
                        message.Result.DocumentNumber = requestInfo.incomingNum;
                        message.Result.RequisitionStatus = requestInfo.onlineStatusId;
                    }
                    catch (Exception)
                    {
                        EntityFrameworkHelper.DetachAll(_niisContext);
                        transaction.Rollback();
                        throw;
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
                var messageLog =
                    $"вх. № {message.Result.DocumentNumber}, штрих-код {message.Result.DocumentID}";
                var infoSuccess = _systemInfoHelper.StatusInfoSuccess(messageLog);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(infoSuccess);
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
            }
        }

        public class CommandException : AbstractionExceptionHandler<Command, RequisitionSendResult>
        {
            private readonly SystemInfoHelper _systemInfoHelper;

            public CommandException(SystemInfoHelper systemInfoHelper)
            {
                _systemInfoHelper = systemInfoHelper;
            }

            public override RequisitionSendResult GetExceptionResult(Command message, Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                Log.Error(ex.InnerException ?? ex, errorMessage);
                message.Result.SystemInfo.Status = Mapper.Map<StatusInfo>(_systemInfoHelper.StatusInfoFail(ex));
                _systemInfoHelper.SaveAnswer(Mapper.Map<SystemInfoDto>(message.Result.SystemInfo), message.LogAction);
                message.Result.RequisitionStatus = -1;
                message.Result.DocumentID = -1;
                message.Result.DocumentNumber = string.Empty;
                return message.Result;
            }
        }
    }
}