using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthenticatePro.Models;
using AuthPro.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using AuthPro.CustomAuthorize;

namespace AuthenticatePro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserStore _userStore;

        public HomeController(ILogger<HomeController> logger,UserStore userStore)
        {
            _logger = logger;
            _userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(Users model)
        {
            Users u = null;
            u = _userStore.users.FirstOrDefault(x=>x.name==model.name&&x.password==model.password);
            
            if(u==null) return View("login");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,u.name),
                new Claim(ClaimTypes.Name,u.name),
                new Claim(ClaimTypes.Email,u.email),
                new Claim(ClaimTypes.StreetAddress,u.address),
                new Claim(Operation.LimitLevel,u.level.ToString()),
                new Claim(Operation.card, u.type),
                new Claim(ClaimTypes.Role,"ideny"),

            };
            var claimsIdentity = new ClaimsIdentity(claims, "identify");
            /* var claims = new List<Claim> {
                  new Claim(JwtClaimTypes.Id,u.id.ToString()),
                  new Claim(JwtClaimTypes.Name,u.name),
                  new Claim(JwtClaimTypes.Email,u.email),
                  new Claim(JwtClaimTypes.Address,u.address),
              };
             var claimsIdentity = new ClaimsIdentity("identity card", JwtClaimTypes.Name, JwtClaimTypes.Role);
 */
            var claimsprinciple = new ClaimsPrincipal(claimsIdentity);
            var result=HttpContext.SignInAsync(claimsprinciple,new AuthenticationProperties
            {
                IsPersistent=true,
                 //ExpiresUtc=DateTime.Now.AddSeconds(3)
            });
            if (result.IsCompleted)
            {
                return Redirect("/users/userlist");
            }
            return View("index");
        }
        public IActionResult logout()
        {
            HttpContext.SignOutAsync();
            return View("index");
        }
        public IActionResult AccessDeny()
        {
            return View();
        }
    }
}
