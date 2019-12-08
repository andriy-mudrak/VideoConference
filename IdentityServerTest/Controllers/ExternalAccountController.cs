using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;
using BLL.Constants;
using BLL.Helpers.Password;
using BLL.Services.Interfaces;
using DAL.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExternalAccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;

        public ExternalAccountController(SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userService = userService;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery]string provider)
        {
            if (provider == null) return BadRequest("Provider name can not be null");
            var loginProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
        
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback"),
                Items =
                {
                    { "scheme", provider }
                }
            };

            var authenticationProvider = 
                loginProviders.Where(x => x.Name.ToUpper() == provider.ToUpper()).LastOrDefault();
                
            if (authenticationProvider == null) BadRequest("Auth provider not found");

            return this.Challenge(authenticationProperties, authenticationProvider.Name);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var response = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (response?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            var externalUserClaims = response.Principal;
            if (externalUserClaims == null)
            {
                throw new Exception("External authentication error");
            }

            var claims = await _userService.ExternalHandler(externalUserClaims);
            await HttpContext.SignInAsync("Cookies", claims.ToArray());

            return Ok(claims);
        }
    }
}