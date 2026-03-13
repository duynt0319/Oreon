//using Oreon.Application.Abstractions.Services;
//using Oreon.Domain.Users;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Oreon.Infrastructure.Services;

//public sealed class IdentityService : IIdentityService
//{
//    private readonly UserManager<AppUser> _userManager;

//    public IdentityService(UserManager<AppUser> userManager) => _userManager = userManager;

//    public async Task<bool> UserExistsAsync(string username, CancellationToken ct = default)
//        => await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower(), ct);

//    public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(AppUser user, string password)
//    {
//        var result = await _userManager.CreateAsync(user, password);
//        return (result.Succeeded, result.Errors.Select(e => e.Description));
//    }

//    public async Task<(bool Succeeded, IEnumerable<string> Errors)> AddToRoleAsync(AppUser user, string role)
//    {
//        var result = await _userManager.AddToRoleAsync(user, role);
//        return (result.Succeeded, result.Errors.Select(e => e.Description));
//    }

//    public async Task<AppUser> FindByNameWithPhotosAsync(string username, CancellationToken ct = default)
//        => await _userManager.Users
//            .Include(u => u.Photos)
//            .FirstOrDefaultAsync(u => u.UserName == username, ct);

//    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
//        => await _userManager.CheckPasswordAsync(user, password);

//    public async Task<IList<string>> GetRolesAsync(AppUser user)
//        => await _userManager.GetRolesAsync(user);
//}
