//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Domain.Presence;
//using MediatR;

//namespace Oreon.Application.Features.Messages.Commands.RemoveFromMessageGroup;

//public sealed record RemoveFromMessageGroupCommand(string ConnectionId) : IRequest<Group>;

//public sealed class RemoveFromMessageGroupCommandHandler : IRequestHandler<RemoveFromMessageGroupCommand, Group>
//{
//    private readonly IUnitOfWork _uow;

//    public RemoveFromMessageGroupCommandHandler(IUnitOfWork uow) => _uow = uow;

//    public async Task<Group> Handle(RemoveFromMessageGroupCommand request, CancellationToken cancellationToken)
//    {
//        var group = await _uow.Messages.GetGroupForConnectionAsync(request.ConnectionId, cancellationToken)
//            ?? throw new KeyNotFoundException($"Group for connection '{request.ConnectionId}' not found.");

//        var connection = await _uow.Messages.GetConnectionAsync(request.ConnectionId, cancellationToken);
//        if (connection != null)
//            _uow.Messages.RemoveConnection(connection);

//        await _uow.CompleteAsync(cancellationToken);

//        return group;
//    }
//}
