using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basicauthentication.Data
{
    //包含所有用户表
    public class AppdataContext:IdentityDbContext
    {
        public AppdataContext(DbContextOptions<AppdataContext> options):base(options)
        {

        }
    }
}
