using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Users;

namespace TaskManager.Infrastructure.Persistence.Seeder;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher hasher, string adminUsername, string adminPassword)
    {
        if (await context.Users.AnyAsync()) return;

        var admin = User.Create(adminUsername, hasher.Hash(adminPassword));
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
