using System.Linq;
using System.Threading.Tasks;
using BLL.Constants;
using BLL.Models;
using BLL.Services.Interfaces;
using IdentityServerTest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
                IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;

        }

        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            var test = HttpContext.User.Claims;
            var test2 = HttpContext.User.Identity.Name;
            var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            //User user = await _userManager.FindByIdAsync(HttpContext.);
            var roles = _signInManager.UserManager.GetRolesAsync(user).Result;
            var userRoles = _userManager.GetRolesAsync(user);
            var b1 = User.HasClaim("role", "Staff");
            var test3 = User.IsInRole("Staff");
            // получаем все роли
            // var allRoles = _roleManager.Roles.ToList();
            // получаем список ролей, которые были добавлены
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
            //   return Ok("Test");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin")]
        public IActionResult Test1()
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("User")]
        public IActionResult Test2()
        {
            //var test = HttpContext.User.Claims;
            //var test2 = HttpContext.User.Identity.Name;
            //var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            ////User user = await _userManager.FindByIdAsync(HttpContext.);
            //var roles = _signInManager.UserManager.GetRolesAsync(user).Result;
            //var userRoles = _userManager.GetRolesAsync(user);
            //// получаем все роли
            //var allRoles = _roleManager.Roles.ToList();
            // получаем список ролей, которые были добавлены
            return Ok("User");
        }

        [HttpGet]
        [Route("All")]
        //[Authorize(Roles = "Admin, Staff")]
        public IActionResult Test3()
        {
            return Ok("All");
        }
        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        ///
        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = await _userService.Login(model);
            await HttpContext.SignInAsync("Cookies", claims.ToArray());

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = await _userService.Register(model);
            await HttpContext.SignInAsync("Cookies", claims.ToArray());

            return Ok(model);
        }
    }
}
