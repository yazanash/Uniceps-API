using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Uniceps.app.Services.NutrationSeeder;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.NutritionSystem;
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
        public static async Task SeedInitialNutritionDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (await context.IngredientCategories.AnyAsync())
                return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ingredients.json");
            if (!File.Exists(filePath))
                return;

            var jsonContent = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var categoryDtos = JsonSerializer.Deserialize<List<CategorySeedDto>>(jsonContent, options);

            if (categoryDtos == null || !categoryDtos.Any())
                return;

            foreach (var categoryDto in categoryDtos)
            {
                var category = new IngredientCategory
                {
                    EnglishName = categoryDto.EnglishName,
                    ArabicName = categoryDto.ArabicName
                };

                await context.IngredientCategories.AddAsync(category);
                await context.SaveChangesAsync();

                var ingredients = categoryDto.Ingredients.Select(iDto => new Ingredient
                {
                    Id = Guid.NewGuid(),
                    EnglishName = iDto.EnglishName,
                    ArabicName = iDto.ArabicName,
                    CategoryId = category.Id, 
                    DefaultServingInGrams = iDto.DefaultServingInGrams,
                    Calories = iDto.Calories,
                    Protein = iDto.Protein,
                    Carbs = iDto.Carbs,
                    Fats = iDto.Fats,
                    IsVerified = true,
                    IsUserGenerated = false,
                    UserId = null,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                await context.Ingredients.AddRangeAsync(ingredients);
            }

            await context.SaveChangesAsync();
        }
    }
}
