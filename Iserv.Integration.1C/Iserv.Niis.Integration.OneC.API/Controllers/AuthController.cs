using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Iserv.Niis.Integration.ApiResult.Models;
using Iserv.Niis.Integration.OneC.Infrastructure.Security;
using Iserv.Niis.Integration.OneC.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Iserv.Niis.Integration.OneC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Локальные переменные
        /// <summary>
        /// Представляет набор свойств конфигурации приложения ключ / значение.
        /// </summary>
        private readonly IConfiguration _configuration;
        #endregion

        /// <summary>
        /// Получение токена доступа.
        /// </summary>
        /// <param name="token">Модель для получения токена.</param>
        /// <returns>Токен доступа.</returns>
        [HttpPost(nameof(Token))]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetDataApiResult<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GetDataApiResult<string>), (int)HttpStatusCode.NotFound)]
        public IActionResult Token([FromBody] TokenDto token)
        {
            var jwtIssuerOptions = JwtBearerConfigureOptions.GetJwtIssuerOptions(_configuration);

            if (!jwtIssuerOptions.AccessKey.Contains(token.AccessKey) ||
                !jwtIssuerOptions.SecretKey.Contains(token.SecretKey))
                return NotFound<string>(null);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, token.AccessKey)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var utcNow = DateTime.UtcNow;

            // создаем JWT-токен
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtIssuerOptions.Issuer,
                notBefore: utcNow,
                claims: claimsIdentity.Claims,
                expires: utcNow.Add(TimeSpan.FromMinutes(jwtIssuerOptions.LifetimeInMinutes)),
                signingCredentials: new SigningCredentials(JwtBearerConfigureOptions.GetSymmetricSecurityKey(jwtIssuerOptions.SigningKey), SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(accessToken);
        }
    }
}