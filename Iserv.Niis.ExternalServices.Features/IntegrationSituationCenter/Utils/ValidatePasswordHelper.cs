using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.Utils.Helpers;
using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Utils
{
    public class ValidatePasswordHelper
    {
        private readonly AppConfiguration _configuration;

        public ValidatePasswordHelper(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValidPassword(string password)
        {
            // todo: Find out what is the password
            return true;

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            var hashPass = ShaHelper.GenerateSha256String(password);
            return _configuration.HashPassword.Equals(hashPass);
        }
    }
}