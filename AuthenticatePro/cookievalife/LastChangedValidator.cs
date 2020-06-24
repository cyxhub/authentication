using AuthPro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthPro.cookievalife
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<UserStore>();
            var userPrincipal = context.Principal;
            string lastChanged = (from c in userPrincipal.Claims where c.Type == "LastUpdated" select c.Value).FirstOrDefault();
            /*if (string.IsNullOrEmpty(lastChanged) || !userRepository.ValidateLastChanged(userPrincipal, lastChanged))
            {
                // 1. fail similar to Principal = principal;
                //context.RejectPrincipal();

                // 2. success，reproduce Cookie。
                context.ShouldRenew = true;
            }*/
        }
    }
}
