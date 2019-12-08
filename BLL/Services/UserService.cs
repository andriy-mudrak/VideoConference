using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Constants;
using BLL.Helpers.Password;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Users.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<Claim>> Login(LoginInputModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var passwordStatus = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && passwordStatus)
            {
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                var userClaims = await this._userManager.GetClaimsAsync(user);
                return userClaims;
            }
            else
            {
                throw new AuthenticationException("User not authenticated");//TODO: change message 
            }
        }

        public async Task<IEnumerable<Claim>> Registration(RegisterRequestViewModel model)
        {
            var user = new AppUser { UserName = model.Email, Name = model.Name, Email = model.Email };
            await CreateUser(user, model.Password);

            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IEnumerable<Claim>> ExternalHandler(ClaimsPrincipal externalUser)
        {
            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {
                return await ExternalLogin(user);
            }
            else
            {
                return await ExternalRegistration(externalUser);
            }
        }

        private async Task<IEnumerable<Claim>> ExternalLogin(AppUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        private async Task<IEnumerable<Claim>> ExternalRegistration(ClaimsPrincipal externalUser)
        {
            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var user = new AppUser { UserName = email, Name = name, Email = email };
            var password = PasswordGenerating.GeneratePassword();

            await CreateUser(user, password);
            return await _userManager.GetClaimsAsync(user);
        }

        private async Task<AppUser> CreateUser(AppUser user, string password)
        {
            var registrationStatus = await _userManager.CreateAsync(user, password);

            if (!registrationStatus.Succeeded) throw new AuthenticationException(string.Join(" ", registrationStatus.Errors.Select(x => x.ToString())));

            await _userManager.AddToRoleAsync(user, RoleConstants.DEFAULT_ROLE);
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.USERNAME, user.UserName));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.NAME, user.Name));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.EMAIL, user.Email));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.ROLE, RoleConstants.DEFAULT_ROLE));

            return user;
        }
    }
}