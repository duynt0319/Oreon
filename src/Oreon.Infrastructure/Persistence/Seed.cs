using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oreon.Infrastructure.Identity;
using System.Text.Json;

namespace Oreon.Infrastructure.Persistence
{
    public class Seed
    {
        public static async Task ClearConnection(DataContext context)
        {
            context.Connections.RemoveRange(context.Connections);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var seedUsers = JsonSerializer.Deserialize<List<SeedUserDto>>(userData, options);

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Member" },
                new AppRole { Name = "Admin" },
                new AppRole { Name = "Moderator" }
            };

            foreach (var role in roles)
                await roleManager.CreateAsync(role);

            // TODO: Refactor to create Member aggregate and sync with ApplicationUser
            foreach (var dto in seedUsers)
            {
                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = dto.UserName.ToLower(),
                    Email = $"{dto.UserName.ToLower()}@example.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                // TODO: Create corresponding Member aggregate with photos
            }

            var admin = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@example.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }

        private sealed record SeedUserDto(
            string UserName,
            string Gender,
            string DateOfBirth,
            string KnownAs,
            string Introduction,
            string LookingFor,
            string Interests,
            string City,
            string Country,
            List<SeedPhotoDto> Photos);

        private sealed record SeedPhotoDto(string Url, bool IsMain);
    }
}
