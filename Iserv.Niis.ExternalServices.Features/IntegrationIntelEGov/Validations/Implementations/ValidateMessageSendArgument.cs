using System;
using System.Collections.Generic;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations
{
    public class ValidateMessageSendArgument : IValidateMessageSendArgument
    {
        private readonly IntegrationValidationHelper _validationHelper;

        public ValidateMessageSendArgument(IntegrationValidationHelper validationHelper)
        {
            _validationHelper = validationHelper;
        }

        public string GetValidationErrors(MessageSendArgument argument)
        {
            var errors = new List<string>();

            if (argument.DocumentType == null)
            {
                errors.Add("Тип документа не может быть пустым");
            }
            else
            {
                if (argument.DocumentType.UID <= 0)
                    errors.Add("Некорректный тип документа");
            }

            if (_validationHelper.FileIsEmpty(argument.File, argument))
                errors.Add("Не добавлен файл!");

            if (!_validationHelper.FileNameIsCorrect(argument.File, argument))
            {
                errors.Add("Не корректное имя файла!");
            }
            else
            {
                if (!_validationHelper.CheckFileExtension(new[] {".pdf"}, argument.File, argument))
                    errors.Add("Файл должен быть в формате \".pdf\"");
            }
            if (errors.Count == 0)
                return null;
            return string.Join(Environment.NewLine, errors.ToArray());
        }
    }
}