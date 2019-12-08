using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer4.Services;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using BLL.Constants;
using BLL.Helpers.Password;
using BLL.Models;
using IdentityServer4;
using IdentityServer4.Stores;
using IdentityServerTest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AuthServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExternalController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public ExternalController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

            var externalUser = response.Principal;
            if (externalUser == null)
            {
                throw new Exception("External authentication error");
            }

            var name = externalUser.FindFirstValue(ClaimTypes.Name);
            var email = externalUser.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {

                var userClaims = await _userManager.GetClaimsAsync(user);
                await HttpContext.SignInAsync("Cookies", userClaims.ToArray());
            }
            else
            {
                var userRegistrationModel = new AppUser
                    { UserName = email, Name = name, Email = email };
                var password = PasswordGenerating.GeneratePassword();
                var registrationStatus = await _userManager.CreateAsync(userRegistrationModel, password);

                if (!registrationStatus.Succeeded) return BadRequest(registrationStatus.Errors);

                await _userManager.AddToRoleAsync(userRegistrationModel, RoleConstants.DEFAULT_ROLE);
                await _userManager.AddClaimAsync(userRegistrationModel,
                    new System.Security.Claims.Claim(ClaimConstants.USERNAME, userRegistrationModel.UserName));
                await _userManager.AddClaimAsync(userRegistrationModel,
                    new System.Security.Claims.Claim(ClaimConstants.NAME, userRegistrationModel.Name));
                await _userManager.AddClaimAsync(userRegistrationModel,
                    new System.Security.Claims.Claim(ClaimConstants.EMAIL, userRegistrationModel.Email));
                await _userManager.AddClaimAsync(userRegistrationModel,
                    new System.Security.Claims.Claim(ClaimConstants.ROLE, RoleConstants.DEFAULT_ROLE));

                await HttpContext.SignInAsync("Cookies",
                    _userManager.GetClaimsAsync(userRegistrationModel).Result.ToArray());
                
            }
            return Ok(new { user = user });

        }
    }
}