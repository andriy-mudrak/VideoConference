using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Services.Interfaces;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult StartPage()
        {
            return Ok("It is must be START PAGE");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Staff()
        {
            return Ok("Staff");
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = await _userService.Login(model);
            await HttpContext.SignInAsync("Cookies", claims.ToArray());

            return Ok(claims);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = await _userService.Registration(model);
            await HttpContext.SignInAsync("Cookies", claims.ToArray());

            return Ok(claims);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var myCookies = Request.Cookies.Keys;
             foreach (string cookie in myCookies)
            {
                Response.Cookies.Delete(cookie);
            }

            return Redirect("https://localhost:44364/Account/StartPage");
        }
    }
}
