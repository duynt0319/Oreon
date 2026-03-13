//using Microsoft.EntityFrameworkCore;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Extensions;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Aggregates.Likes;
//using Oreon.Infrastructure.Identity;

//namespace Oreon.Infrastructure.Persistence.Repositories;

//public sealed class LikeRepository : ILikesRepository
//{
//    private readonly DataContext _context;

//    public LikeRepository(DataContext context) => _context = context;

//    public async Task<UserLike> GetUserLikeAsync(int sourceUserId, int targetUserId, CancellationToken ct = default)
//        => await _context.Likes.FindAsync(new object[] { sourceUserId, targetUserId }, ct);

//    public async Task<AppUser> GetUserWithLikesAsync(int userId, CancellationToken ct = default)
//        => await _context.Users
//            .Include(u => u.LikedUsers)
//            .FirstOrDefaultAsync(u => u.Id == userId, ct);

//    public async Task<PagedList<LikeDto>> GetUserLikesAsync(LikesParams likesParams, CancellationToken ct = default)
//    {
//        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
//        var likes = _context.Likes.AsQueryable();

//        if (likesParams.Predicate == "liked")
//        {
//            likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
//            users = likes.Select(l => l.TargetUser);
//        }

//        if (likesParams.Predicate == "likedBy")
//        {
//            likes = likes.Where(l => l.TargetUserId == likesParams.UserId);
//            users = likes.Select(l => l.SourceUser);
//        }

//        var likedUsers = users.Select(u => new LikeDto
//        {
//            Id = u.Id,
//            UserName = u.UserName,
//            KnownAs = u.KnownAs,
//            Age = u.DateOfBirth.CalculateAge(),
//            PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
//            City = u.City
//        });

//        var count = await likedUsers.CountAsync(ct);
//        var items = await likedUsers
//            .Skip((likesParams.PageNumber - 1) * likesParams.PageSize)
//            .Take(likesParams.PageSize)
//            .ToListAsync(ct);
//        return new PagedList<LikeDto>(items, count, likesParams.PageNumber, likesParams.PageSize);
//    }
//}
