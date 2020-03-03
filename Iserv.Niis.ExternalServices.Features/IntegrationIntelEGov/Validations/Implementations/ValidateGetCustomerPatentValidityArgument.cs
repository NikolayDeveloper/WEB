using System;
using System.Collections.Generic;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations
{
    public class ValidateGetCustomerPatentValidityArgument : IValidateGetCustomerPatentValidityArgument
    {
        public string GetValidationErrors(CustomerPatentValidityRequest request)
        {
            var errors = new List<string>();
            if (request.GosNumber == null)
                errors.Add("GosNumber is null");
            if (request.PatentType == null)
                errors.Add("PatentType is null");
            return string.Join(Environment.NewLine, errors.ToArray());
        }
    }
}