//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Users;

//namespace Oreon.Application.Abstractions.Persistence;

//public interface IUserRepository
//{
//    void Update(AppUser user);
//    Task<AppUser> GetByIdAsync(int id, CancellationToken ct = default);
//    Task<AppUser> GetByUsernameAsync(string username, CancellationToken ct = default);
//    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams, CancellationToken ct = default);
//    Task<MemberDto> GetMemberByUsernameAsync(string username, CancellationToken ct = default);
//    Task<string> GetGenderAsync(string username, CancellationToken ct = default);
//}
