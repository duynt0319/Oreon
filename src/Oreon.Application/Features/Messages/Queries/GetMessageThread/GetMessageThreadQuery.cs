//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.DTOs;
//using MediatR;

//namespace Oreon.Application.Features.Messages.Queries.GetMessageThread;

//public sealed record GetMessageThreadQuery(string CurrentUsername, string RecipientUsername) : IRequest<IEnumerable<MessageDto>>;

//public sealed class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, IEnumerable<MessageDto>>
//{
//    private readonly IUnitOfWork _uow;

//    public GetMessageThreadQueryHandler(IUnitOfWork uow) => _uow = uow;

//    public Task<IEnumerable<MessageDto>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
//        => _uow.Messages.GetMessageThreadAsync(request.CurrentUsername, request.RecipientUsername, cancellationToken);
//}
