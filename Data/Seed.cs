using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);


            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {

                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "123456");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });


        }

        public static async Task SeedProducts(DataContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var productData = await File.ReadAllTextAsync("Data/ProductSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var products = JsonSerializer.Deserialize<List<Product>>(productData, options);

            foreach (var product in products)
            {
                foreach (var photo in product.Photos)
                {
                    photo.AssociatedEntity = "Product";
                    photo.ProductId = product.Id;
                    context.Photos.Add(photo);
                }

                var categoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
                product.ProductCategories = categoryIds.Select(categoryId => new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                }).ToList();

                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
        }


        public static async Task SeedCategories(DataContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }

            var categoryData = File.ReadAllText("Data/CategorySeedData.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }




    }
}
