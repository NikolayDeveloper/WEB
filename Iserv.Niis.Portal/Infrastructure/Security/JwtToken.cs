﻿using System;
using System.IdentityModel.Tokens.Jwt;

namespace Iserv.Niis.Portal.Infrastructure.Security
{
    public sealed class JwtToken
    {
        private readonly JwtSecurityToken _token;

        internal JwtToken(JwtSecurityToken token)
        {
            this._token = token;
        }

        public DateTime ValidTo => _token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this._token);
    }
}
