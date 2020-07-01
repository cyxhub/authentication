using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AuthPro.cookievalife;
using AuthPro.CustomAuthorize;
using AuthPro.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using static AuthPro.Controllers.ResourceController;

namespace AuthenticatePro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddAuthentication("auth")
                .AddCookie("auth", config =>
                 {
                     config.Cookie.Name = "links";
                     config.LoginPath = "/home/login";
                     config.AccessDeniedPath = "/home/AccessDeny";
                     /*config.Events = new CookieAuthenticationEvents
                     {
                         OnValidatePrincipal = LastChangedValidator.ValidateAsync
                     };*/
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options=>
                {
                    options.Events= new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Query["token"];
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed=context=>
                        {
                            context.Response.WriteAsync("authenticate fail");
                            return Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new JWTTokenOptions().Key,
                        ValidateIssuer = true,
                        ValidIssuer = JWTTokenOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = JWTTokenOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(300)
                    };
                });
            
            services.AddAuthorization(options=>
            {
                options.AddPolicy("normal",policy=>
                {
                    policy.RequireAuthenticatedUser().Build();
                    //policy.RequireAuthenticatedUser().RequireClaim("","").Build();
                });
            });
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizeHandler>();
            services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationProvider>();
            services.AddMvc();
            services.AddScoped<UserStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
