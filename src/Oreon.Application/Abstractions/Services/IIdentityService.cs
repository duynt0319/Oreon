//using Oreon.Domain.Users;

//namespace Oreon.Application.Abstractions.Services;

//public interface IIdentityService
//{
//    Task<bool> UserExistsAsync(string username, CancellationToken ct = default);
//    Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(AppUser user, string password);
//    Task<(bool Succeeded, IEnumerable<string> Errors)> AddToRoleAsync(AppUser user, string role);
//    Task<AppUser> FindByNameWithPhotosAsync(string username, CancellationToken ct = default);
//    Task<bool> CheckPasswordAsync(AppUser user, string password);
//    Task<IList<string>> GetRolesAsync(AppUser user);
//}
