namespace IdentitySample.Data
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class DbInitializer
    {
        public async static Task Initialize(IServiceProvider serviceProvider, string testPassword)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                var adminId = await EnsureUser(serviceProvider, "admin@test.com", testPassword);
                var userModId = await EnsureUser(serviceProvider, "usermod@test.com", testPassword);
                var userId = await EnsureUser(serviceProvider, "user@test.com", testPassword);

                await EnsureRole(serviceProvider, adminId, Constants.AdminRole);
                await EnsureRole(serviceProvider, userModId, Constants.UserModRole);
                await EnsureRole(serviceProvider, userId, Constants.UserRole);

                SeedDB(context);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userName, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(userName);
            if(user == null)
            {
                user = new ApplicationUser { UserName = userName };
                await userManager.CreateAsync(user, password);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(userId);
            return await userManager.AddToRoleAsync(user, role);
        }

        private static void SeedDB(ApplicationDbContext context)
        {
            if (context.Articles.Any())
            {
                return;
            }

            var articles = new Article[]
            {
                new Article{Title = "Article 1",ShortContent = "Short content 1", Content = "Content 1", CreateBy = "ndtung449@gmail.com", IsActivated = true},
                new Article{Title = "Article 2",ShortContent = "Short content 2", Content = "Content 2", CreateBy = "ndtung449@gmail.com", IsActivated = true},
                new Article{Title = "Article 3",ShortContent = "Short content 3", Content = "Content 3", CreateBy = "ndtung449@gmail.com", IsActivated = true},
                new Article{Title = "Article 4",ShortContent = "Short content 4", Content = "Content 4", CreateBy = "ndtung449@gmail.com", IsActivated = false}
            };

            foreach (var a in articles)
            {
                context.Articles.Add(a);
            }

            context.SaveChanges();
        }
    }
}
