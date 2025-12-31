using ECommerce.Domain.Models.Products;
using ECommerce.Persistence.Contexts;
using ECommerce.Domain.Models.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Models.Orders;

namespace ECommerce.Domain.Contracts.Seeding
{
    public class DataSeeding(StoreDbContext context,StoreIdentityDbContext identityContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IDataSeeding
    {
      
        public async Task DataSeedAsync()
        {
            var PendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
                context.Database.Migrate();

            if(!context.Brands.Any())
            {
                var BrandData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\brands.json");
                var ProductBrands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (ProductBrands.Any() && ProductBrands is not null)
                {
                    context.Brands.AddRange(ProductBrands);
                }
            }

            if (!context.Types.Any())
            {
                var TypeData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\types.json");
                var ProductTypes = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (ProductTypes.Any() && ProductTypes is not null)
                {
                    context.Types.AddRange(ProductTypes);
                }
            }
            context.SaveChanges();
            if (!context.Products.Any())
            {
                var ProductData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (products.Any() && products is not null)
                {
                    context.Products.AddRange(products);
                }
            }
            
            if (!context.Set<DeliveryMethod>().Any())
            {
                var MethodsData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);
                if (Methods.Any() && Methods is not null)
                {
                    context.Set<DeliveryMethod>().AddRange(Methods);
                }
            }
            context.SaveChanges();
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await roleManager.CreateAsync(new IdentityRole("NormalUser"));
                }
                if (!userManager.Users.Any())
                {
                    var user1 = new ApplicationUser()
                    {
                        Email = "mohamedoffical@gmail.com",
                        DisplayName = " Mohamed Ahmed",
                        PhoneNumber = "01022095771",
                        UserName = "mohamed"
                    };
                    var user2 = new ApplicationUser()
                    {
                        Email = "Aly_Ahmed@gmail.com",
                        DisplayName = "Aly Ahmed",
                        PhoneNumber = "01522095771",
                        UserName = "AlyAhmed"
                    };
                    var user3 = new ApplicationUser()
                    {
                        Email = "Alaa@gmail.com",
                        DisplayName = "Alaa Mohamed",
                        PhoneNumber = "01122095771",
                        UserName = "Alaa"
                    };

                    await userManager.CreateAsync(user1, "P@ssw0rd");
                    await userManager.CreateAsync(user2, "P@ssw0rd");
                    await userManager.CreateAsync(user3, "P@ssw0rd");

                    await userManager.AddToRoleAsync(user1, "Admin");
                    await userManager.AddToRoleAsync(user2, "superAdmin");
                    await userManager.AddToRoleAsync(user3, "NormalUser");

                    await identityContext.SaveChangesAsync();

                }
                await identityContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
