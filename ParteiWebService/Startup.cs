using ParteiWebService.Views.Manager;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParteiWebService.Services;
using System.IO;

namespace ParteiWebService
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

            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredLength = 6;       
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;

            })
            .AddEntityFrameworkStores<ParteiDbContext>()
            .AddDefaultTokenProviders();

            //services.AddAuthorization(config =>
            //{
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    var defaultAuthPolicy = defaultAuthBuilder
            //    .RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.DateOfBirth)
            //    .Build();

            //    config.DefaultPolicy = defaultAuthPolicy;
            //});

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Account/Login";
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<ParteiDbContext>(options => options.UseSqlite("Data Source=cloudbobdb.db"));

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<Authmessagesenderoptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    await ErrorPageCreator.CreateAsync(context);
                });
            });
            */

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
