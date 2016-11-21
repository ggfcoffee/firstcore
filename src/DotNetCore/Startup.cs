using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IDAL;
using DAL;
using IBLL;
using BLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Common;


namespace DotNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
           
            services.AddMvc();
            services.AddSession();
            services.AddMemoryCache();
            services.AddAuthorization();
            
            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddSession();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            
            
            //services.AddSession(option => { option. })
            
            var connection = @"Data Source=.\SQLServer2008R2;Initial Catalog=DotNetCoreTest;User ID=sa;Password=sls_taobao;";

            

            services.AddDbContext<MyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession(new SessionOptions() { CookieName = "MySession", IdleTimeout = TimeSpan.FromMinutes(2) });
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                LoginPath = new PathString("/Home/Index/"),
                AccessDeniedPath = new PathString("/Home/About/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieName = "MyCookie",
                ExpireTimeSpan = TimeSpan.FromHours(2)
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
           
            
        }
    }
}
