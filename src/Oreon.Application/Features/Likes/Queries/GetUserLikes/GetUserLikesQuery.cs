//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Likes.Queries.GetUserLikes;

//public sealed record GetUserLikesQuery(LikesParams Params) : IRequest<PagedList<LikeDto>>;

//public sealed class GetUserLikesQueryHandler : IRequestHandler<GetUserLikesQuery, PagedList<LikeDto>>
//{
//    private readonly IUnitOfWork _uow;

//    public GetUserLikesQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<PagedList<LikeDto>> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
//        => _uow.Likes.GetUserLikesAsync(request.Params, cancellationToken);
//}
