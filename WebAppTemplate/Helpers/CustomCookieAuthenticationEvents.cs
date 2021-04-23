using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using System.Threading.Tasks;
using WebAppTemplate.Services;

namespace WebAppTemplate.Helpers
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IUserService _userService;

        public CustomCookieAuthenticationEvents(IUserService userService)
        {
            // Get the database from registered DI services.
            _userService = userService;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            // Look for the LastChanged claim.
            var email = userPrincipal.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            var role = userPrincipal.Claims.FirstOrDefault(c => c.Type == "Role").Value;

            if (!_userService.UserAuthorized(email, role))
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
