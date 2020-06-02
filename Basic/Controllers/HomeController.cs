using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basic.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    public class HomeController : Controller
    {
        private IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult secret()
        {
            return View();
        }
        [Authorize(Policy = "claim.dob")]
        public IActionResult secretpolicy()
        {
            return View();
        }
        [SecurityLevel(3)]
        public IActionResult secretlevel()
        {
            return View();
        }
        [SecurityLevel(10)]
        public IActionResult secretlevel2()
        {
            return View("secret");
        }
        [Authorize(Roles =  "admin")]
        public IActionResult secretrole()
        {
            return View();
        }
        public IActionResult authenticate()
        {
            var defclim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"bob"),
                new Claim(ClaimTypes.Email,"bob@qq.com"),
                new Claim("defs","dingyi"),
                new Claim(ClaimTypes.DateOfBirth,"2000-01-01"),
                new Claim(Dynamicpolicy.securityLevel,"7"),
                //
                new Claim(ClaimTypes.Role,"admin")
            };
            var lisenceclim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"bob"),
                
                new Claim("lisence","xu ke zheng"),
            };
            var defidentity = new ClaimsIdentity(defclim,"def identity");
            var lisenceidentity = new ClaimsIdentity(lisenceclim, "google lisence");
            IEnumerable<ClaimsIdentity> cms = new List<ClaimsIdentity>()
            {
                defidentity,
                lisenceidentity
            };
            var userprinciple = new ClaimsPrincipal(cms);
            HttpContext.SignInAsync(userprinciple);
            return Redirect("index");
        }
        public async Task<IActionResult> doStuff()
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("hello").Build();
            var authResult = await _authorizationService.AuthorizeAsync(User, customPolicy);
            if (authResult.Succeeded)
            {

            }
            return View("index");
        }
    }
}