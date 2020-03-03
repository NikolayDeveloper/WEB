using System;
using System.Collections.Generic;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations
{
    public class ValidateGetPaySumArgument : IValidateGetPaySumArgument
    {
        public string GetValidationErrors(GetPaySumArgument argument)
        {
            var errors = new List<string>();
            if (argument.MainDocumentType == null)
                errors.Add("MainDocumentType is null");
            if (argument.DocumentType == null)
                errors.Add("DocumentType is null");
            return string.Join(Environment.NewLine, errors.ToArray());
        }
    }
}