using System;
using AutoMapper;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.Models;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.Utils
{
    public class SystemInfoHelper
    {
        private readonly LoggingHelper _logging;

        public SystemInfoHelper(LoggingHelper logging)
        {
            _logging = logging;
        }
        public StatusInfoDto StatusInfoSuccess(string message = null)
        {
            return new StatusInfoDto
            {
                Code = "4",
                MessageRu = message ?? "Успешно",
                MessageKz = null
            };
        }


        public StatusInfoDto StatusInfoFail(Exception ex)
        {
            return new StatusInfoDto
            {
                Code = "-3",
                MessageRu = "Внутренняя ошибка.",
                MessageKz = ExceptionHelper.ExceptionFullText(ex)
            };
        }
        public StatusInfoDto StatusInfoFail(string errorText)
        {
            return new StatusInfoDto
            {
                Code = "-3",
                MessageRu = "Внутренняя ошибка.",
                MessageKz = errorText
            };
        }
        public StatusInfoDto StatusInfoFailArgument(string message)
        {
            return new StatusInfoDto
            {
                Code = "-5",
                MessageRu = "Некорректные входные данные",
                MessageKz = message
            };
        }

        public SystemInfoDto InitializeSystemInfo(SystemInfoDto argumentSystemInfo=null)
        {
            var systemInfo = new SystemInfoDto
            {
                MessageDate = DateTime.Now,
                MessageId = Guid.NewGuid().ToString(),
                Sender = CommonConstants.SystemInfoSenderNiis,
                AdditionalInfo = null,
                ChainId = null
            };
            if (argumentSystemInfo != null)
            {
                systemInfo.MessageId = argumentSystemInfo.MessageId;
                systemInfo.ChainId = argumentSystemInfo.ChainId;
            }
            return systemInfo;
        }

        public void LoggingSystemInfo(SystemInfoDto systemInfo, LogAction logAction)
        {
            logAction.SystemInfoQueryId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(systemInfo));
            _logging.CreateLogAction(logAction);
        }
        public void SaveAnswer(SystemInfoDto resultSystemInfo, LogAction logAction)
        {
            try
            {
                logAction.SystemInfoAnswerId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(resultSystemInfo));
                _logging.UpdateLogAction(logAction);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
        public string GetValidationSystemInfoError(SystemInfoDto argument, SystemInfoDto result, LogAction logAction)
        {
            if (argument == null)
            {
                result.Status = StatusInfoFail("SystemInfo отсутствует.");
                SaveAnswer(result, logAction);
                return "SystemInfo отсутствует.";
            }
            var sender = argument.Sender;
            if (string.IsNullOrEmpty(sender))
            {
                result.Status = StatusInfoFail("Sender не указан.");
                SaveAnswer(result, logAction);
                return "Sender не указан.";
            }
            if (!sender.Equals(CommonConstants.SenderKazPatent, StringComparison.OrdinalIgnoreCase) &&
                !sender.Equals(CommonConstants.SenderPep, StringComparison.OrdinalIgnoreCase))
            {
                result.Status = StatusInfoFail($"Sender {sender} неизвестен.");
                SaveAnswer(result, logAction);
                return $"Sender {sender} неизвестен.";
            }
            if (argument.ChainId == null)
            {
                result.Status = StatusInfoFail("ChainId не указан.");
                SaveAnswer(result, logAction);
                return "ChainId не указан.";
            }
            return null;
        }
    }
}