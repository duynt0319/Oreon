//using AutoMapper;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Messages;
//using MediatR;

//namespace Oreon.Application.Features.Messages.Commands.CreateMessage;

//public sealed record CreateMessageCommand(string SenderUsername, CreateMessageDto Dto) : IRequest<MessageDto>;

//public sealed class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
//{
//    private readonly IUnitOfWork _uow;
//    private readonly IMapper _mapper;

//    public CreateMessageCommandHandler(IUnitOfWork uow, IMapper mapper)
//    {
//        _uow = uow;
//        _mapper = mapper;
//    }

//    public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
//    {
//        if (request.SenderUsername.Equals(request.Dto.RecipientUsername, StringComparison.OrdinalIgnoreCase))
//            throw new InvalidOperationException("You cannot send messages to yourself.");

//        var sender = await _uow.Users.GetByUsernameAsync(request.SenderUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.SenderUsername}' not found.");

//        var recipient = await _uow.Users.GetByUsernameAsync(request.Dto.RecipientUsername, cancellationToken)
//            ?? throw new KeyNotFoundException($"User '{request.Dto.RecipientUsername}' not found.");

//        var message = Message.Create(
//            sender.Id, sender.UserName,
//            recipient.Id, recipient.UserName,
//            request.Dto.Content);

//        _uow.Messages.AddMessage(message);
//        await _uow.CompleteAsync(cancellationToken);

//        return _mapper.Map<MessageDto>(message);
//    }
//}
