using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tmp.Models;

namespace tmp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
        public IActionResult gettocken()
        {
            var defclim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"bob"),
                new Claim(ClaimTypes.Email,"bob@qq.com"),
                new Claim(ClaimTypes.Role,"admin")
            };
            var defidentity = new ClaimsIdentity(defclim, "def identity");
            
            var userprinciple = new ClaimsPrincipal(defidentity);Console.WriteLine(userprinciple);
            var result=HttpContext.SignInAsync(userprinciple);
            if (result.IsCompleted)
            {
                return Json("sucess");
            }
            return Json("fail");
        }
        [Authorize("default")]
        public IActionResult resource()
        {
            return View();
        }
    }
}
