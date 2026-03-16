//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Extensions;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Users;
//using Microsoft.EntityFrameworkCore;

//namespace Oreon.Infrastructure.Persistence.Repositories;

//public sealed class UserRepository : IUserRepository
//{
//    private readonly DataContext _context;
//    private readonly IMapper _mapper;

//    public UserRepository(DataContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public void Update(AppUser user) => _context.Entry(user).State = EntityState.Modified;

//    public async Task<AppUser> GetByIdAsync(int id, CancellationToken ct = default)
//        => await _context.Users.FindAsync(new object[] { id }, ct);

//    public async Task<AppUser> GetByUsernameAsync(string username, CancellationToken ct = default)
//        => await _context.Users
//            .Include(u => u.Photos)
//            .SingleOrDefaultAsync(u => u.UserName == username, ct);

//    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams, CancellationToken ct = default)
//    {
//        var query = _context.Users.AsQueryable();

//        query = query.Where(u => u.UserName != userParams.CurrentUsername);
//        query = query.Where(u => u.Gender == userParams.Gender);

//        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
//        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

//        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

//        query = userParams.OrderBy switch
//        {
//            "created" => query.OrderByDescending(u => u.Created),
//            _ => query.OrderByDescending(u => u.LastActive)
//        };

//        var projectedQuery = query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
//        var count = await projectedQuery.CountAsync(ct);
//        var items = await projectedQuery
//            .Skip((userParams.PageNumber - 1) * userParams.PageSize)
//            .Take(userParams.PageSize)
//            .ToListAsync(ct);
//        return new PagedList<MemberDto>(items, count, userParams.PageNumber, userParams.PageSize);
//    }

//    public async Task<MemberDto> GetMemberByUsernameAsync(string username, CancellationToken ct = default)
//        => await _context.Users
//            .Where(u => u.UserName == username)
//            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
//            .AsNoTracking()
//            .FirstOrDefaultAsync(ct);

//    public async Task<string> GetGenderAsync(string username, CancellationToken ct = default)
//        => await _context.Users
//            .Where(u => u.UserName == username)
//            .Select(u => u.Gender)
//            .FirstOrDefaultAsync(ct);
//}
