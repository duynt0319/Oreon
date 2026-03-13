//using Oreon.Application.Abstractions.Services;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Users;
//using MediatR;

//namespace Oreon.Application.Features.Account.Commands.Register;

//public sealed record RegisterCommand(RegisterDto Dto) : IRequest<UserDto>;

//public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
//{
//    private readonly IIdentityService _identityService;
//    private readonly ITokenService _tokenService;

//    public RegisterCommandHandler(IIdentityService identityService, ITokenService tokenService)
//    {
//        _identityService = identityService;
//        _tokenService = tokenService;
//    }

//    public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
//    {
//        var dto = request.Dto;

//        if (await _identityService.UserExistsAsync(dto.Username, cancellationToken))
//            throw new InvalidOperationException("Username is already taken.");

//        var user = AppUser.Create(
//            dto.Username.ToLower(),
//            dto.DateOfBirth!.Value,
//            dto.KnownAs,
//            dto.Gender,
//            dto.City,
//            dto.Country);

//        var (created, errors) = await _identityService.CreateUserAsync(user, dto.Password);
//        if (!created)
//            throw new InvalidOperationException(string.Join(", ", errors));

//        var (addedRole, roleErrors) = await _identityService.AddToRoleAsync(user, "Member");
//        if (!addedRole)
//            throw new InvalidOperationException(string.Join(", ", roleErrors));

//        return new UserDto
//        {
//            Username = user.UserName,
//            Token = await _tokenService.CreateTokenAsync(user),
//            KnownAs = user.KnownAs,
//            Gender = user.Gender
//        };
//    }
//}
