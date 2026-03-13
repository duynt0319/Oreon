//using AutoMapper;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Abstractions.Services;
//using Oreon.Application.DTOs;
//using MediatR;
//using Microsoft.AspNetCore.Http;

//namespace Oreon.Application.Features.Users.Commands.AddPhoto;

//public sealed record AddPhotoCommand(string CurrentUsername, IFormFile File) : IRequest<PhotoDto>;

//public sealed class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, PhotoDto>
//{
//    private readonly IUnitOfWork _uow;
//    private readonly IPhotoService _photoService;
//    private readonly IMapper _mapper;

//    public AddPhotoCommandHandler(IUnitOfWork uow, IPhotoService photoService, IMapper mapper)
//    {
//        _uow = uow;
//        _photoService = photoService;
//        _mapper = mapper;
//    }

//    public async Task<PhotoDto> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _uow.Users.GetByUsernameAsync(request.CurrentUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.CurrentUsername}' not found.");

//        var result = await _photoService.AddPhotoAsync(request.File);
//        if (result.Error != null)
//            throw new InvalidOperationException(result.Error.Message);

//        var photo = user.AddPhoto(result.SecureUrl.AbsoluteUri, result.PublicId);
//        _uow.Users.Update(user);
//        await _uow.CompleteAsync(cancellationToken);

//        return _mapper.Map<PhotoDto>(photo);
//    }
//}
