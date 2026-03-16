//using Oreon.Application.Abstractions.Persistence;
//using MediatR;

//namespace Oreon.Application.Features.Users.Commands.SetMainPhoto;

//public sealed record SetMainPhotoCommand(string CurrentUsername, int PhotoId) : IRequest<Unit>;

//public sealed class SetMainPhotoCommandHandler : IRequestHandler<SetMainPhotoCommand, Unit>
//{
//    private readonly IUnitOfWork _uow;

//    public SetMainPhotoCommandHandler(IUnitOfWork uow) => _uow = uow;

//    public async Task<Unit> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _uow.Users.GetByUsernameAsync(request.CurrentUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.CurrentUsername}' not found.");

//        user.SetMainPhoto(request.PhotoId);
//        _uow.Users.Update(user);
//        await _uow.CompleteAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
