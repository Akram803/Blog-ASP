using Blog.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // creatre admin user if not exist
            try
            {
                var scop = host.Services.CreateScope();

                var ctx = scop.ServiceProvider.GetRequiredService<AppDbContext>();
                var UserMgr = scop.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var RoleMgr = scop.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Ensure Database Created
                ctx.Database.EnsureCreated();

                // create admn role instance
                var adminRole = new IdentityRole("admin");

                // but admn role in BD if not exist
                if (!ctx.Roles.Any())
                {
                    RoleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!ctx.Users.Any(u => u.UserName == "admin"))
                {
                    // create user instance
                    var adminUser = new IdentityUser()
                    {
                        UserName = "admin",
                        Email = "admin@site.com"
                    };
                    // add created user to BD
                    var done = UserMgr.CreateAsync(adminUser, "admin").GetAwaiter().GetResult();

                    // add the role "admin" to the created user
                    UserMgr.AddToRoleAsync(adminUser, adminRole.Name);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
