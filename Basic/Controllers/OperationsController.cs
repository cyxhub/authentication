using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basic.Controllers
{
    public class OperationsController:Controller
    {
        private IAuthorizationService _authorizationService;

        public OperationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        /*public async Task<IActionResult> open()
        {
            var requirement = new OperationAuthorizationRequirement()
            {
                Name = CookieJarOperations.Open
            };
            var cookiej = new CookieJar();//从数据库获取cookiejar
            await _authorizationService.AuthorizeAsync(User, null, requirement);
            return View();
        }*/
        public async Task<IActionResult> open()
        {
            var cookieJar = new CookieJar();
            await _authorizationService.AuthorizeAsync(User, cookieJar, CookieAuthOperation.open);
            return View();
        }
    }
    public static class CookieAuthOperation
    {
        public static OperationAuthorizationRequirement open = new OperationAuthorizationRequirement()
        {
            Name=CookieJarOperations.Open
        };
    }
    public  class CookieJarAuthorizationHandler:
        AuthorizationHandler<OperationAuthorizationRequirement,CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement,
            CookieJar cookieJar)
        {
            

            if (requirement.Name == CookieJarOperations.look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == CookieJarOperations.comenear)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
    public static class CookieJarOperations
    {
        public static string Open = "open";
        public static string takecookie = "takecookie";
        public static string comenear = "comenear";
        public static string look = "look";
    }
    public class CookieJar
    {
        public string name { get; set; }
    }
}
