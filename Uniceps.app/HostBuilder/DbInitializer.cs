using Microsoft.AspNetCore.Identity;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.HostBuilder
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();

            // جلب الخدمات اللازمة من الـ Scope
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            try
            {
                // 1. تعريف الأدوار الأساسية في النظام
                string[] roleNames = { "Admin", "User", "Tester" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // 2. جلب قائمة الإيميلات التي ستكون Admin من appsettings.json
                // نتوقع في الملف قائمة: "InitialSetup": { "Admins": ["email1", "email2"] }
                var adminEmails = configuration.GetSection("InitialSetup:Admins").Get<List<string>>();

                if (adminEmails != null && adminEmails.Any())
                {
                    foreach (var email in adminEmails)
                    {
                        var user = await userManager.FindByEmailAsync(email);

                        if (user == null)
                        {
                            // إنشاء المستخدم إذا لم يكن موجوداً (بدون كلمة مرور لأنك تستخدم OTP)
                            var newAdmin = new AppUser
                            {
                                UserName = email.Split('@')[0],
                                Email = email,
                                EmailConfirmed = true,
                                UserType = UserType.Normal, // عدلها حسب نوع المستخدم عندك
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
                            // إذا كان المستخدم موجوداً أصلاً، نتأكد فقط أنه يملك دور Admin
                            if (!await userManager.IsInRoleAsync(user, "Admin"))
                            {
                                await userManager.AddToRoleAsync(user, "Admin");
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
