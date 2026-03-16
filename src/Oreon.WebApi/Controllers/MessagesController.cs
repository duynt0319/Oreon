//using Oreon.WebApi.Extensions;
//using Oreon.WebApi.Helpers;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Application.Features.Messages.Commands.CreateMessage;
//using Oreon.Application.Features.Messages.Commands.DeleteMessage;
//using Oreon.Application.Features.Messages.Queries.GetMessageThread;
//using Oreon.Application.Features.Messages.Queries.GetMessagesForUser;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Oreon.WebApi.Controllers
//{
//    [Authorize]
//    public class MessagesController : BaseApiController
//    {
//        [HttpPost]
//        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
//        {
//            var message = await Sender.Send(new CreateMessageCommand(User.GetUsername(), createMessageDto));
//            return Ok(message);
//        }

//        [HttpGet]
//        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
//        {
//            messageParams.Username = User.GetUsername();
//            var messages = await Sender.Send(new GetMessagesForUserQuery(messageParams));
//            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
//            return Ok(messages);
//        }

//        [HttpGet("thread/{username}")]
//        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
//        {
//            var messages = await Sender.Send(new GetMessageThreadQuery(User.GetUsername(), username));
//            return Ok(messages);
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteMessage(int id)
//        {
//            await Sender.Send(new DeleteMessageCommand(User.GetUsername(), id));
//            return Ok();
//        }
//    }
//}
