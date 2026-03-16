//using MediatR;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Domain.Aggregates.Presence;

//namespace Oreon.Application.Features.Messages.Commands.AddToMessageGroup;

//public sealed record AddToMessageGroupCommand(string ConnectionId, string Username, string GroupName) : IRequest<Group>;

//public sealed class AddToMessageGroupCommandHandler : IRequestHandler<AddToMessageGroupCommand, Group>
//{
//    private readonly IUnitOfWork _uow;

//    public AddToMessageGroupCommandHandler(IUnitOfWork uow) => _uow = uow;

//    public async Task<Group> Handle(AddToMessageGroupCommand request, CancellationToken cancellationToken)
//    {
//        var group = await _uow.Messages.GetMessageGroupAsync(request.GroupName, cancellationToken)
//                    ?? new Group(request.GroupName);

//        var connection = new Connection(request.ConnectionId, request.Username);
//        group.AddConnection(connection);

//        if (group.Connections.Count == 1)
//            _uow.Messages.AddGroup(group);

//        await _uow.CompleteAsync(cancellationToken);

//        return group;
//    }
//}
