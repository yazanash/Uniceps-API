using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.HostBuilder
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                string[] roleNames = { "Admin", "User", "Tester" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var adminEmails = configuration.GetSection("InitialSetup:Admins").Get<List<string>>();

                if (adminEmails != null && adminEmails.Any())
                {
                    foreach (var email in adminEmails)
                    {
                        var user = await userManager.FindByEmailAsync(email);

                        if (user == null)
                        {
                            var newAdmin = new AppUser
                            {
                                UserName = email.Split('@')[0],
                                Email = email,
                                EmailConfirmed = true,
                                UserType = UserType.Normal, 
                                CreatedAt = DateTime.UtcNow
                            };

                            var result = await userManager.CreateAsync(newAdmin);

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(newAdmin, "Admin");
                            }
                        }
                        else
                        {
                            if (!await userManager.IsInRoleAsync(user, "Admin"))
                            {
                                await userManager.AddToRoleAsync(user, "Admin");
                            }
                        }
                    }
                }

                var productsToUpdate = await context.Set<Product>().ToListAsync();

                if (productsToUpdate.Any())
                {
                    foreach (var product in productsToUpdate)
                    {
                        product.GenerateSlug();
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }
    }
}
