using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthPro.Controllers
{
    public class ResourceController : Controller
    {
        private IAuthorizationService _authorizationService;
        private IWebHostEnvironment _hostEnvironment;

        public ResourceController(IAuthorizationService authorizationService, IWebHostEnvironment hostEnvironment)
        {
            _authorizationService = authorizationService;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> getreource()
        {
            string filename = Path.Combine(_hostEnvironment.WebRootPath, "resource.txt");
            string text = System.IO.File.ReadAllText(filename);

            var opt = new OperationAuthorizationRequirement
            {
                Name = ResourceOpt.insert
            };
            var result=await _authorizationService.AuthorizeAsync(User,text,opt);
            if (result.Succeeded)
            {
                return Json(text);
            }
            return Json("you are forbidden");
        }
        public static class ResourceOpt
        {
            public static string insert = "insert";
            public static string del = "del";
            public static string edit = "edit";
            public static string query = "query";
        }

        public class ResourceAuthorizeHandler:
            AuthorizationHandler<OperationAuthorizationRequirement>
        {
            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
            {
                var val = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                if (requirement.Name == ResourceOpt.del)
                {
                    if (val == "ideny")
                    {
                        context.Succeed(requirement);
                    }
                }else if (requirement.Name == ResourceOpt.insert)
                {
                    if (val == "ideny2")
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
        }
    }
}
