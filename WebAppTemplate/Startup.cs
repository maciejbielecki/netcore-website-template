using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAppTemplate.Data;
using WebAppTemplate.Helpers;
using WebAppTemplate.Services;

namespace WebAppTemplate
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/Login";
                });
            //services.AddAuthentication()
            //    .AddCookie()
            //    .AddMicrosoftAccount(microsoftOptions =>
            //    {
            //        microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
            //        microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            //        microsoftOptions.AccessDeniedPath = "/User/AccessDeniedPathInfo";
            //    })
            //    .AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //        googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //        googleOptions.AccessDeniedPath = "/User/AccessDeniedPathInfo";
            //    })
            //    .AddTwitter(twitterOptions =>
            //    {
            //        twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerAPIKey"];
            //        twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            //        twitterOptions.AccessDeniedPath = "/User/AccessDeniedPathInfo";
            //        twitterOptions.RetrieveUserDetails = true;
            //    })
            //    .AddFacebook(facebookOptions =>
            //    {
            //        facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //        facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //        facebookOptions.AccessDeniedPath = "/User/AccessDeniedPathInfo";
            //    });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=Database.db"));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<CustomCookieAuthenticationEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
