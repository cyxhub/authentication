using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AuthPro.CustomAuthorize
{
    public static class Operation
    {
        public const string LimitLevel = "LimitLevel";
        
    }
    public class LimitLevelAttribute : AuthorizeAttribute
    {
        public LimitLevelAttribute(int level)
        {
            Policy = $"{Operation.LimitLevel}.{level}";
        }
    }
    public class LimitValueRequirement : IAuthorizationRequirement
    {
        public int level { get; }
        public LimitValueRequirement(int lev)
        {
            level = lev;
        }
    }
    public class CustomAuthorizationHandler:
        AuthorizationHandler<LimitValueRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, LimitValueRequirement requirement)
        {
            var value = int.Parse(context.User.Claims.FirstOrDefault(x=>x.Type==Operation.LimitLevel)?.Value??"0");
            if (requirement.level < value)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
   
    public class CustomAuthorizationProvider:
        DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizationProvider(IOptions<AuthorizationOptions> options):base(options)
        {

        }
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(Operation.LimitLevel))
            {
                var names = policyName.Split(".");
                var type = names.First();
                var val = names.Last();
                var policy = new AuthorizationPolicyBuilder();
                if (type == Operation.LimitLevel)
                    return Task.FromResult(policy.AddRequirements(new LimitValueRequirement(int.Parse(val))).Build());
            }
            return base.GetPolicyAsync(policyName);
        }
    }
   

    //------------------------------------------
    public static class CardType
    {
        public const string subscribe = "subscribe";
        public const string author = "subscribe";
        public const string administrator = "administrator";
        public static List<string> cardlist = new List<string> { subscribe, author, administrator };
    }
}
