//using Oreon.Application.Abstractions.Persistence;
//using MediatR;

//namespace Oreon.Application.Features.Users.Queries.GetMembers;

//public sealed record GetMemberGenderQuery(string Username) : IRequest<string>;

//public sealed class GetMemberGenderQueryHandler : IRequestHandler<GetMemberGenderQuery, string>
//{
//    private readonly IUnitOfWork _uow;

//    public GetMemberGenderQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<string> Handle(GetMemberGenderQuery request, CancellationToken cancellationToken)
//        => _uow.Users.GetGenderAsync(request.Username, cancellationToken);
//}
