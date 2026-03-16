//using Oreon.WebApi.Extensions;
//using Oreon.Application.DTOs;
//using Oreon.Application.Features.Messages.Commands.AddToMessageGroup;
//using Oreon.Application.Features.Messages.Commands.CreateMessage;
//using Oreon.Application.Features.Messages.Commands.RemoveFromMessageGroup;
//using Oreon.Application.Features.Messages.Queries.GetMessageThread;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.SignalR;

//namespace Oreon.WebApi.SignalR
//{
//    [Authorize]
//    public class MessageHub : Hub
//    {
//        private readonly ISender _sender;
//        private readonly IHubContext<PresenceHub> _presenceHub;

//        public MessageHub(ISender sender, IHubContext<PresenceHub> presenceHub)
//        {
//            _sender = sender;
//            _presenceHub = presenceHub;
//        }

//        public override async Task OnConnectedAsync()
//        {
//            var httpContext = Context.GetHttpContext();
//            var otherUser = httpContext.Request.Query["user"];
//            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

//            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
//            var group = await _sender.Send(new AddToMessageGroupCommand(Context.ConnectionId, Context.User.GetUsername(), groupName));

//            var messages = await _sender.Send(new GetMessageThreadQuery(Context.User.GetUsername(), otherUser));
//            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
//        }

//        public override async Task OnDisconnectedAsync(Exception exception)
//        {
//            await _sender.Send(new RemoveFromMessageGroupCommand(Context.ConnectionId));
//            await base.OnDisconnectedAsync(exception);
//        }

//        public async Task SendMessage(CreateMessageDto createMessageDto)
//        {
//            var username = Context.User.GetUsername();

//            var message = await _sender.Send(new CreateMessageCommand(username, createMessageDto));

//            var groupName = GetGroupName(username, createMessageDto.RecipientUsername);
//            await Clients.Group(groupName).SendAsync("NewMessage", message);
//        }

//        private string GetGroupName(string caller, string other)
//        {
//            var stringCompare = string.CompareOrdinal(caller, other) < 0;
//            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
//        }
//    }
//}
