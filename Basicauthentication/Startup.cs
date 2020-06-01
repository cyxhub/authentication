using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basicauthentication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basicauthentication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppdataContext>(config=> {
                config.UseInMemoryDatabase("Memory");
            });
            //addidentity◊¢≤·∑˛ŒÒ √‹¬Î≈‰÷√
            services.AddIdentity<IdentityUser, IdentityRole>(config=> {
                config.Password.RequiredLength = 2;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppdataContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(config=> {
                config.Cookie.Name = "tmc";
                config.LoginPath = "/home/login";
            });
            services.AddControllersWithViews();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
