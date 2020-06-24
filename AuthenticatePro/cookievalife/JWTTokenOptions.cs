using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthPro.cookievalife
{
    public class JWTTokenOptions
    {
        //
        public static string Issuer { get; set; } = "http://localhost:5001";
        //
        public static string Audience { get; set; } = "api";
        //encypt
        public static string SecurityKey { get; private set; } = "a secret that needs to be at least 16 characters long";
        //edit password，reproduce tockenkey
        public void SetSecurityKey(string value)
        {
            SecurityKey = value;
            CreateKey();
        }
        //symmetric
        public SymmetricSecurityKey Key { get; set; }
        //signatrue
        public SigningCredentials Credentials { get; set; }
        public JWTTokenOptions()
        {
            CreateKey();
        }
        private void CreateKey()
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
