using System;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL;
using DAL.Models;
using DAL.Users;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("Default")));
            
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            services.AddTransient<IUserService, UserService>();

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                    googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:ClientId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:ClientSecret"];
                    facebookOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                })
                .AddLinkedIn(linkedinOptions =>
                {
                    linkedinOptions.ClientId = Configuration["Authentication:Linkedin:ClientId"];
                    linkedinOptions.ClientSecret = Configuration["Authentication:Linkedin:ClientSecret"];
                    linkedinOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(Configuration.GetConnectionString("Default"));
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<AppUser>()
                .AddProfileService<IdentityClaimsProfileService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
