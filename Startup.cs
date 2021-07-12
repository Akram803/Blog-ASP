using Blog.Data;
using Blog.Data.FileMangers;
using Blog.Data.Repositories;
using Blog.Models;
using Blog.Services.Email;
using Blog.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
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
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings")); 

            services.AddControllersWithViews(opt =>
                opt.CacheProfiles.Add("weekly", new CacheProfile { Duration = 60* 60* 24* 7 })
            ) ;

            services.AddDbContext<AppDbContext>(opt =>
                            opt.UseSqlServer(Configuration["ConnectionStrings:default"]));
            
            services.AddIdentity<AppUser, IdentityRole>(opt => {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(config => {
                config.LoginPath = "/auth/login";
                config.LogoutPath = "/auth/logout";
                config.AccessDeniedPath = "/auth/forbid";
            });

            services.AddScoped<PostRepository>();
            services.AddScoped<CommentRepository>();
            services.AddScoped<CategoryRepository>();

            services.AddTransient<IFileManager, FileManager>();

            services.AddSingleton<EmailService>();

            services.AddAutoMapper(typeof(MappingProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
                app.UseDeveloperExceptionPage();

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
