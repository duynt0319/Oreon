//using Oreon.Application.Abstractions.Persistence;
//using MediatR;

//namespace Oreon.Application.Features.Messages.Commands.DeleteMessage;

//public sealed record DeleteMessageCommand(string CurrentUsername, int MessageId) : IRequest<Unit>;

//public sealed class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Unit>
//{
//    private readonly IUnitOfWork _uow;

//    public DeleteMessageCommandHandler(IUnitOfWork uow) => _uow = uow;

//    public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
//    {
//        var message = await _uow.Messages.GetByIdAsync(request.MessageId, cancellationToken)
//            ?? throw new KeyNotFoundException($"Message {request.MessageId} not found.");

//        if (message.SenderUsername != request.CurrentUsername && message.RecipientUsername != request.CurrentUsername)
//            throw new UnauthorizedAccessException("Cannot delete this message.");

//        if (message.SenderUsername == request.CurrentUsername) message.DeleteForSender();
//        if (message.RecipientUsername == request.CurrentUsername) message.DeleteForRecipient();

//        if (message.SenderDeleted && message.RecipientDeleted)
//            _uow.Messages.RemoveMessage(message);

//        await _uow.CompleteAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
