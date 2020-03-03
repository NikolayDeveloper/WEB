using System.Threading.Tasks;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Infrastructure.Abstract;
using Iserv.Niis.Infrastructure.Implementations;
using Iserv.Niis.Model.Models.Security;
using Iserv.Niis.Portal.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private const int MaxAccessFailedCount = 4;
        private readonly ICertificateService _certificateService;
        private readonly IJwtFactory _jwtFactory;
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AuthController> logger,
            IJwtFactory jwtFactory,
            ICertificateService certificateService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _jwtFactory = jwtFactory;
            _certificateService = certificateService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                throw new SecurityException("Incorrect credentials");

            if (!model.IsCertificate) return await PasswordSignIn(model);

            return await CertificateSignIn(model);
        }

        private async Task<IActionResult> CertificateSignIn(LoginModel model)
        {
            var certificateData = _certificateService.GetCertificateData(model.CertData, CertificateService.RsaAlgType);

            if (string.IsNullOrEmpty(certificateData.Iin))
                throw new SecurityException("Inn in certificate not found");

            var userToVerify = await _userManager.FindByNameAsync(certificateData.Iin);
            if (userToVerify == null)
                throw new SecurityException("User not found");

            var isValid = _certificateService.VerifyRsaCertificate(certificateData.Certificate, model.PlainData,
                model.SignedPlainData);
            if (!isValid)
                throw new SecurityException("Certificate is not valid");

            if (await _userManager.IsLockedOutAsync(userToVerify) ||
                userToVerify.AccessFailedCount == MaxAccessFailedCount)
            {
                await _userManager.SetLockoutEnabledAsync(userToVerify, true);
                throw new SecurityException("User is locked, contact your Administrator");
            }

            //var result = await _signInManager.PasswordSignInAsync(certificateData.Iin, model.Password, false, true);

            var isCanSignIn = await _signInManager.CanSignInAsync(userToVerify);
            if (isCanSignIn)
            {
                _logger.LogInformation(1, "User logged in.");
                var response = await _jwtFactory.Create(userToVerify);

                return Ok(response);
            }

            throw new SecurityException("Invalid login attempt.");
        }

        private async Task<IActionResult> PasswordSignIn(LoginModel model)
        {
            var userToVerify = await _userManager.FindByNameAsync(model.UserName);
            if (userToVerify == null)
                throw new SecurityException("User not found");

            if (await _userManager.IsLockedOutAsync(userToVerify) 
                || userToVerify.LockoutEnabled
                || userToVerify.AccessFailedCount == MaxAccessFailedCount)
            {
                await _userManager.SetLockoutEnabledAsync(userToVerify, true);
                throw new SecurityException("User is locked, contact your Administrator");
            }

            //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
            var result = await _signInManager.CheckPasswordSignInAsync(userToVerify, model.Password, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                var response = await _jwtFactory.Create(userToVerify);

                return Ok(response);
            }

            if (result.IsLockedOut)
                throw new SecurityException("User account locked out. Wait 5 minutes and try again");

            //await _userManager.AccessFailedAsync(userToVerify);
            throw new SecurityException("Invalid login attempt.");
        }
    }
}