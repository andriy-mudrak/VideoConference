using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Constants;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class UserService: IUserService
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

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                var userClaims = await this._userManager.GetClaimsAsync(user);
                return userClaims;
            }
            else
            {
                throw new AuthenticationException("Bad password");//TODO: change message 
            }
        }

        public async Task<IEnumerable<Claim>> Register(RegisterRequestViewModel model)
        {
            var user = new AppUser { UserName = model.Email, Name = model.Name, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new AuthenticationException(string.Join(" ", result.Errors.Select(x=>x.ToString())));

            await _userManager.AddToRoleAsync(user, model.Role);
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.USERNAME, user.UserName));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.NAME, user.Name));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.EMAIL, user.Email));
            await _userManager.AddClaimAsync(user,
                new System.Security.Claims.Claim(ClaimConstants.ROLE, RoleConstants.DEFAULT_ROLE));

            return await _userManager.GetClaimsAsync(user);
        }
    }
}