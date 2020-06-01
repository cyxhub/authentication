using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Basicauthentication.Controllers
{
    public class HomeController:Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signinManager;

        public HomeController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signinManager = signInManager;
        }
        public IActionResult index()
        {
            return View();
        }
        [Authorize]
        public IActionResult secret()
        {
            return View();
        }
        public IActionResult login()
        {
            
            return View();
        }
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(string um,string pss)
        {
            var user =await  _userManager.FindByNameAsync(um);
            if (user != null)
            {
                //登录
                var signinresult= await _signinManager.PasswordSignInAsync(user,pss,false,false);
                if (signinresult.Succeeded)
                {
                    Redirect("secret");
                }
            }
            return Redirect("index");
        }
        [HttpPost]
        public async Task<IActionResult> register(string um, string pss)
        {
            var user = new IdentityUser
            {
                UserName = um,
                Email=""
            };
            var result = await _userManager.CreateAsync(user,pss);
            Console.WriteLine(result.Succeeded);
            foreach(var i in result.Errors)
            {
                Console.WriteLine(i.Description);
            }
            
            if (result.Succeeded)
            {
                //sign here
                var signinresult = await _signinManager.PasswordSignInAsync(user, pss, false, false);
                if (signinresult.Succeeded)
                {
                   return Redirect("secret");
                }
            }
            
            return Redirect("index");
        }
        public async Task<IActionResult> logout()
        {
            await _signinManager.SignOutAsync();
            return Redirect("index");
        }
    }
}
