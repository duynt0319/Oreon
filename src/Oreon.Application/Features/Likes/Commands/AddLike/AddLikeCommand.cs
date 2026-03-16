//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Domain.Users;
//using MediatR;

//namespace Oreon.Application.Features.Likes.Commands.AddLike;

//public sealed record AddLikeCommand(int SourceUserId, string TargetUsername) : IRequest<Unit>;

//public sealed class AddLikeCommandHandler : IRequestHandler<AddLikeCommand, Unit>
//{
//    private readonly IUnitOfWork _uow;

//    public AddLikeCommandHandler(IUnitOfWork uow) => _uow = uow;

//    public async Task<Unit> Handle(AddLikeCommand request, CancellationToken cancellationToken)
//    {
//        var likedUser = await _uow.Users.GetByUsernameAsync(request.TargetUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.TargetUsername}' not found.");

//        var sourceUser = await _uow.Likes.GetUserWithLikesAsync(request.SourceUserId, cancellationToken);

//        if (sourceUser.UserName == request.TargetUsername)
//            throw new InvalidOperationException("You cannot like yourself.");

//        var existingLike = await _uow.Likes.GetUserLikeAsync(request.SourceUserId, likedUser.Id, cancellationToken);
//        if (existingLike != null)
//            throw new InvalidOperationException("You already like this user.");

//        var userLike = new UserLike(request.SourceUserId, likedUser.Id);
//        sourceUser.LikedUsers.Add(userLike);

//        await _uow.CompleteAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
