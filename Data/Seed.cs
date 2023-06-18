using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
                user.PasswordSalt = hmac.Key;
                foreach (var photo in user.Photos)
                {
                    photo.AssociatedEntity = "AppUser";
                    context.Photos.Add(photo);
                }
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
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

                var categoryIds = product.Categories.Select(c => c.Id).ToList();
                product.Categories = context.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .ToList();

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
