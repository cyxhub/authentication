using AuthPro.cookievalife;
using AuthPro.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthPro.Controllers
{
    
    public class JwtController:Controller
    {
        private UserStore _userStore;

        public JwtController(UserStore userStore)
        {
            _userStore = userStore;
        }
        
        [HttpGet]
        public IActionResult GetToken(Users model)
        {
            Console.WriteLine(model.name);
            Users u = null;
            foreach (var user in _userStore.users)
            {
                if (model.name == user.name && model.password == user.password)
                {
                    u = new Users
                    {
                        id = user.id,
                        name = user.name,
                        email = user.email,
                        address = user.address,
                        password = user.password
                    };
                }

            }
            if (u == null) return Json("the user is not exist");

            //identify
            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Id, u.id.ToString()),
                new Claim(JwtClaimTypes.Name, u.name),
                new Claim(JwtClaimTypes.Email, u.email),
                new Claim(JwtClaimTypes.Address, u.address),
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: JWTTokenOptions.Issuer,
                audience: JWTTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new JWTTokenOptions().Credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Json(jwtToken);
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult jwtuserList()
        {
            UserStore userStore = new UserStore();

            return Json(userStore.users);
        }
    }
}
