//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Users.Queries.GetMemberByUsername;

//public sealed record GetMemberByUsernameQuery(string Username) : IRequest<MemberDto>;

//public sealed class GetMemberByUsernameQueryHandler : IRequestHandler<GetMemberByUsernameQuery, MemberDto>
//{
//    private readonly IUnitOfWork _uow;

//    public GetMemberByUsernameQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<MemberDto> Handle(GetMemberByUsernameQuery request, CancellationToken cancellationToken)
//        => _uow.Users.GetMemberByUsernameAsync(request.Username, cancellationToken);
//}
