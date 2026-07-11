using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Users;

namespace TaskManager.Infrastructure.Persistence.Seeder;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher hasher)
    {
        if (await context.Users.AnyAsync()) return;

        var admin = User.Create("admin", hasher.Hash("admin123"));
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
