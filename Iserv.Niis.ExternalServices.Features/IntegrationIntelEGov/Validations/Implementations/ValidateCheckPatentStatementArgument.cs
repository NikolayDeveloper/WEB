using System;
using System.Collections.Generic;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations
{
    public class ValidateCheckPatentStatementArgument : IValidateCheckPatentStatementArgument
    {
        public string GetValidationErrors(CheckPatentStatementArgument argument)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(argument.GosNumber))
                errors.Add("GosNumber is null or empty");
            if (string.IsNullOrEmpty(argument.Identifier))
                errors.Add("Identifier is null or empty");
            return string.Join(Environment.NewLine, errors.ToArray());
        }
    }
}