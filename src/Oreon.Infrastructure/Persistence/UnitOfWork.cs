//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Infrastructure.Persistence.Repositories;
//using AutoMapper;

//namespace Oreon.Infrastructure.Persistence;

//public sealed class UnitOfWork : IUnitOfWork
//{
//    private readonly DataContext _context;
//    private readonly IMapper _mapper;

//    private IUserRepository _users;
//    private IMessageRepository _messages;
//    private ILikesRepository _likes;

//    public UnitOfWork(DataContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public IUserRepository Users => _users ??= new UserRepository(_context, _mapper);
//    public IMessageRepository Messages => _messages ??= new MessageRepository(_context, _mapper);
//    public ILikesRepository Likes => _likes ??= new LikeRepository(_context);

//    public async Task<bool> CompleteAsync(CancellationToken ct = default)
//        => await _context.SaveChangesAsync(ct) > 0;

//    public bool HasChanges() => _context.ChangeTracker.HasChanges();
//}
