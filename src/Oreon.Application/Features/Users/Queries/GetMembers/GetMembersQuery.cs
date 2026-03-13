//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Users.Queries.GetMembers;

//public sealed record GetMembersQuery(UserParams Params) : IRequest<PagedList<MemberDto>>;

//public sealed class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, PagedList<MemberDto>>
//{
//    private readonly IUnitOfWork _uow;

//    public GetMembersQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<PagedList<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
//        => _uow.Users.GetMembersAsync(request.Params, cancellationToken);
//}
