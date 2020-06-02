using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basic.AuthorizationRequirements
{
    public class CustomRequireClaim:IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
        public string ClaimType { get; }
    }
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            Console.WriteLine(hasClaim+"************"+requirement.ClaimType);
            foreach(var i in context.User.Claims)
            {
                Console.WriteLine(i.Type);
            }
            if (hasClaim)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder authenticationBuilder,
            string claimType)
        {
            Console.WriteLine(claimType+"*********  static");
            authenticationBuilder.AddRequirements(new CustomRequireClaim(claimType));
            return authenticationBuilder;
        }
    }
}
