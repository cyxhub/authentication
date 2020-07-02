using AuthPro.CustomAuthorize;
using AuthPro.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthPro.Controllers
{
    public class UsersController:Controller
    {
        [Authorize]
        public IActionResult userList()
        {
            UserStore userStore = new UserStore();
            
            return View(userStore.users);
        }
        [Authorize(Roles ="ideny")]
        public IActionResult userself()
        {
            var uname = HttpContext.User.FindFirst(x=>x.Type==System.Security.Claims.ClaimTypes.Name).Value;
            var role = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            UserStore userStore = new UserStore();
            var u = userStore.users.FirstOrDefault(x=>x.name==uname);
                if (u !=null)
                {
                    return View(u);
                }
            
            return Json("get null");
        }
        [Authorize(Roles = "ideny2")]
        public IActionResult userself2()
        {
            var uname = HttpContext.User.FindFirst(x => x.Type == System.Security.Claims.ClaimTypes.Name).Value;
            UserStore userStore = new UserStore();
            foreach (var u in userStore.users)
            {
                if (uname == u.name)
                {
                    return View(u);
                }
            }
            return Json("get null");
        }

        [LimitLevel(6)]
        public IActionResult admin()
        {
            UserStore userStore = new UserStore();
            return View(userStore.users);
        }
        [LimitLevel(3)]
        public IActionResult author()
        {
            UserStore userStore = new UserStore();
            return View(userStore.users);
        }

        /*[LimitLevel(4)]
        public IActionResult subscribe()
        {
            UserStore userStore = new UserStore();
            return View(userStore.users);
        }*/
    }
}
