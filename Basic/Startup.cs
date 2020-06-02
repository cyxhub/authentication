using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basic.AuthorizationRequirements;
using Basic.Controllers;
using Basic.CustomPolicyProvider;
using Basic.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basic
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication("cookieauth")
                .AddCookie("cookieauth", config => {
                    config.Cookie.Name = "cookienm";
                    config.LoginPath = "/home/authenticate";
                });
            services.AddAuthorization(
                config => {
                    /*var defaultauthBuilder = new AuthorizationPolicyBuilder();
                    var defaultauthPolicy = defaultauthBuilder
                                            .RequireAuthenticatedUser()
                                            .RequireClaim(ClaimTypes.DateOfBirth)
                                            .Build();*/
                    config.AddPolicy("claim.dob", policybuilder =>
                    {
                        policybuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                    });
                    /*config.AddPolicy("claimdob", policybuilder =>
                    {
                        policybuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                    });*/

                });
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecuritylevelHandler>();
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
            services.AddControllersWithViews(
                config=> {
                    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                    var defaultAuthPolicy = defaultAuthBuilder.RequireAuthenticatedUser().Build();
                    //È«¾Ö¹ýÂËÆ÷
                    //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
                });
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
