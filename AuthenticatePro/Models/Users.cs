using AuthPro.CustomAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthPro.Models
{
    public class Users
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string type { get; set; }
        public int level { get; set; }
    }
    public class UserStore
    {
        public IEnumerable<Users> users = new List<Users> { 
            new Users
            {
                id=1,name="jerry",email="rt@rt.com",address="def",password="jerry",type=CardType.author,level=4
            },
            new Users
            {
                id=2,name="nino",email="er@rert.com",address="def",password="nino",type=CardType.subscribe,level=3
            },
            new Users
            {
                id=3,name="er",email="et@et.com",address="def",password="er",type=CardType.administrator,level=9
            },
             new Users
            {
                id=4,name="ef",email="ef@et.com",address="def",password="ef",type=CardType.author,level=6
            },
        };
    }
}
