using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppTemplate.Data;
using WebAppTemplate.Models.Request;
using WebAppTemplate.Models.Response;
using WebAppTemplate.Services;

namespace WebAppTemplate.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] Register request, string returnurl)
        {
            var usr = _userService.Login(request);
            if (usr == null)
            {
                return View(new AuthorizeResponse() { Errors = new string[] { "Niepoprawny login lub hasło" } });
            }

            AddCookieAuth(usr);

            if (string.IsNullOrEmpty(returnurl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var tab = returnurl.Split('/');
                return RedirectToAction(tab[2], tab[1]);
            }
        }


        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] Register request, string returnurl)
        {
            var usr = _userService.Register(request);
            if (usr == null)
            {
                return View(new AuthorizeResponse() { Errors = new string[] { "Nie udało się utworzyć nowego konta" } });
            }

            AddCookieAuth(usr);


            if (string.IsNullOrEmpty(returnurl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var tab = returnurl.Split('/');
                return RedirectToAction(tab[2], tab[1]);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> AccessDeniedPathInfo()
        {
            return View();
        }

        private async Task AddCookieAuth(User user)
        {
            var claims = new List<Claim>
                            {
                                new Claim("Email", user.Email),
                                new Claim("Role", user.Role.Name),
                                new Claim("UserName", $"{user.UserData.FirstName} {user.UserData.LastName}")
                            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                                            CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(claimsIdentity),
                                            new AuthenticationProperties
                                            {
                                                IsPersistent = true,
                                                //ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                                            });
        }
    }
}
