using Microsoft.AspNetCore.Identity;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(PizzaContext pizzaContext, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        var username = configuration["Admin:Username"];
        var password = configuration["Admin:Password"];

        if (username is null || password is null) return;

        bool adminExists = await pizzaContext.Users.AnyAsync(eachUser => eachUser.Role == "Admin");

        if (adminExists) return;

        User adminUser = new User { Username = username, Role = "Admin" };

        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, password);

        pizzaContext.Users.Add(adminUser);

        await pizzaContext.SaveChangesAsync();
    }
}