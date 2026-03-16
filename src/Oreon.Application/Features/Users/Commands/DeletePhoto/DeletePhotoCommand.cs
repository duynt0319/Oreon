//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Abstractions.Services;
//using MediatR;

//namespace Oreon.Application.Features.Users.Commands.DeletePhoto;

//public sealed record DeletePhotoCommand(string CurrentUsername, int PhotoId) : IRequest<Unit>;

//public sealed class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Unit>
//{
//    private readonly IUnitOfWork _uow;
//    private readonly IPhotoService _photoService;

//    public DeletePhotoCommandHandler(IUnitOfWork uow, IPhotoService photoService)
//    {
//        _uow = uow;
//        _photoService = photoService;
//    }

//    public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _uow.Users.GetByUsernameAsync(request.CurrentUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.CurrentUsername}' not found.");

//        var photo = user.Photos.FirstOrDefault(p => p.Id == request.PhotoId)
//            ?? throw new KeyNotFoundException($"Photo {request.PhotoId} not found.");

//        if (photo.PublicId != null)
//        {
//            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
//            if (result.Error != null)
//                throw new InvalidOperationException(result.Error.Message);
//        }

//        user.DeletePhoto(request.PhotoId);
//        _uow.Users.Update(user);
//        await _uow.CompleteAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
