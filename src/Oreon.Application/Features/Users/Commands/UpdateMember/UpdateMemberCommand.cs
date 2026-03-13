//using AutoMapper;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Users.Commands.UpdateMember;

//public sealed record UpdateMemberCommand(string CurrentUsername, MemberUpdateDto Dto) : IRequest<Unit>;

//public sealed class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Unit>
//{
//    private readonly IUnitOfWork _uow;
//    private readonly IMapper _mapper;

//    public UpdateMemberCommandHandler(IUnitOfWork uow, IMapper mapper)
//    {
//        _uow = uow;
//        _mapper = mapper;
//    }

//    public async Task<Unit> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _uow.Users.GetByUsernameAsync(request.CurrentUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.CurrentUsername}' not found.");

//        user.UpdateProfile(
//            request.Dto.Introduction,
//            request.Dto.LookingFor,
//            request.Dto.Interests,
//            request.Dto.City,
//            request.Dto.Country);

//        _uow.Users.Update(user);
//        await _uow.CompleteAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
