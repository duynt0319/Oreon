//using Oreon.Application.Abstractions.Services;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Account.Commands.Login;

//public sealed record LoginCommand(LoginDto Dto) : IRequest<UserDto>;

//public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
//{
//    private readonly IIdentityService _identityService;
//    private readonly ITokenService _tokenService;

//    public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
//    {
//        _identityService = identityService;
//        _tokenService = tokenService;
//    }

//    public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _identityService.FindByNameWithPhotosAsync(request.Dto.Username.ToLower(), cancellationToken)
//            ?? throw new UnauthorizedAccessException("Invalid username.");

//        var valid = await _identityService.CheckPasswordAsync(user, request.Dto.Password);
//        if (!valid)
//            throw new UnauthorizedAccessException("Invalid password.");

//        var mainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);

//        return new UserDto
//        {
//            Username = user.UserName,
//            Token = await _tokenService.CreateTokenAsync(user),
//            PhotoUrl = mainPhoto?.Url,
//            KnownAs = user.KnownAs,
//            Gender = user.Gender
//        };
//    }
//}
