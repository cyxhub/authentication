using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basic.CustomPolicyProvider
{
    public class SecurityLevelAttribute:AuthorizeAttribute
    {
        public SecurityLevelAttribute(int level)
        {
            Policy = $"{Dynamicpolicy.securityLevel}.{level}";
        }
    }
    public static class Dynamicpolicy
    {
        public static IEnumerable<string> get()
            {
            yield return securityLevel;
            yield return rank;
            }
        public const string securityLevel = "securityLevel";
        public const string rank = "rank";
    }
    public  static class DynamicAuthorizationPlicyFactory
    {
        public static AuthorizationPolicy create(string policyname)
        {
            var parts = policyname.Split(".");
            var type = parts.First();
            var value = parts.Last();
            switch (type)
            {
                case Dynamicpolicy.rank:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim("rank", value).Build();
                case Dynamicpolicy.securityLevel:
                    Console.WriteLine(value);
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new SecurityLevelRequirement(Convert.ToInt32(value)))
                        .Build();
                default:
                    return null;
            }
        }
    }
    public class SecurityLevelRequirement : IAuthorizationRequirement
    {
        public SecurityLevelRequirement(int level)
        {
            Level = level;
        }
        public int Level { get;  }
    }
    public class SecuritylevelHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            SecurityLevelRequirement requirement)
        {
            var Claimvalue = Convert.ToInt32(context.User.Claims
                .FirstOrDefault(x => x.Type == Dynamicpolicy.securityLevel)?.Value??"0");
            if (requirement.Level <= Claimvalue)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    public class CustomAuthorizationPolicyProvider:DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options):base(options)
        {

        }
        public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach(var custompolicy in Dynamicpolicy.get())
            {
                if (policyName.StartsWith(custompolicy))
                {
                    //var policy = new AuthorizationPolicyBuilder().Build();
                    var policy = DynamicAuthorizationPlicyFactory.create(policyName);
                    return Task.FromResult(policy);
                }
                
            }
            return base.GetPolicyAsync(policyName);
        }
    }
}
