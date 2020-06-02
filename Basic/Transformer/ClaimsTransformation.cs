using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic.Transformer
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasfriendClaim = principal.Claims.Any(x => x.Type == "friend");
            if (!hasfriendClaim)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("friend", "bad"));
            }
            return Task.FromResult(principal);
        }
    }
}
