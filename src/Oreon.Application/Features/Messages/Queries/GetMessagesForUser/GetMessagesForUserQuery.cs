//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Messages.Queries.GetMessagesForUser;

//public sealed record GetMessagesForUserQuery(MessageParams Params) : IRequest<PagedList<MessageDto>>;

//public sealed class GetMessagesForUserQueryHandler : IRequestHandler<GetMessagesForUserQuery, PagedList<MessageDto>>
//{
//    private readonly IUnitOfWork _uow;

//    public GetMessagesForUserQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<PagedList<MessageDto>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
//        => _uow.Messages.GetMessagesForUserAsync(request.Params, cancellationToken);
//}
